using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace EasyImgur.APIResponses
{
    public class BaseResponse
    {
        private class BaseReponseErrorConverter : JsonConverter
        {
            public override bool CanConvert(Type _ObjectType)
            {
                return (_ObjectType == typeof(BaseResponseError));
            }

            public override object ReadJson(JsonReader _Reader, Type _ObjectType, object _ExistingValue, JsonSerializer _Serializer)
            {
                // BaseResponseErrors can either be a string or an object containing more extensive information.
                // Internally we want to represent both version as an object, but when the source data is a simple string we only fill in the message property.

                Newtonsoft.Json.Linq.JToken jtoken = _Serializer.Deserialize<Newtonsoft.Json.Linq.JToken>(_Reader);

                if (jtoken.Type == Newtonsoft.Json.Linq.JTokenType.String)
                {
                    BaseResponseError error = new BaseResponseError();
                    error.Message = jtoken.ToObject<string>();
                    return error;
                }
                else if (jtoken.Type == Newtonsoft.Json.Linq.JTokenType.Object)
                {
                    Newtonsoft.Json.Linq.JObject jobject = jtoken.ToObject<Newtonsoft.Json.Linq.JObject>();

                    BaseResponseError error = new BaseResponseError();
                    error.Code = jobject.Value<int>("code");
                    error.Message = jobject.Value<string>("message");
                    error.Type = jobject.Value<string>("type");
                    return error;
                }

                throw new ArgumentException("A BaseResponseError is expected to be either a string or an object");
            }

            public override void WriteJson(JsonWriter _Writer, object _Value, JsonSerializer _Serializer)
            {
                _Serializer.Serialize(_Writer, _Value, typeof(BaseResponseError));
            }
        }

        [JsonConverter(typeof(BaseReponseErrorConverter))]
        public class BaseResponseError
        {
            public BaseResponseError()
            {}

            public BaseResponseError(string _ErrorString)
            {
                Message = _ErrorString;
            }

            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class BaseData
        {
            [JsonProperty("error")]
            public BaseResponseError Error { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("parameters")]
            public string Parameters { get; set; }

            [JsonProperty("request")]
            public string Request { get; set; }
        }
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
