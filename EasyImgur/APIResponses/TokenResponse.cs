using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace EasyImgur.APIResponses
{
    class TokenResponse
    {
        // Received when requesting new tokens through a pin.
        [JsonProperty("bearer")]
        public string Bearer { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }


        // Received when refreshing tokens.
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("account_username")]
        public string AccountUsername { get; set; }


        // Always received.
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public TokenResponse()
        {
            Bearer = Scope = TokenType = AccountUsername = AccessToken = RefreshToken = string.Empty;
            ExpiresIn = 0;
        }
    }
}
