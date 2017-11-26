using Authentication.VKontakte.Core;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VKontakteAuthenticationOptionsExtensions
    {
        public static AuthenticationBuilder AddVKontakte(this AuthenticationBuilder builder)
            => builder.AddVKontakte(VKontakteDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddVKontakte(this AuthenticationBuilder builder, Action<VKontakteOptions> configureOptions)
            => builder.AddVKontakte(VKontakteDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddVKontakte(this AuthenticationBuilder builder, string authenticationScheme, Action<VKontakteOptions> configureOptions)
            => builder.AddVKontakte(authenticationScheme, VKontakteDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddVKontakte(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<VKontakteOptions> configureOptions)
            => builder.AddOAuth<VKontakteOptions, VKontakteHandler>(authenticationScheme, displayName, configureOptions);
    }
}
