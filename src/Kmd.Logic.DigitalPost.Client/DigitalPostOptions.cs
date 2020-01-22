using System;

namespace Kmd.Logic.DigitalPost.Client
{
    /// <summary>
    /// Provide the configuration options for using the DigitalPost service.
    /// </summary>
    public sealed class DigitalPostOptions
    {
        /// <summary>
        /// Gets or sets the Logic DigitalPost service.
        /// </summary>
        /// <remarks>
        /// This option should not be overridden except for testing purposes.
        /// </remarks>
        public Uri DigitalPostServiceUri { get; set; } = new Uri("https://gateway.kmdlogic.io/digital-post/v1");

        /// <summary>
        /// Gets or sets the Logic Subscription.
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the Digital Post Configuration identifier.
        /// </summary>
        public Guid ConfigurationId { get; set; }
    }
}