using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Authentication.VKontakte.Core
{
    public class VKontakteOptions : OAuthOptions
    {
        public VKontakteOptions()
        {
            ClaimsIssuer = VKontakteDefaults.ClaimsIssuer;
            CallbackPath = new PathString(VKontakteDefaults.CallbackPath);
            AuthorizationEndpoint = VKontakteDefaults.AuthorizationEndpoint;
            TokenEndpoint = VKontakteDefaults.TokenEndpoint;
            UserInformationEndpoint = VKontakteDefaults.UserInformationEndpoint;
            Scope.Add("offline");
            Scope.Add("email");

            Fields.Add("uid");
            Fields.Add("email");
            Fields.Add("first_name");
            Fields.Add("last_name");
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(AppId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Отсутствует {0}", nameof(AppId)), nameof(AppId));
            }

            if (string.IsNullOrEmpty(AppSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Отсутствует {0}", nameof(AppSecret)), nameof(AppSecret));
            }

            base.Validate();
        }

        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        public string AppSecret
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        public bool SendAppSecretProof { get; set; }

        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}
