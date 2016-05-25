using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace EasyImgur
{
    public static class ImgurAPI
    {
        static private string mEndPoint = "https://api.imgur.com/3/";

        static private string mClientId = "5fae4323a27c0cf";
        static private string mClientSecret = "3e9200a0bf59d5b23de53287ec47898997ee4b98";

        static private int mNumUploads = 0;

        static private string mCurrentAccessToken = string.Empty;
        static private string mCurrentRefreshToken = string.Empty;
        static private DateTime mTokensExpireAt = DateTime.MinValue;

        static private System.Threading.Thread mTokenThread = null;

        static public event AuthorizationEventHandler ObtainedAuthorization;
        static public event AuthorizationEventHandler LostAuthorization;
        static public event AuthorizationEventHandler RefreshedAuthorization;
        static public event NetworkEventHandler NetworkRequestFailed;
        public delegate void AuthorizationEventHandler();
        public delegate void NetworkEventHandler();

        static public int NumSuccessfulUploads
        {
            get
            {
                return mNumUploads;
            }
        }

        static private APIResponses.ImageResponse InternalUploadImage(object obj, bool _URL, string title, string description, bool anonymous, string album = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }

            var url = mEndPoint + "image";

            var responseString = string.Empty;
            byte[] response = null;

            APIResponses.ImageResponse resp = null;
            using (var memStream = new System.IO.MemoryStream())
            {
                if (!_URL)
                {
                    var image = obj as Image;
                    var format = image.RawFormat;
                    switch (Properties.Settings.Default.imageFormat)
                    {
                        case 1:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            }
                        case 2:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Png;
                                break;
                            }
                        case 3:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            }
                        case 4:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            }
                        case 5:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Icon;
                                break;
                            }
                        case 6:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Tiff;
                                break;
                            }
                        case 7:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Emf;
                                break;
                            }
                        case 8:
                            {
                                format = System.Drawing.Imaging.ImageFormat.Wmf;
                                break;
                            }
                        case 0:
                        default:
                            // Auto mode.
                            {
                                // Check whether it is a valid format.
                                if (format.Equals(System.Drawing.Imaging.ImageFormat.Bmp) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Gif) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Icon) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Png) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Tiff) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Emf) ||
                                    format.Equals(System.Drawing.Imaging.ImageFormat.Wmf))
                                {
                                    // It's fine.
                                }
                                else
                                {
                                    // In all other cases, use PNG.
                                    format = System.Drawing.Imaging.ImageFormat.Png;
                                }
                                break;
                            }
                    }

                    image.Save(memStream, format);
                }

                var status = 0;
                var error = "An unknown error occurred.";
                using (var t = WebClientFactory.Create())
                {
                    t.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(anonymous);
                    try
                    {
                        var values = new System.Collections.Specialized.NameValueCollection
                        {
                            {
                                "image", _URL ? obj as string : Convert.ToBase64String(memStream.ToArray())
                            },
                            {
                                "title", title
                            },
                            {
                                "description", description
                            },
                            {
                                "type", _URL ? "URL" : "base64"
                            }
                        };
                        if (album != "")
                            values.Add("album", album);

                        response = t.UploadValues(url, "POST", values);
                        responseString = Encoding.ASCII.GetString(response);
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response == null)
                        {
                            if (NetworkRequestFailed != null) NetworkRequestFailed();
                        }
                        else
                        {
                            int.TryParse(ex.Message.Split('(')[1].Split(')')[0], out status); // gets status code from message string in case of emergency
                            error = ex.Message.Split('(')[1].Split(')')[1]; // I believe this gets the rest of the error message supplied, but Imgur went back up before I could test it
                            var stream = ex.Response.GetResponseStream();
                            var currByte = -1;
                            var strBuilder = new StringBuilder();
                            while ((currByte = stream.ReadByte()) != -1)
                            {
                                strBuilder.Append((char)currByte);
                            }
                            responseString = strBuilder.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Unexpected Exception: " + ex.ToString());
                    }
                }

                try
                {
                    resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.ImageResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
                }
                catch (Exception ex)
                {
                    Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                    resp = null;
                }

                if (resp == null || responseString == null || responseString == string.Empty)
                {
                    // generally indicates a server failure; on problems such as 502 Proxy Error and 504 Gateway Timeout HTML is returned
                    // which can't be parsed by the JSON converter.
                    resp = new APIResponses.ImageResponse();
                    resp.Success = false;
                    resp.Status = status;
                    resp.ResponseData = new APIResponses.ImageResponse.Data() { Error = error };
                }

                if (resp.Success)
                {
                    Log.Info("Successfully uploaded image! (" + resp.Status.ToString() + ")[\n\rid: " + resp.ResponseData.Id + "\n\rlink: " + resp.ResponseData.Link + "\n\rdeletehash: " + resp.ResponseData.DeleteHash + "\n\r]");
                    ++mNumUploads;
                }
                else
                {
                    Log.Error("Failed to upload image (" + resp.Status.ToString() + ")");
                }
            }

            return resp;
        }

        static public APIResponses.ImageResponse UploadImage(Image image, string title, string description, bool anonymous)
        {
            return InternalUploadImage(image, false, title, description, anonymous);
        }

        static public APIResponses.ImageResponse UploadImage(string url, string title, string description, bool anonymous)
        {
            return InternalUploadImage(url, true, title, description, anonymous);
        }

        static public APIResponses.AlbumResponse UploadAlbum(Image[] images, string albumTitle, bool anonymous, string[] titles, string[] descriptions)
        {
            var url = mEndPoint + "album";
            var responseString = "";

            using (var t = WebClientFactory.Create())
            {
                t.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(anonymous);
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "title", albumTitle
                        },
                        {
                            "layout", "vertical"
                        }
                    };
                    responseString = Encoding.ASCII.GetString(t.UploadValues(url, "POST", values));
                    //responseString = t.UploadString(url + "/ZHPG7sztcWB26YM", "DELETE", "");
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    else
                    {
                        var stream = ex.Response.GetResponseStream();
                        var currByte = -1;
                        var strBuilder = new StringBuilder();
                        while ((currByte = stream.ReadByte()) != -1)
                        {
                            strBuilder.Append((char)currByte);
                        }
                        responseString = strBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            APIResponses.AlbumResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.AlbumResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                resp = null;
            }

            if (resp == null || responseString == "" || responseString == null)
                resp = new APIResponses.AlbumResponse() { Success = false };

            if (resp.Success)
                Log.Info("Successfully created album! (" + resp.Status.ToString() + ")");
            else
            {
                Log.Error("Failed to create album! (" + resp.Status.ToString() + ")");
                return resp;
            }

            // sometimes this happens! it's weird.
            if (anonymous && resp.ResponseData.DeleteHash == null)
            {
                Log.Error("Anonymous album creation didn't return deletehash. Can't add to album.");
                resp.Success = false;
                resp.ResponseData.Error = "Imgur API error. Try again in a minute.";
                // can't even be responsible and delete our orphaned album
                return resp;
            }

            // in case I need them later 
            var responses = new List<APIResponses.ImageResponse>();
            for (var i = 0; i < images.Count(); ++i)
            {
                var image = images[i];
                var title = string.Empty;
                if (i < titles.Count())
                    title = titles[i];

                var description = string.Empty;
                if (i < descriptions.Count())
                    description = descriptions[i];

                responses.Add(InternalUploadImage(image, false, title, description, anonymous, anonymous ? resp.ResponseData.DeleteHash : resp.ResponseData.Id));
            }

            // since an album creation doesn't return very much in the manner of information, make a request to 
            // get the fully populated album
            var deletehash = resp.ResponseData.DeleteHash; // save deletehash
            responseString = "";
            using (var t = WebClientFactory.Create())
            {
                t.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(anonymous);
                try
                {
                    responseString = t.DownloadString(url + "/" + resp.ResponseData.Id);
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    else
                    {
                        var stream = ex.Response.GetResponseStream();
                        var currByte = -1;
                        var strBuilder = new StringBuilder();
                        while ((currByte = stream.ReadByte()) != -1)
                        {
                            strBuilder.Append((char)currByte);
                        }
                        responseString = strBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            var oldResp = resp;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.AlbumResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
            }

            if (resp == null || responseString == "" || responseString == null)
                resp = new APIResponses.AlbumResponse() { Success = false };

            resp.ResponseData.DeleteHash = deletehash;

            if (resp.Success)
            {
                var i = 0;
                foreach (var response in resp.ResponseData.Images)
                    if (response.Id == resp.ResponseData.Cover)
                        break;
                    else
                        i++;
                if (i < images.Length)
                    resp.CoverImage = images[i];
                else
                    resp.CoverImage = null;
            }

            if (resp.Success)
                Log.Info("Successfully created album! (" + resp.Status.ToString() + ")");
            else
            {
                Log.Error("Created album, but failed to get album information! (" + resp.Status.ToString() + ")");
                return oldResp;
            }

            return resp;
        }

        static public bool DeleteAlbum(string deleteHash, bool anonymousAlbum)
        {
            var url = mEndPoint + "album/" + deleteHash;

            if (!anonymousAlbum && !HasBeenAuthorized())
            {
                Log.Error("Can't delete an album that belongs to an account while the app is no longer authorized!");
                return false;
            }

            var responseString = string.Empty;
            using (var wc = WebClientFactory.Create())
            {
                wc.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(false);
                try
                {
                    responseString = wc.UploadString(url, "DELETE", string.Empty);
                }
                catch (WebException ex)
                {
                    if (ex.Status != WebExceptionStatus.Success)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    Log.Error("An exception was thrown while trying to delete an image from Imgur (" + ex.Status + ") [deletehash: " + deleteHash + "]");
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            APIResponses.BaseResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.BaseResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                resp = null;
            }

            if (resp == null || responseString == null || responseString == string.Empty)
            {
                resp = new APIResponses.ImageResponse();
                resp.Success = false;
            }

            if (resp.Success)
            {
                Log.Info("Successfully deleted album! (" + resp.Status.ToString() + ")");
                return true;
            }

            Log.Error("Failed to delete album! (" + resp.Status.ToString() + ") [\n\rdeletehash: " + deleteHash + "\n\r]");
            return false;
        }

        static public bool DeleteImage(string deleteHash, bool anonymousImage)
        {
            var url = mEndPoint + "image/" + deleteHash;

            if (!anonymousImage && !HasBeenAuthorized())
            {
                Log.Error("Can't delete an image that belongs to an account while the app is no longer authorized!");
                return false;
            }

            var responseString = string.Empty;
            using (var wc = WebClientFactory.Create())
            {
                wc.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(false);
                try
                {
                    responseString = wc.UploadString(url, "DELETE", string.Empty);
                }
                catch (WebException ex)
                {
                    if (ex.Status != WebExceptionStatus.Success)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    Log.Error("An exception was thrown while trying to delete an image from Imgur (" + ex.Status + ") [deletehash: " + deleteHash + "]");
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            APIResponses.BaseResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.BaseResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                resp = null;
            }

            if (resp == null || responseString == null || responseString == string.Empty)
            {
                resp = new APIResponses.ImageResponse();
                resp.Success = false;
            }

            if (resp.Success)
            {
                Log.Info("Successfully deleted image! (" + resp.Status.ToString() + ")");
                return true;
            }

            Log.Error("Failed to delete image! (" + resp.Status.ToString() + ") [\n\rdeletehash: " + deleteHash + "\n\r]");
            return false;
        }

        static public void OpenAuthorizationPage()
        {
            var url = "https://api.imgur.com/oauth2/authorize?client_id=" + mClientId + "&response_type=pin&state=";

            System.Diagnostics.Process.Start(url);
        }

        static public void RequestTokens(string pin)
        {
            var url = "https://api.imgur.com/oauth2/token";

            var responseString = string.Empty;
            using (var wc = WebClientFactory.Create())
            {
                //t.Headers[HttpRequestHeader.Authorization] = "Client-ID " + m_ClientID;
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "client_id", mClientId
                        },
                        {
                            "client_secret", mClientSecret
                        },
                        {
                            "grant_type", "pin"
                        },
                        {
                            "pin", pin
                        }
                    };
                    var response = wc.UploadValues(url, "POST", values);
                    responseString = Encoding.ASCII.GetString(response);
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    else
                    {
                        var stream = ex.Response.GetResponseStream();
                        var currByte = -1;
                        var strBuilder = new StringBuilder();
                        while ((currByte = stream.ReadByte()) != -1)
                        {
                            strBuilder.Append((char)currByte);
                        }
                        responseString = strBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            if (responseString == string.Empty)
            {
                return;
            }

            var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.TokenResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            if (resp != null && resp.AccessToken != null && resp.RefreshToken != null)
            {
                StoreNewTokens(resp.ExpiresIn, resp.AccessToken, resp.RefreshToken);

                Log.Info("Received tokens from PIN");

                StartTokenThread();

                if (ObtainedAuthorization != null) ObtainedAuthorization.Invoke();
            }
            else
            {
                Log.Error("Something went wrong while trying to obtain access and refresh tokens");
            }
        }

        static public void ForceRefreshTokens()
        {
            Log.Info("Forcing token refresh...");
            if (mTokenThread != null) mTokenThread.Abort();
            RefreshTokensAndStartTokenThread();
        }

        static private void RefreshTokensAndStartTokenThread()
        {
            if (RefreshTokens())
            {
                StartTokenThread();
            }
        }

        static private bool RefreshTokens()
        {
            if (!HasBeenAuthorized())
            {
                return false;
            }

            var url = "https://api.imgur.com/oauth2/token";

            var responseString = string.Empty;
            using (var wc = WebClientFactory.Create())
            {
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "client_id", mClientId
                        },
                        {
                            "client_secret", mClientSecret
                        },
                        {
                            "grant_type", "refresh_token"
                        },
                        {
                            "refresh_token", mCurrentRefreshToken
                        }
                    };
                    var response = wc.UploadValues(url, "POST", values);
                    responseString = Encoding.ASCII.GetString(response);
                }
                catch (WebException ex)
                {
                    if (ex.Response == null)
                    {
                        if (NetworkRequestFailed != null) NetworkRequestFailed.Invoke();
                    }
                    else
                    {
                        var stream = ex.Response.GetResponseStream();
                        var currByte = -1;
                        var strBuilder = new StringBuilder();
                        while ((currByte = stream.ReadByte()) != -1)
                        {
                            strBuilder.Append((char)currByte);
                        }
                        responseString = strBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected Exception: " + ex.ToString());
                }
            }

            if (responseString == string.Empty)
            {
                return false;
            }

            APIResponses.TokenResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.TokenResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                Log.Error("Newtonsoft.Json.JsonReaderException occurred while trying to deserialize the following string:\n" + responseString + " (Line: " + ex.LineNumber + ", Position: " + ex.LinePosition + ", Message: " + ex.Message + ")");
                resp = null;
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected Exception: " + ex.ToString());
            }
            if (resp != null && resp.AccessToken != null && resp.RefreshToken != null)
            {
                StoreNewTokens(resp.ExpiresIn, resp.AccessToken, resp.RefreshToken);

                Log.Info("Refreshed tokens");

                if (RefreshedAuthorization != null) RefreshedAuthorization.Invoke();

                return true;
            }

            Log.Error("Something went wrong while trying to refresh access- and refresh-tokens");

            mCurrentAccessToken = null;
            mCurrentRefreshToken = null;

            Properties.Settings.Default.accessToken = null;
            Properties.Settings.Default.refreshToken = null;
            Properties.Settings.Default.Save();

            if (LostAuthorization != null) LostAuthorization.Invoke();

            return false;
        }

        static private void StoreNewTokens(int expiresInSeconds, string accessToken, string refreshToken)
        {
            mTokensExpireAt = DateTime.Now.AddSeconds(expiresInSeconds / 2);

            mCurrentAccessToken = accessToken;
            mCurrentRefreshToken = refreshToken;

            Properties.Settings.Default.accessToken = mCurrentAccessToken;
            Properties.Settings.Default.refreshToken = mCurrentRefreshToken;
            Properties.Settings.Default.Save();
        }

        static private void StartTokenThread()
        {
            mTokenThread = new System.Threading.Thread(TokenThread);
            mTokenThread.Start();
        }

        static private void TokenThread()
        {
            Log.Info("Token thread started");
            while (true)
            {
                var timeSpan = (mTokensExpireAt > DateTime.Now) ? (mTokensExpireAt - DateTime.Now) : (DateTime.Now.AddSeconds(60.0) - DateTime.Now);
                Log.Info("Token thread will refresh in " + timeSpan.TotalSeconds + " seconds");
                System.Threading.Thread.Sleep(timeSpan);
                if (!RefreshTokens())
                {
                    Log.Error("Could not refresh tokens on token thread, thread aborting");
                    break;
                }
            }
        }

        // This function attempts to read any old refresh and access tokens from the settings file
        // and then tries to use these to obtain new ones. This method should be called at the start of the application
        // in order to be able to persistently keep the app authorized after the user doing so once.
        static public void AttemptRefreshTokensFromDisk()
        {
            var accessToken = Properties.Settings.Default.accessToken;
            var refreshToken = Properties.Settings.Default.refreshToken;

            if (accessToken != null &&
                accessToken != string.Empty &&
                refreshToken != null &&
                refreshToken != string.Empty)
            {
                Log.Info("Detected old tokens on disk, attempting to exchange tokens for fresh ones...");

                // Super hacky way of getting old tokens to be used. But it works!
                mTokensExpireAt = DateTime.Now.AddSeconds(10.0);

                mCurrentAccessToken = accessToken;
                mCurrentRefreshToken = refreshToken;
                //m_TokensExpireAt = DateTime.Now.AddHours(1337.0);   // Just so the tokens appear to expire way in the future when we call RefreshTokens.
                RefreshTokensAndStartTokenThread();
            }
        }

        static private string GetAuthorizationHeader(bool anonymous)
        {
            if (!anonymous && HasBeenAuthorized())
            {
                return "Bearer " + mCurrentAccessToken;
            }
            return "Client-ID " + mClientId;
        }

        static public bool HasBeenAuthorized()
        {
            return (mCurrentAccessToken != null && mCurrentAccessToken != string.Empty && mCurrentRefreshToken != null && mCurrentRefreshToken != string.Empty && mTokensExpireAt > DateTime.MinValue/*&& m_TokensExpireAt > DateTime.Now*/);
        }

        static public void OnMainThreadExit()
        {
            if (mTokenThread != null)
            {
                Log.Info("Waiting for token thread to abort due to main thread exiting...");
                mTokenThread.Abort();
                mTokenThread.Join();
            }
        }

        static public void ForgetTokens()
        {
            mTokenThread.Abort();
            mCurrentAccessToken = string.Empty;
            mCurrentRefreshToken = string.Empty;
            Properties.Settings.Default.accessToken = string.Empty;
            Properties.Settings.Default.refreshToken = string.Empty;
            Properties.Settings.Default.Save();
            if (LostAuthorization != null) LostAuthorization.Invoke();
        }

        public static WebClientFactory WebClientFactory { get; } = new WebClientFactory();
    }
}
