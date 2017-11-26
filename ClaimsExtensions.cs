using System;
using System.Security.Claims;

namespace Authentication.VKontakte.Core
{
    public static class ClaimsExtensions
    {
        public static ClaimsIdentity AddOptionalClaim(this ClaimsIdentity identity, string name, string value, string issuer)
        {
            return AddOptionalClaim(identity, name, value, ClaimValueTypes.String, issuer);
        }

        public static ClaimsIdentity AddOptionalClaim(this ClaimsIdentity identity, string name, string value, string type, string issuer)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return identity;
            }

            identity.AddClaim(new Claim(name, value, type ?? ClaimValueTypes.String, issuer ?? ClaimsIdentity.DefaultIssuer));
            return identity;
        }
    }
}
