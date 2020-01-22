using System;
using Kmd.Logic.DigitalPost.Client.Models;

namespace Kmd.Logic.DigitalPost.Client
{
    public static class UploadAttachmentResponseExtensions
    {
        /// <summary>
        /// Convert an <see cref="UploadAttachmentResponse">UploadAttachmentResponse</see> into a <see cref="MessageAttachment">MessageAttachment</see>.
        /// </summary>
        /// <param name="response">The response to convert.</param>
        /// <param name="fileName">The file name of the attachment.</param>
        /// <returns>A MessageAttachment.</returns>
        /// <exception cref="ArgumentNullException">Missing response or fileName.</exception>
        /// <exception cref="ArgumentException">Invalid response.</exception>
        public static MessageAttachment ToAttachment(this UploadAttachmentResponse response, string fileName)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (response.ReferenceId == null)
            {
                throw new ArgumentException("Transmission of attachment failed");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return new MessageAttachment
            {
                ReferenceId = response.ReferenceId,
                FileName = fileName,
            };
        }
    }
}
