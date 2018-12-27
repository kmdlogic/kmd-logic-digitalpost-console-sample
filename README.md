# KMD Logic DigitalPost Console Sample

This application allows you to send an documents/messages/sms via the KMD Logic DigitalPost service (using EBoks).

## Usage

1. Configure the `appsettings.json` with your KMD Logic subscription settings. Read the comments in the `appsettings.json` for further details on which settings you need to configure.
2. [Install dotnet core](https://dotnet.microsoft.com/download)
3. Execute `dotnet run` in the root project folder.

If all goes well, you should recieve console output including a unique eboks `messageId`.
