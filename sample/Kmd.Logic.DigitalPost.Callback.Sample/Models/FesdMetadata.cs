using System;
using System.Collections.Generic;

namespace Kmd.Logic.Digitalpost.CallbackSample.Models
{
    /// <summary>
    /// The type contains FESD-specific metadata fields.
    /// </summary>
    public class FesdMetadata
    {
        /// <summary>
        /// Gets or sets the identifier of a document.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the participant.
        /// </summary>
        public string ParticipantIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the case.
        /// </summary>
        public string CaseIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the classification of the case.
        /// </summary>
        public string CaseClassificationIdentifier { get; set; }
    }
}
