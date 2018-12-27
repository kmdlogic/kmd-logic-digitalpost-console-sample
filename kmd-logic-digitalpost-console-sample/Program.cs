using Kmd.Logic.Digitalpost.ConsoleSample.Client;
using Kmd.Logic.Digitalpost.ConsoleSample.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Kmd.Logic.Digitalpost.ConsoleSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build()
                    .Get<AppConfiguration>();

                await Run(config);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Caught a fatal unhandled exception");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task Run(AppConfiguration config)
        {
            ValidateConfiguration(config);

            Log.Information("Logic environment is {LogicEnvironmentName}", config.LogicEnvironmentName);
            var logicEnvironment = config.LogicEnvironments.FirstOrDefault(e => e.Name == config.LogicEnvironmentName);
            if (logicEnvironment == null)
            {
                Log.Error("No logic environment named {LogicEnvironmentName}", config.LogicEnvironmentName);
                return;
            }            
            
            var client = new DigitalPostClient(new TokenCredentials(new LogicTokenProvider(config)));            
            client.BaseUri = logicEnvironment.ApiRootUri;

            var subscriptionId = config.LogicAccount.SubscriptionId.Value;

            UploadAttachmentResponse uploadAttachmentResponse = null;
            using (var stream = File.OpenRead("attachment_logic.pdf")) {
                uploadAttachmentResponse = await client.UploadAttachmentAsync(subscriptionId, stream);
            }

            Log.Information("Uploaded attachment and got referenceId {@ReferenceId}", uploadAttachmentResponse.ReferenceId);
            
            var result = await client.SendSingleDocumentAsync(subscriptionId, new SendDocumentRequest
            {
                MaterialId = config.DigitalPost.MaterialId,
                SystemId = config.DigitalPost.SystemId,
                IdentifierType = "cpr",
                Identifier = "1103500113",
                Title = "Message from Logic Console Sample",
                ContentExtension = "pdf",
                ContentReferenceId = uploadAttachmentResponse.ReferenceId
            });

            Log.Information("Document was send with eboks messageId {@MessageId}", result.MessageId);
        }

        

        private static void ValidateConfiguration(AppConfiguration config)
        {
            if (config.LogicAccount == null
                || string.IsNullOrWhiteSpace(config.LogicAccount?.ClientId)
                || string.IsNullOrWhiteSpace(config.LogicAccount?.ClientSecret)
                || config.LogicAccount?.SubscriptionId == null)
            {
                Log.Error("Please add your LogicAccount configuration to `appsettings.json`. You currently have {@LogicAccount}",
                    config.LogicAccount);
                return;
            }

            if (config.DigitalPost == null
                || config.DigitalPost?.MaterialId == null
                || config.DigitalPost?.SystemId == null)
            {
                Log.Error("Please add your DigitalPost configuration to `appsettings.json`. You currently have {@DigitalPost}",
                    config.DigitalPost);
                return;
            }
        }        
    }
}
