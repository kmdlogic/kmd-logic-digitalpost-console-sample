# KMD Logic Digital Post Client

A dotnet client library for sending digital post (e-Boks) messages via the Logic platform.

## How to use this client library

In projects or components where you need to access send messages, add a NuGet package reference to [Kmd.Logic.DigitalPost.Client](https://www.nuget.org/packages/Kmd.Logic.DigitalPost.Client).

The simplest example to send a message is:

```csharp
using (var httpClient = new HttpClient())
using (var tokenProviderFactory = new LogicTokenProviderFactory(configuration.TokenProvider))
{
    var client = new DigitalPostClient(httpClient, tokenProviderFactory, configuration.DigitalPost);
    var response = await client.SendMessageAsync(IdentifierType.Cpr, "0101010000", "Hi there", "Digital Post test").ConfigureAwait(false);
}
```

The `LogicTokenProviderFactory` authorizes access to the Logic platform through the use of a Logic Identity issued client credential. The authorization token is reused until it expires. You would generally create a single instance of `LogicTokenProviderFactory`.

The `DigitalPostClient` accesses the Logic Digital Post service.

## How to configure the Digital Post client

Perhaps the easiest way to configure the Digital Post client is from Application Settings.

```json
{
  "TokenProvider": {
    "ClientId": "",
    "ClientSecret": ""
  },
  "Digital Post": {
    "SubscriptionId": "",
    "ConfigurationId": ""
  }
}
```

To get started:

1. Create a subscription in [Logic Console](https://console.kmdlogic.io). This will provide you the `SubscriptionId`.
2. Request a client credential. Once issued you can view the `ClientId` and `ClientSecret` in [Logic Console](https://console.kmdlogic.io).
3. Create a digital post configuration. This will give you the `ConfigurationId`.

## Sample application

A simple console application is included to demonstrate how to call Logic Digital Post API to transmit a document. You will need to provide the settings described above in `appsettings.json`.

When run this you should see the message id for the transmission printed to the console.

## Callback sample

When configuring your digital post configuration you can optionally specify a callback URL. 
This allows you to be notified when a citizen or company interacts with your message. 
Most importantly, this is how you make use of the inbuilt Digital Post consent feature.
To enable consent you must arrange with the underlying provider for a `MaterialID` to be configured with this option. 
NOTE: There is a cost associated with this capability.

## Doc2Mail Provider

Doc2Mail is an e-Boks provider which can be used in any scenario. Please contact [Charlie Tango](mailto:e-boks@charlietango.dk) or call [25 10 43 01](tel:+4525104301) to arrange your credentials. There is a cost to use this service.

## e-Boks Provider

The e-Boks platform may be accessed directly by approved Government agencies. This can be arranged through Nets.

## Digital Post Fake Provider

The Fake Provider is a simple solution for use in Demo or Test environments and also allows you to begin development immediately whilst you wait for your formal credentials.

The fake provider accepts any message sent and always returns a successful response. No callbacks are executed with the fake provider.
