using System;
using System.Collections.Generic;

namespace Kmd.Logic.Digitalpost.CallbackSample.Models
{

    
    public class Metadata
    {
        /// <summary>
        /// The key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }
    }


    public class MailboxMetadata
    {
        /// <summary>
        /// Clear specification of the mailbox.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Clear specification of the subject.
        /// </summary>
        public string SubjectIdentifier { get; set; }

        public List<Metadata> Metadata { get; set; }
    }

    /// <summary>
    /// The type contains FESD-specific metadata fields.
    /// </summary>
    public class FesdMetadata
    {
        /// <summary>
        /// Identifies a document.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Identifies the participant.
        /// </summary>
        public string ParticipantIdentifier { get; set; }

        /// <summary>
        /// Identifies the case.
        /// </summary>
        public string CaseIdentifier { get; set; }

        /// <summary>
        /// Classification of the case.
        /// </summary>
        public string CaseClassificationIdentifier { get; set; }
    }
    

    public class Attachment
    {
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the format of the content of the attachment. The format name is the suffix of the file name which you would give to the content were it to be saved as a file. For example, "pdf", "docx".
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The actual content of the attachment encoded in the Base64 format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The size of the file in kilobytes before Base64 encoding.
        /// </summary>
        public int FileSize { get; set; }
    }

    public class CallbackModel
    {
        /// <summary>
        /// Reference to the message. Used as a response
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Time of receipt
        /// </summary>
        public DateTime ReceipDateTime { get; set; }

        /// <summary>
        /// Message type (Will always be Meddelelse.)
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Name of the authority that has sent the digital message.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The size of the file in kilobytes before Base64 encoding.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// The title of the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The actual binary content of the message encoded in the Base64 format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Specifies the format of the content of the message.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Message Thread Identifier
        /// If the afsendelsen (dispatch) is a response to an inquiry from an end-user, the response must contain a reference that links it to the original inquiry
        /// </summary>
        public string MessageThreadIdentifier { get; set; }

        /// <summary>
        /// A date which indicates that there is a deadline associated with the message.
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// A memo which the sender can associate with the dispatch. For example, to describe the deadline.
        /// </summary>
        public string MemoText { get; set; }

        /// <summary>
        /// Mailbox metadata
        /// </summary>
        public MailboxMetadata MailboxMetadata { get; set; }

        /// <summary>
        /// FESD Metadata
        /// </summary>
        public FesdMetadata FesdMetadata { get; set; }

        /// <summary>
        /// The number of attachments contained in the message.
        /// </summary>
        public int NumberOfAttachments { get; set; }

        /// <summary>
        /// List of attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Specifies whether the sending authority allows this message to be responded to.
        /// </summary>
        public string CanBeResponded { get; set; }
        
        /// <summary>
        /// Logic Subscription identifier
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Eboks system identifier
        /// </summary>
        public int SystemId { get; set; }
    }
}
