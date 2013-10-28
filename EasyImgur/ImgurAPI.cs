using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.Linq;

namespace EasyImgur
{
    public class ImgurAPI
    {
        static private string m_EndPoint = "https://api.imgur.com/3/";

        static private string m_ClientID = "5fae4323a27c0cf";
        static private string m_ClientSecret = "3e9200a0bf59d5b23de53287ec47898997ee4b98";

        static private int m_NumUploads = 0;

        static private string m_CurrentAccessToken = string.Empty;
        static private string m_CurrentRefreshToken = string.Empty;
        static private DateTime m_TokensExpireAt;

        static private System.Threading.Thread m_TokenThread = null;

        static public event AuthorizationEventHandler obtainedAuthorization;
        static public event AuthorizationEventHandler lostAuthorization;
        static public event AuthorizationEventHandler refreshedAuthorization;
        public delegate void AuthorizationEventHandler();

        static public int numSuccessfulUploads
        {
            get
            {
                return m_NumUploads;
            }
        }

        static private APIResponses.ImageResponse InternalUploadImage( object _Obj, bool _URL, string _Title, string _Description, bool _Anonymous )
        {
            if (_Obj == null)
            {
                throw new System.ArgumentNullException();
            }

            string url = m_EndPoint + "image";

            string responseString = string.Empty;
            byte[] response = null;

            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            if (!_URL)
            {
                Image _Image = _Obj as Image;
                System.Drawing.Imaging.ImageFormat format = _Image.RawFormat;
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
                            if (format == System.Drawing.Imaging.ImageFormat.Bmp ||
                                format == System.Drawing.Imaging.ImageFormat.Gif ||
                                format == System.Drawing.Imaging.ImageFormat.Jpeg ||
                                format == System.Drawing.Imaging.ImageFormat.Icon ||
                                format == System.Drawing.Imaging.ImageFormat.Png ||
                                format == System.Drawing.Imaging.ImageFormat.Tiff ||
                                format == System.Drawing.Imaging.ImageFormat.Emf ||
                                format == System.Drawing.Imaging.ImageFormat.Wmf)
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

                _Image.Save(memStream, format);
            }

            using (WebClient t = new WebClient())
            {
                t.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(_Anonymous);
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "image", _URL ? _Obj as string : Convert.ToBase64String(memStream.ToArray())
                        },
                        {
                            "title", _Title
                        },
                        {
                            "description", _Description
                        },
                        {
                            "type", _URL ? "URL" : "base64"
                        }
                    };
                    response = t.UploadValues(url, "POST", values);
                    responseString = System.Text.Encoding.ASCII.GetString(response);
                }
                catch (System.Net.WebException ex)
                {
                    System.IO.Stream stream = ex.Response.GetResponseStream();
                    int currByte = -1;
                    StringBuilder strBuilder = new StringBuilder();
                    while ((currByte = stream.ReadByte()) != -1)
                    {
                        strBuilder.Append((char)currByte);
                    }
                    responseString = strBuilder.ToString();
                }
            }

            APIResponses.ImageResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.ImageResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (System.Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                resp = new APIResponses.ImageResponse();
                resp.success = false;
            }

            if (resp.success)
            {
                Log.Info("Successfully uploaded image! (" + resp.status.ToString() + ")[\n\rid: " + resp.data.id + "\n\rlink: " + resp.data.link + "\n\rdeletehash: " + resp.data.deletehash + "\n\r]");
                ++m_NumUploads;
            }
            else
            {
                Log.Error("Failed to upload image (" + resp.status.ToString() + ")");
            }

            return resp;
        }

        static public APIResponses.ImageResponse UploadImage( Image _Image, string _Title, string _Description, bool _Anonymous )
        {
            return InternalUploadImage(_Image, false, _Title, _Description, _Anonymous);
        }

        static public APIResponses.ImageResponse UploadImage( string _URL, string _Title, string _Description, bool _Anonymous )
        {
            return InternalUploadImage(_URL, true, _Title, _Description, _Anonymous);
        }

        static public bool DeleteImage( string _DeleteHash, bool _AnonymousImage )
        {
            string url = m_EndPoint + "image/" + _DeleteHash;

            if (!_AnonymousImage && !HasBeenAuthorized())
            {
                Log.Error("Can't delete an image that belongs to an account while the app is no longer authorized!");
                return false;
            }

            string responseString = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = GetAuthorizationHeader(false);
                try
                {
                    responseString = wc.UploadString(url, "DELETE", string.Empty);
                }
                catch (System.Net.WebException ex)
                {
                    Log.Error("An exception was thrown while trying to delete an image from Imgur (" + ex.Status + ") [deletehash: " + _DeleteHash + "]");
                }
            }

            APIResponses.BaseResponse resp = null;
            try
            {
                resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.BaseResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            }
            catch (System.Exception ex)
            {
                Log.Error("Newtonsoft.Json.JsonConvert.DeserializeObject threw an exception!: " + ex.Message + "Stack trace:\n\r" + ex.StackTrace);
                resp = new APIResponses.ImageResponse();
                resp.success = false;
            }

            if (resp.success)
            {
                Log.Info("Successfully deleted image! (" + resp.status.ToString() + ")");
                return true;
            }

            Log.Error("Failed to delete image! (" + resp.status.ToString() + ") [\n\rdeletehash: " + _DeleteHash + "\n\r]");
            return false;
        }

        static public void OpenAuthorizationPage()
        {
            string url = "https://api.imgur.com/oauth2/authorize?client_id=" + m_ClientID + "&response_type=pin&state=";

            System.Diagnostics.Process.Start(url);
        }

        static public void RequestTokens( string _PIN )
        {
            string url = "https://api.imgur.com/oauth2/token";

            string responseString = string.Empty;
            using (WebClient wc = new WebClient())
            {
                //t.Headers[HttpRequestHeader.Authorization] = "Client-ID " + m_ClientID;
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "client_id", m_ClientID
                        },
                        {
                            "client_secret", m_ClientSecret
                        },
                        {
                            "grant_type", "pin"
                        },
                        {
                            "pin", _PIN
                        }
                    };
                    byte[] response = wc.UploadValues(url, "POST", values);
                    responseString = System.Text.Encoding.ASCII.GetString(response);
                }
                catch (System.Net.WebException ex)
                {
                    System.IO.Stream stream = ex.Response.GetResponseStream();
                    int currByte = -1;
                    StringBuilder strBuilder = new StringBuilder();
                    while ((currByte = stream.ReadByte()) != -1)
                    {
                        strBuilder.Append((char)currByte);
                    }
                    responseString = strBuilder.ToString();
                }
            }

            if (responseString == string.Empty)
            {
                return;
            }

            APIResponses.TokenResponse resp = Newtonsoft.Json.JsonConvert.DeserializeObject<APIResponses.TokenResponse>(responseString, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects });
            if (resp != null && resp.access_token != null && resp.refresh_token != null)
            {
                StoreNewTokens(resp.expires_in, resp.access_token, resp.refresh_token);

                Log.Info("Received tokens from PIN");

                StartTokenThread();

                if (obtainedAuthorization != null) obtainedAuthorization.Invoke();
            }
            else
            {
                Log.Error("Something went wrong while trying to obtain access and refresh tokens");
            }
        }

        static public void ForceRefreshTokens()
        {
            Log.Info("Forcing token refresh...");
            m_TokenThread.Abort();
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

            string url = "https://api.imgur.com/oauth2/token";

            string responseString = string.Empty;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        {
                            "client_id", m_ClientID
                        },
                        {
                            "client_secret", m_ClientSecret
                        },
                        {
                            "grant_type", "refresh_token"
                        },
                        {
                            "refresh_token", m_CurrentRefreshToken
                        }
                    };
                    byte[] response = wc.UploadValues(url, "POST", values);
                    responseString = System.Text.Encoding.ASCII.GetString(response);
                }
                catch (System.Net.WebException ex)
                {
                    System.IO.Stream stream = ex.Response.GetResponseStream();
                    int currByte = -1;
                    StringBuilder strBuilder = new StringBuilder();
                    while ((currByte = stream.ReadByte()) != -1)
                    {
                        strBuilder.Append((char)currByte);
                    }
                    responseString = strBuilder.ToString();
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
            if (resp != null && resp.access_token != null && resp.refresh_token != null)
            {
                StoreNewTokens(resp.expires_in, resp.access_token, resp.refresh_token);

                Log.Info("Refreshed tokens");

                if (refreshedAuthorization != null) refreshedAuthorization.Invoke();

                return true;
            }
            
            Log.Error("Something went wrong while trying to refresh access- and refresh-tokens");

            m_CurrentAccessToken = null;
            m_CurrentRefreshToken = null;

            Properties.Settings.Default.accessToken = null;
            Properties.Settings.Default.refreshToken = null;
            Properties.Settings.Default.Save();

            if (lostAuthorization != null) lostAuthorization.Invoke();

            return false;
        }

        static private void StoreNewTokens( int _ExpiresInSeconds, string _AccessToken, string _RefreshToken )
        {
            m_TokensExpireAt = System.DateTime.Now.AddSeconds(_ExpiresInSeconds / 2);

            m_CurrentAccessToken = _AccessToken;
            m_CurrentRefreshToken = _RefreshToken;

            Properties.Settings.Default.accessToken = m_CurrentAccessToken;
            Properties.Settings.Default.refreshToken = m_CurrentRefreshToken;
            Properties.Settings.Default.Save();
        }

        static private void StartTokenThread()
        {
            m_TokenThread = new System.Threading.Thread(TokenThread);
            m_TokenThread.Start();
        }

        static private void TokenThread()
        {
            Log.Info("Token thread started");
            while (true)
            {
                TimeSpan timeSpan = (m_TokensExpireAt - DateTime.Now);
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
            string accessToken = Properties.Settings.Default.accessToken;
            string refreshToken = Properties.Settings.Default.refreshToken;

            if (accessToken != null && 
                accessToken != string.Empty && 
                refreshToken != null && 
                refreshToken != string.Empty)
            {
                Log.Info("Detected old tokens on disk, attempting to exchange tokens for fresh ones...");

                m_CurrentAccessToken = accessToken;
                m_CurrentRefreshToken = refreshToken;
                //m_TokensExpireAt = DateTime.Now.AddHours(1337.0);   // Just so the tokens appear to expire way in the future when we call RefreshTokens.
                RefreshTokensAndStartTokenThread();
            }
        }

        static private string GetAuthorizationHeader( bool _Anonymous )
        {
            if (!_Anonymous && HasBeenAuthorized())
            {
                return "Bearer " + m_CurrentAccessToken;
            }
            return "Client-ID " + m_ClientID;
        }

        static public bool HasBeenAuthorized()
        {
            return (m_CurrentAccessToken != null && m_CurrentAccessToken != string.Empty && m_CurrentRefreshToken != null && m_CurrentRefreshToken != string.Empty && m_TokensExpireAt != null /*&& m_TokensExpireAt > DateTime.Now*/);
        }

        static public void OnMainThreadExit()
        {
            if (m_TokenThread != null)
            {
                Log.Info("Waiting for token thread to abort due to main thread exiting...");
                m_TokenThread.Abort();
                m_TokenThread.Join();
            }
        }

        static public void ForgetTokens()
        {
            m_TokenThread.Abort();
            m_CurrentAccessToken = string.Empty;
            m_CurrentRefreshToken = string.Empty;
            Properties.Settings.Default.accessToken = string.Empty;
            Properties.Settings.Default.refreshToken = string.Empty;
            Properties.Settings.Default.Save();
            if (lostAuthorization != null) lostAuthorization.Invoke();
        }
    }
}
