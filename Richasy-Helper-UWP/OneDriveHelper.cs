using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.QueryStringDotNET;
using Newtonsoft.Json;
using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _clientId = "";
        private string[] _scopes;
        private const string _baseUrl = "https://graph.microsoft.com/v1.0";
        private IPublicClientApplication _clientApp;
        private GraphServiceClient _graphClient;
        private HttpClient _httpClient;

        private const string API_AppRoot = _baseUrl + "/me/drive/special/approot";
        private const string API_Items = _baseUrl + "/me/drive/items";

        public OneDriveHelper(string clientId, string[] scopes)
        {
            _clientId = clientId;
            _scopes = scopes;
            _clientApp = PublicClientApplicationBuilder.Create(_clientId)
                .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
             .Build();
            DeviceCodeProvider authProvider = new DeviceCodeProvider(_clientApp, _scopes);
            _graphClient = new GraphServiceClient(authProvider);
            _httpClient = new HttpClient();
        }

        public OneDriveHelper(string clientId, string[] scopes, string token) : this(clientId, scopes)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// 预热链接
        /// </summary>
        /// <returns></returns>
        public async Task WarmUpAsync()
        {
            try
            {
                await _httpClient.SendAsync(new HttpRequestMessage
                {
                    Method = new HttpMethod("HEAD"),
                    RequestUri = new Uri(_baseUrl)
                });
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationResult> AuthorizationAsync()
        {
            var accounts = await _clientApp.GetAccountsAsync();
            var response = await _clientApp.AcquireTokenInteractive(_scopes)
                    .WithAccount(accounts.FirstOrDefault())
                    .WithPrompt(Microsoft.Identity.Client.Prompt.SelectAccount)
                    .ExecuteAsync();
            if (response != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
            return response;
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationResult> RefreshTokenAsync()
        {
            var accounts = await _clientApp.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();
            AuthenticationResult authResult = null;
            try
            {
                authResult = await _clientApp.AcquireTokenSilent(_scopes, firstAccount)
                .ExecuteAsync();
                if (authResult != null)
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                return authResult;
            }
            catch (MsalUiRequiredException)
            {
                return await AuthorizationAsync();
            }
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<bool> IsFileExistAsync(string path)
        {
            try
            {
                var item = await _graphClient.Me.Drive.Special.AppRoot.ItemWithPath(path).Request().GetAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径（应用文件夹下）</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public async Task CreateFileAsync(string path, string content)
        {
            await PutRequestAsync<DriveItem>(API_AppRoot + $":/{path}:/content", content, "text/plain");
        }

        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public async Task UpdateFileAsync(string id, string content)
        {
            await PutRequestAsync<DriveItem>(API_Items + $"/{id}/content", content);
        }

        /// <summary>
        /// 根据路径获取文件ID
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public async Task<string> GetFileIdFromPathAsync(string path)
        {
            var item = await _graphClient.Me.Drive.Special.AppRoot.ItemWithPath(path).Request().GetAsync();
            return item.Id;
        }

        /// <summary>
        /// 根据路径获取文件内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public async Task<string> GetFileContentAsync(string path)
        {
            string result = null;
            var stream = await _graphClient.Me.Drive.Special.AppRoot.ItemWithPath(path).Content.Request().GetAsync();
            if (stream != null)
            {
                using (stream)
                {
                    var reader = new StreamReader(stream);
                    result = await reader.ReadToEndAsync();
                }
            }
            return result;
        }

        /// <summary>
        /// 获取个人资料
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetMeAsync()
        {
            var profile = await _graphClient.Me.Request().GetAsync();
            return profile;
        }

        private async Task<T> PutRequestAsync<T>(string path, string content, string format = "application/json") where T : class
        {
            var response = await _httpClient.PutAsync(path, new StringContent(content, Encoding.UTF8, format));
            if (response.IsSuccessStatusCode)
            {
                string re = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(re);
            }
            return null;
        }
    }
}
