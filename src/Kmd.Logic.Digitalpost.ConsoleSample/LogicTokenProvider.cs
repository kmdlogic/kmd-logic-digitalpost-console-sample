using Microsoft.Rest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Kmd.Logic.Digitalpost.ConsoleSample
{
    class LogicTokenProvider : ITokenProvider
    {
        private AppConfiguration config;
        private TokenResponse currentToken;
        private DateTime? tokenExpiration;

        public LogicTokenProvider(AppConfiguration config)
        {
            this.config = config;
        }

        public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken)
        {
            var logicEnvironment = config.LogicEnvironments.FirstOrDefault(e => e.Name == config.LogicEnvironmentName);

            if (tokenExpiration == null || tokenExpiration.Value < DateTime.Now)
            {
                Log.Debug("Token expired or does not exist.");
                currentToken = null;
            }

            if (currentToken == null)
            {
                Log.Information("Requesting auth token for account ClientID `{LogicAccount}` and subscription `{SubscriptionId}`",
                    config.LogicAccount.ClientId, config.LogicAccount.SubscriptionId);

                var tokenExpireLocal = DateTime.Now;
                var token = await RequestToken(
                     logicEnvironment.AuthorizationServerTokenIssuerUri,
                     config.LogicAccount.ClientId,
                     logicEnvironment.ScopeUri.ToString(),
                     config.LogicAccount.ClientSecret);
                tokenExpiration = tokenExpireLocal.AddSeconds(token.expires_in);

                Log.Debug("Got access token {@Token}", token);
                currentToken = token;
            }
            return new AuthenticationHeaderValue(currentToken.token_type, currentToken.access_token);
        }

        private static async Task<TokenResponse> RequestToken(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpResponseMessage responseMessage;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("scope", scope),
                        new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                Log.Debug("Requesting an access token {@Request}", tokenRequest);
                responseMessage = await client.SendAsync(tokenRequest);
            }

            return await responseMessage.Content.ReadAsAsync<TokenResponse>();
        }

        public class TokenResponse
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
        }
    }
}
