using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.APIResponses
{
    class TokenResponse
    {
        // Received when requesting new tokens through a pin.
        public string bearer;
        public string scope;

        // Received when refreshing tokens.
        public string token_type;
        public string account_username;

        // Always received.
        public string access_token;
        public string refresh_token;
        public int expires_in;
    }
}
