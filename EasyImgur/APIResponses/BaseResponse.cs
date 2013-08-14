using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.APIResponses
{
    public class BaseResponse
    {
        public class BaseData
        {
            public string error;
            public string method;
            public string parameters;
            public string request;
        }
        public bool success;
        public int status;
    }
}
