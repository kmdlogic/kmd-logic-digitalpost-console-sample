using System;
using System.Collections.Generic;

namespace Kmd.Logic.Digitalpost.CallbackSample.Models
{
    public class CallbackModel
    {
        /// <summary>
        /// Gets or sets a reference to the message. Used as a response.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the time of receipt.
        /// </summary>
        public DateTime ReceipDateTime { get; set; }

        /// <summary>
        /// Gets or sets the message type (Will always be Meddelelse.)
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the name of the authority that has sent the digital message.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in kilobytes before Base64 encoding.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Gets or sets the title of the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the actual binary content of the message encoded in the Base64 format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the format of the content of the message.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the message thread identifier.
        /// </summary>
        /// <remarks>
        /// If the afsendelsen (dispatch) is a response to an inquiry from an end-user, the response must contain a reference that links it to the original inquiry.
        /// </remarks>
        public string MessageThreadIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the date which indicates that there is a deadline associated with the message.
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Gets or sets the memo which the sender can associate with the dispatch. For example, to describe the deadline.
        /// </summary>
        public string MemoText { get; set; }

        /// <summary>
        /// Gets or sets the mailbox metadata.
        /// </summary>
        public MailboxMetadata MailboxMetadata { get; set; }

        /// <summary>
        /// Gets or sets the FESD metadata.
        /// </summary>
        public FesdMetadata FesdMetadata { get; set; }

        /// <summary>
        /// Gets or sets the number of attachments contained in the message.
        /// </summary>
        public int NumberOfAttachments { get; set; }

        /// <summary>
        /// Gets the list of attachments.
        /// </summary>
        public List<Attachment> Attachments { get; } = new List<Attachment>();

        /// <summary>
        /// Gets or sets a value indicating whether the sending authority allows this message to be responded to.
        /// </summary>
        public bool CanBeResponded { get; set; }

        /// <summary>
        /// Gets or sets the logic subscription identifier.
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the Eboks system identifier.
        /// </summary>
        public int SystemId { get; set; }
    }
}
