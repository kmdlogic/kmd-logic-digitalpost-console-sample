using System;
using Kmd.Logic.Identity.Authorization;

namespace Kmd.Logic.DigitalPost.Client.Sample
{
    internal class AppConfiguration
    {
        public string Title { get; set; }
        public string DocumentPath { get; set; }

        public IdentifierType? IdentifierType { get; set; }

        public string Identifier { get; set; }

        public string MaterialId { get; set; }

        public string PNumber { get; set; }

        public string Metadata { get; set; }

        public LogicTokenProviderOptions TokenProvider { get; set; } = new LogicTokenProviderOptions();

        public DigitalPostOptions DigitalPost { get; set; } = new DigitalPostOptions();
    }
}