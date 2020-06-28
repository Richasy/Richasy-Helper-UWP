using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Richasy.Helper.UWP
{
    public class IOHelper
    {
        /// <summary>
        /// 打开单个本地文件
        /// </summary>
        /// <param name="types">后缀名列表(如.jpg,.mp3等)</param>
        /// <returns>单个文件</returns>
        public async Task<StorageFile> OpenLocalFileAsync(params string[] types)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            Regex typeReg = new Regex(@"^\.[a-zA-Z0-9]+$");
            foreach (var type in types)
            {
                if (type == "*" || typeReg.IsMatch(type))
                    picker.FileTypeFilter.Add(type);
                else
                    throw new InvalidCastException("文件后缀名不正确");
            }
            var file = await picker.PickSingleFileAsync();
            return file;
        }
        /// <summary>
        /// 打开多个本地文件
        /// </summary>
        /// <param name="types">后缀名列表(如.jpg,.mp3等)</param>
        /// <returns>多个文件</returns>
        public async Task<List<StorageFile>> OpenLocalFilesAsync(params string[] types)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            Regex typeReg = new Regex(@"^\.[a-zA-Z0-9]+$");
            foreach (var type in types)
            {
                if (type == "*" || typeReg.IsMatch(type))
                    picker.FileTypeFilter.Add(type);
                else
                    throw new InvalidCastException("文件后缀名不正确");
            }
            var file = await picker.PickMultipleFilesAsync();
            return file.ToList();
        }
        /// <summary>
        /// 获取保存的文件
        /// </summary>
        /// <param name="type">文件后缀名，如<c>.png</c></param>
        /// <param name="name">文件名</param>
        /// <param name="fileTypeName">文件后缀说明文本</param>
        /// <returns></returns>
        public async Task<StorageFile> GetSaveFileAsync(string type, string name, string fileTypeName)
        {
            var save = new FileSavePicker();
            save.DefaultFileExtension = type;
            save.SuggestedFileName = name;
            save.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            save.FileTypeChoices.Add(fileTypeName, new List<string>() { type });
            var file = await save.PickSaveFileAsync();
            return file;
        }

        /// <summary>
        /// 获取本地存储的数据并进行转化
        /// </summary>
        /// <typeparam name="T">转化类型</typeparam>
        /// <param name="path">文件名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public async Task<T> GetLocalDataAsync<T>(string fileName, string defaultValue = "[]",string folderName="")
        {
            try
            {
                string path = string.IsNullOrEmpty(folderName) ? $"ms-appdata:///local/{fileName}" : $"ms-appdata:///local/{folderName}/{fileName}";
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                string content = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(content))
                    content = defaultValue;
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<T>(defaultValue);
            }
        }

        /// <summary>
        /// 将数据存储到本地
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public async Task SetLocalDataAsync(string fileName, string content, string folderName="")
        {
            StorageFolder folder;
            if (string.IsNullOrEmpty(folderName))
                folder = ApplicationData.Current.LocalFolder;
            else
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, content);
        }
    }
}
