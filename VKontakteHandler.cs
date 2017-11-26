using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Authentication.VKontakte.Core
{
    internal class VKontakteHandler : OAuthHandler<VKontakteOptions>
    {
        public VKontakteHandler(IOptionsMonitor<VKontakteOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            Microsoft.AspNetCore.Authentication.AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            if (Options.Fields.Count != 0)
            {
                address = QueryHelpers.AddQueryString(address, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(address, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("Произошла ошибка при получении профиля пользователя: удаленный сервер " +
                                "вернул {Status} ответ со следующей информацией: {Headers} {Body}.",
                                response.StatusCode,
                                response.Headers.ToString(),
                                await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("Произошла ошибка при получении профиля пользователя.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var user = (JObject)payload["response"][0];

            foreach (var scope in Options.Scope)
            {
                var scope_value = tokens.Response.Value<string>(scope);
                if (!string.IsNullOrEmpty(scope_value))
                {
                    user.Add(scope, scope_value);
                }
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, user.Value<string>("uid"), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, user.Value<string>("first_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, user.Value<string>("last_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, user.Value<string>("email"), Options.ClaimsIssuer);


            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, user);

            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
