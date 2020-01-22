using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kmd.Logic.DigitalPost.Client.Models;
using Kmd.Logic.Identity.Authorization;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Kmd.Logic.DigitalPost.Client.Sample
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            InitLogger();

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddUserSecrets(typeof(Program).Assembly)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build()
                    .Get<AppConfiguration>();

                await Run(config).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Log.Fatal(ex, "Caught a fatal unhandled exception");
            }
#pragma warning restore CA1031 // Do not catch general exception types
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static async Task Run(AppConfiguration configuration)
        {
            var validator = new ConfigurationValidator(configuration);
            if (!validator.Validate())
            {
                return;
            }

            using (var httpClient = new HttpClient())
            using (var tokenProviderFactory = new LogicTokenProviderFactory(configuration.TokenProvider))
            {
                var client = new DigitalPostClient(httpClient, tokenProviderFactory, configuration.DigitalPost);

                var response = await client.SendMessageAsync(IdentifierType.Cpr, "0101010000", "Hi there", "Digital Post test").ConfigureAwait(false);
                var configurations = await client.GetAllConfigurationsAsync().ConfigureAwait(false);

                if (configuration.DigitalPost.ConfigurationId == Guid.Empty)
                {
                    if (configurations != null && configurations.Count > 0)
                    {
                        if (configurations.Count > 1)
                        {
                            Log.Error("There is more than one digital post configuration defined for this subscription");
                            return;
                        }
                        else
                        {
                            configuration.DigitalPost.ConfigurationId = configurations[0].Id.Value;
                        }
                    }
                    else
                    {
                        Log.Error("There is no digital post configurations defined for this subscription");
                        return;
                    }
                }
                else
                {
                    var usedConfig = configurations.FirstOrDefault(x => x.Id == configuration.DigitalPost.ConfigurationId);

                    if (usedConfig == null)
                    {
                        Log.Error("The specified digital post configuration does not exist");
                        return;
                    }
                    else
                    {
                        Log.Information("Using configuration {Name} for environment {Environment}", usedConfig.Name, usedConfig.Environment);
                    }
                }

                MessageAttachment document = null;
                using (var stream = File.OpenRead(configuration.DocumentPath))
                {
                    var uploadResponse = await client.UploadAttachmentAsync(stream).ConfigureAwait(false);
                    if (uploadResponse == null || uploadResponse.ReferenceId == null)
                    {
                        Log.Error("Unable to upload attachment {Path}", configuration.DocumentPath);
                        return;
                    }

                    document = uploadResponse.ToAttachment(Path.GetFileName(configuration.DocumentPath));
                }

                Log.Information("Attachment was uploaded and got ReferenceId {ReferenceId}", document.ReferenceId);

                var result = await client.SendDocumentAsync(
                    configuration.IdentifierType.Value,
                    configuration.Identifier,
                    document,
                    configuration.Title,
                    configuration.MaterialId,
                    configuration.PNumber,
                    configuration.Metadata)
                    .ConfigureAwait(false);

                Log.Information("Document was sent and got MessageId {MessageId}", result.MessageId);
            }
        }
    }
}