using System.Collections.Generic;

namespace Kmd.Logic.Digitalpost.CallbackSample.Models
{
    public class MailboxMetadata
    {
        /// <summary>
        /// Gets or sets the specification of the mailbox.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the specification of the subject.
        /// </summary>
        public string SubjectIdentifier { get; set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public List<Metadata> Metadata { get; } = new List<Metadata>();
    }
}
