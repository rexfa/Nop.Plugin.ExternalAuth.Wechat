using System;
using Microsoft.AspNetCore.Authentication;
using Nop.Plugin.ExternalAuth.Wechat.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WechatAuthenticationOptionsExtensions
    {
        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder)
    => builder.AddWechat(WechatDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder, Action<WechatOptions> configureOptions)
            => builder.AddWechat(WechatDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder, string authenticationScheme, Action<WechatOptions> configureOptions)
            => builder.AddWechat(authenticationScheme, WechatDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddWechat(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<WechatOptions> configureOptions)
            => builder.AddOAuth<WechatOptions, WechatHandler>(authenticationScheme, displayName, configureOptions);
    }
}
