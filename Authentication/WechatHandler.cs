﻿using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Authentication.Wechat
{
    /// <summary>
    /// Wechat 的基于 OAuth 的身份验证的身份验证处理程序。
    /// </summary>
    public class WechatHandler : OAuthHandler<WechatOptions>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="WechatHandler"/>.
        /// </summary>
        /// <inheritdoc />
        public WechatHandler(IOptionsMonitor<WechatOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
           : base(options, logger, encoder, clock)
        { }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            if (Options.SendAppSecretProof)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
            }
            if (Options.Fields.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Facebook user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
            }
            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted)))
            {
                var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                context.RunClaimActions();
                await Events.CreatingTicket(context);
                return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
            }

        }

        private string GenerateAppSecretProof(string accessToken)
        {
            var key = Encoding.ASCII.GetBytes(Options.AppSecret);
            var tokenBytes = Encoding.ASCII.GetBytes(accessToken);
            var hash = HMACSHA256.HashData(key, tokenBytes);
            var builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }
        /// <inheritdoc />
        protected override string FormatScope(IEnumerable<string> scopes)
        {
            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
            // https://developers.facebook.com/docs/reference/dialogs/oauth
            // http://tools.ietf.org/html/rfc6749#section-3.3
            return string.Join(",", scopes);
        }
        /// <inheritdoc />
        protected override string FormatScope()
            => base.FormatScope();
    }
}
