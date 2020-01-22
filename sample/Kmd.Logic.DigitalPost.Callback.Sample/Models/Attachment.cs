using System;
using System.Collections.Generic;

namespace Kmd.Logic.Digitalpost.CallbackSample.Models
{
    public class Attachment
    {
        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the format of the content of the attachment. The format name is the suffix of the file name which you would give to the content were it to be saved as a file. For example, "pdf", "docx".
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the actual content of the attachment encoded in the Base64 format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in kilobytes before Base64 encoding.
        /// </summary>
        public int FileSize { get; set; }
    }
}
