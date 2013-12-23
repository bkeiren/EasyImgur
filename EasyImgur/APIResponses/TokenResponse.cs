using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.APIResponses
{
    class TokenResponse
    {
        // Received when requesting new tokens through a pin.
        public string bearer = string.Empty;
        public string scope = string.Empty;

        // Received when refreshing tokens.
        public string token_type = string.Empty;
        public string account_username = string.Empty;

        // Always received.
        public string access_token = string.Empty;
        public string refresh_token = string.Empty;
        public int expires_in = 0;
    }
}
