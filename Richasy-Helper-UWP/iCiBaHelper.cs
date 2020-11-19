using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Helper.UWP.Models;

namespace Richasy.Helper.UWP
{
    public class iCiBaHelper
    {
        private Instance instance;
        private const string iCiBaKey = "FD6631A6A5BC97C124D85CDCAFCA7806";
        public iCiBaHelper(Instance ins)
        {
            instance = ins;
        }
        /// <summary>
        /// 从网络获取单词释义
        /// </summary>
        /// <param name="text">单词</param>
        /// <param name="targetLan">目标语言</param>
        /// <returns></returns>
        public async Task<Ciba> GetWebICiBaModel(string text,string targetLan)
        {
            string url = $"http://dict-co.iciba.com/api/dictionary.php?w={text.ToLower()}&type=json&key={iCiBaKey}&t={targetLan}&f=auto";
            Ciba data = null;
            try
            {
                var response = await instance.Net.GetTextFromWebAsync(url);
                if (response.Contains("\"word_mean\":"))
                {
                    response = response.Replace("\"means\":", "\"means_other\":");
                }
                data = JsonConvert.DeserializeObject<Ciba>(response);
            }
            catch (Exception) { }
            return data;
        }
        /// <summary>
        /// 获取词霸的翻译
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="targetLan">目标语言</param>
        /// <returns></returns>
        public async Task<Translate> GetICiBaTranslate(string text,string targetLan)
        {
            string url = $"http://fy.iciba.com/ajax.php?a=fy";
            var client = new HttpClient();
            var kvs = new Dictionary<string, string>();
            kvs.Add("f", "auto");
            kvs.Add("t", targetLan);
            kvs.Add("w", text);
            var pos = new FormUrlEncodedContent(kvs);
            Translate result = null;
            using (client)
            {
                try
                {
                    var data = await client.PostAsync(url, pos);
                    if (data.IsSuccessStatusCode)
                    {
                        string content = await data.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Translate>(content);
                    }
                }
                catch (Exception) { }
            }
            return result;
        }
    }
}
