using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Kmd.Logic.DigitalPost.Client.Models;
using Kmd.Logic.Identity.Authorization;
using Microsoft.Rest;

namespace Kmd.Logic.DigitalPost.Client
{
    /// <summary>
    /// Securely send a message or document to a citizen or company.
    /// </summary>
    /// <remarks>
    /// To access the digital post service you:
    /// - Create a Logic subscription
    /// - Have a client credential issued for the Logic platform
    /// - Create a DigitalPost Configuration.
    /// </remarks>
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "HttpClient is not owned by this class.")]
    public sealed class DigitalPostClient
    {
        private readonly HttpClient httpClient;
        private readonly DigitalPostOptions options;
        private readonly LogicTokenProviderFactory tokenProviderFactory;

        private InternalClient internalClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalPostClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use. The caller is expected to manage this resource and it will not be disposed.</param>
        /// <param name="tokenProviderFactory">The Logic access token provider factory.</param>
        /// <param name="options">The required configuration options.</param>
        public DigitalPostClient(HttpClient httpClient, LogicTokenProviderFactory tokenProviderFactory, DigitalPostOptions options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.tokenProviderFactory = tokenProviderFactory ?? throw new ArgumentNullException(nameof(tokenProviderFactory));
        }

        /// <summary>
        /// Prepare a document to be transmitted to a citizen or company.
        /// </summary>
        /// <param name="stream">The stream of the document to send.</param>
        /// <returns>The attachment reference.</returns>
        /// <remarks>
        /// An attachment may be referenced in multiple transmissions.
        /// </remarks>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        public async Task<UploadAttachmentResponse> UploadAttachmentAsync(Stream stream)
        {
            var client = this.CreateClient();

            return await client.UploadAttachmentAsync(this.options.SubscriptionId, stream).ConfigureAwait(false);
        }

        /// <summary>
        /// Send a message to a citizen or company.
        /// </summary>
        /// <param name="identifierType">Type of identifier - CPR for a citizen, CVR for a company.</param>
        /// <param name="identifier">The identifier (CPR/CVR).</param>
        /// <param name="message">The message to send.</param>
        /// <param name="title">The title of the message.</param>
        /// <param name="materialId">The identifier of the material defined in eBoks or the Document Type defined by Doc2Mail.</param>
        /// <param name="pNumber">The Production Unit Identifier agreed between the sender and receiver to distribute a message to a particular department, production site, etc. within a company.</param>
        /// <param name="metadata">The meta-data attached to the message.</param>
        /// <param name="attachments">Any attachments to transmit with the message.</param>
        /// <returns>The message response including the MessageId.</returns>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="DigitalPostValidationException">Invalid parameters.</exception>
        /// <exception cref="DigitalPostConfigurationException">Invalid configuration details.</exception>
        public async Task<SendMessageResponse> SendMessageAsync(IdentifierType identifierType, string identifier, string message, string title, string materialId = null, string pNumber = null, string metadata = null, IEnumerable<MessageAttachment> attachments = null)
        {
            var client = this.CreateClient();

            try
            {
                List<MessageAttachment> attList = null;
                if (attachments != null)
                {
                    attList = new List<MessageAttachment>(attachments);
                    if (attList.Count == 0)
                    {
                        attList = null;
                    }
                }

                using (var response = await client.SendMessageWithHttpMessagesAsync(
                                     subscriptionId: this.options.SubscriptionId,
                                     new SendMessageRequest
                                     {
                                         ConfigurationId = this.options.ConfigurationId,
                                         IdentifierType = identifierType.ToString(),
                                         Identifier = identifier,
                                         Message = message,
                                         Title = title,
                                         MaterialId = materialId,
                                         PNumber = pNumber,
                                         Metadata = metadata,
                                         Attachments = attList,
                                     }).ConfigureAwait(false))
                {
                    switch (response.Response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            return response.Body as SendMessageResponse;

                        case System.Net.HttpStatusCode.BadRequest:
                            throw new DigitalPostValidationException(response.Body as IDictionary<string, IList<string>>);

                        default:
                            throw new DigitalPostConfigurationException("Invalid configuration provided to access DigitalPost service", response.Body as string);
                    }
                }
            }
            catch (ValidationException ex)
            {
                throw new DigitalPostValidationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Send a document to a citizen or company.
        /// </summary>
        /// <param name="identifierType">Type of identifier - CPR for a citizen, CVR for a company.</param>
        /// <param name="identifier">The identifier (CPR/CVR).</param>
        /// <param name="document">The document to send.</param>
        /// <param name="title">The title of the message.</param>
        /// <param name="materialId">The identifier of the material defined in eBoks or the Document Type defined by Doc2Mail.</param>
        /// <param name="pNumber">The Production Unit Identifier agreed between the sender and receiver to distribute a document to a particular department, production site, etc. within a company.</param>
        /// <param name="metadata">The meta-data attached to the document.</param>
        /// <param name="attachments">Any attachments to transmit with the document.</param>
        /// <returns>The message response including the MessageId.</returns>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="DigitalPostValidationException">Invalid parameters.</exception>
        /// <exception cref="DigitalPostConfigurationException">Invalid configuration details.</exception>
        public async Task<SendMessageResponse> SendDocumentAsync(IdentifierType identifierType, string identifier, MessageAttachment document, string title, string materialId = null, string pNumber = null, string metadata = null, IEnumerable<MessageAttachment> attachments = null)
        {
            var client = this.CreateClient();

            try
            {
                if (document == null
                    || document.ReferenceId == null
                    || document.ReferenceId.Value == Guid.Empty
                    || string.IsNullOrEmpty(document.FileName))
                {
                    throw new DigitalPostValidationException("No document provided to transmit");
                }

                var extn = Path.GetExtension(document.FileName);
                if (extn.StartsWith(".", StringComparison.Ordinal))
                {
                    extn = extn.Substring(1);
                }

                if (string.IsNullOrEmpty(extn))
                {
                    throw new DigitalPostValidationException("Unable to determine the file extension for the document being transmitted");
                }

                List<MessageAttachment> attList = null;
                if (attachments != null)
                {
                    attList = new List<MessageAttachment>(attachments);
                    if (attList.Count == 0)
                    {
                        attList = null;
                    }
                }

                using (var response = await client.SendDocumentWithHttpMessagesAsync(
                                    subscriptionId: this.options.SubscriptionId,
                                    new SendDocumentRequest
                                    {
                                        ConfigurationId = this.options.ConfigurationId,
                                        IdentifierType = identifierType.ToString(),
                                        Identifier = identifier,
                                        ContentReferenceId = document.ReferenceId,
                                        ContentExtension = extn,
                                        Title = title,
                                        MaterialId = materialId,
                                        PNumber = pNumber,
                                        Metadata = metadata,
                                        Attachments = attList,
                                    }).ConfigureAwait(false))
                {
                    switch (response.Response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            return response.Body as SendMessageResponse;

                        case System.Net.HttpStatusCode.BadRequest:
                            throw new DigitalPostValidationException(response.Body as IDictionary<string, IList<string>>);

                        default:
                            throw new DigitalPostConfigurationException("Invalid configuration provided to access DigitalPost service", response.Body as string);
                    }
                }
            }
            catch (ValidationException ex)
            {
                throw new DigitalPostValidationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the digital post configurations for the Logic subscription.
        /// </summary>
        /// <returns>The list of digital post configurations.</returns>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        public async Task<IList<DigitalPostProviderConfigurationModel>> GetAllConfigurationsAsync()
        {
            var client = this.CreateClient();

            return await client.GetAllDigitalPostConfigurationsAsync(this.options.SubscriptionId).ConfigureAwait(false);
        }

        private InternalClient CreateClient()
        {
            if (this.internalClient != null)
            {
                return this.internalClient;
            }

            var tokenProvider = this.tokenProviderFactory.GetProvider(this.httpClient);

            this.internalClient = new InternalClient(new TokenCredentials(tokenProvider))
            {
                BaseUri = this.options.DigitalPostServiceUri,
            };

            return this.internalClient;
        }
    }
}