using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;

namespace Richasy.Helper.UWP
{
    public class WebHelper
    {
        /// <summary>
        /// 从网络获取文本
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public async Task<string> GetTextFromWebAsync(string url,Dictionary<string,string> headers=null)
        {
            var client = new HttpClient();
            using (client)
            {
                try
                {
                    if (headers != null)
                    {
                        foreach (var kv in headers)
                        {
                            client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                        }
                    }
                    var uri = new Uri(url);
                    var response = await client.GetAsync(new Uri(url));
                    if (response.IsSuccessStatusCode)
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        return res;
                    }
                    else if (response.StatusCode == HttpStatusCode.TemporaryRedirect || response.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        string tempUrl = response.Headers.Location.AbsoluteUri;
                        throw new RedirectException(tempUrl);
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        /// <summary>
        /// 获取网络数据并转化为对应的类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public async Task<T> GetEntityFromWebAsync<T>(string url, Dictionary<string, string> headers = null) where T: class
        {
            string response = await GetTextFromWebAsync(url, headers);
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(response);
                }
                catch (Exception ex)
                {
                    throw new JsonConvertException(ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// 向网络上传字符串
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">数据</param>
        /// <param name="format">数据上传格式</param>
        /// <param name="headers">自定义请求头</param>
        /// <returns></returns>
        public async Task<string> PostContentToWebAsync(string url, string content, Dictionary<string, string> headers = null, string format= "application/json")
        {
            var client = new HttpClient();
            using (client)
            {
                if (headers != null)
                {
                    foreach (var kv in headers)
                    {
                        client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                    }
                }
                var response = await client.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, format));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }
        /// <summary>
        /// URL组合
        /// </summary>
        /// <param name="url">基础URL</param>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public string UrlCombine(string url, Dictionary<string, string> param)
        {
            string p = "";
            if (param != null)
            {
                foreach (var item in param)
                {
                    p = $"{item.Key}={item.Value}&";
                }
            }
            p = p.TrimEnd('&');
            if (!string.IsNullOrEmpty(p))
                return url + $"?{p}";
            return url;
        }
    }
}
