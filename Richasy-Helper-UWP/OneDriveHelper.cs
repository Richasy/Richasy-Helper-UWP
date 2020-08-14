using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.QueryStringDotNET;
using Newtonsoft.Json;
using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP
{
    public class OneDriveHelper
    {
        private string _baseUrl = "https://graph.microsoft.com/v1.0";
        private string _token = "";
        public HttpClient _httpClient;
        public User User { get; internal set; }

        public OneDriveHelper(string token, HttpClient client = null)
        {
            _token = token;
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                client = new HttpClient(handler);
                client.DefaultRequestHeaders.Connection.Add("keep-alive");
            }
            SetHttpClient(client);
        }
        public void SetHttpClient(HttpClient client)
        {
            _httpClient = client;
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }
        public async Task<OneDriveTokenResponse> AuthorizeAsync(string _clientId, string _clientSecret, string _redirectUrl, string[] _scopes)
        {
            string scope = string.Join(' ', _scopes);
            var startUri = new Uri($"https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id={_clientId}&redirect_uri={WebUtility.UrlEncode(_redirectUrl)}&response_type=code&scope={WebUtility.UrlEncode(scope)}");
            var webAuthenticationResult =
                        await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                        Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                        startUri,
                        new Uri(_redirectUrl));
            OneDriveTokenResponse result = null;
            switch (webAuthenticationResult.ResponseStatus)
            {
                case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                    string data = webAuthenticationResult.ResponseData.ToString();
                    var parseUri = new Uri(data);
                    if (parseUri.Query.Contains("code"))
                    {
                        var query = QueryString.Parse(parseUri.Query.TrimStart('?'));
                        string code = query["code"];
                        var nvc = new Dictionary<string, string>();
                        nvc.Add("code", code);
                        nvc.Add("client_id", _clientId);
                        nvc.Add("client_secret", _clientSecret);
                        nvc.Add("redirect_uri", _redirectUrl);
                        nvc.Add("grant_type", "authorization_code");
                        var content = new FormUrlEncodedContent(nvc);
                        var response = await _httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var tokenResponse = await response.Content.ReadAsStringAsync();
                            var tokenModel = JsonConvert.DeserializeObject<OneDriveTokenResponse>(tokenResponse);
                            _token = tokenModel.access_token;
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.access_token);
                            result = tokenModel;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<OneDriveTokenResponse> RefreshTokenAsync(string _clientId, string _clientSecret, string _redirectUrl, string refreshToken)
        {
            OneDriveTokenResponse result = null;
            try
            {
                var nvc = new Dictionary<string, string>();
                nvc.Add("refresh_token", refreshToken);
                nvc.Add("client_id", _clientId);
                nvc.Add("client_secret", _clientSecret);
                nvc.Add("redirect_uri", _redirectUrl);
                nvc.Add("grant_type", "refresh_token");
                var content = new FormUrlEncodedContent(nvc);
                var response = await _httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", content);
                if (response.IsSuccessStatusCode)
                {
                    var temp = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<OneDriveTokenResponse>(temp);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.access_token);
                    result = data;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}
