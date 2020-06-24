using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Richasy.Helper.UWP
{
    public class AppHelper
    {
        private Options _options;
        internal AppHelper(Options options)
        {
            _options = options;
        }
        /// <summary>
        /// 写入本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="value">设置值</param>
        public void WriteLocalSetting(Enum key, string value)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer(_options.SettingContainerName, ApplicationDataCreateDisposition.Always);
            localcontainer.Values[key.ToString()] = value;
        }
        /// <summary>
        /// 读取本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetLocalSetting(Enum key, string defaultValue)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer(_options.SettingContainerName, ApplicationDataCreateDisposition.Always);
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist)
            {
                return localcontainer.Values[key.ToString()].ToString();
            }
            else
            {
                WriteLocalSetting(key, defaultValue);
                return defaultValue;
            }
        }
        /// <summary>
        /// 获取布尔值的设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public bool GetBoolSetting(Enum key, bool defaultValue = true)
        {
            return Convert.ToBoolean(GetLocalSetting(key, defaultValue.ToString()));
        }
        /// <summary>
        /// 日期转Unix时间戳(秒)
        /// </summary>
        /// <returns></returns>
        public int DateToTimeStamp(DateTime date, int hours = 0)
        {
            var start = new DateTime(1970, 1, 1, hours, 0, 0, 0);
            TimeSpan ts = date - start;
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            return seconds;
        }
        /// <summary>
        /// Unix时间戳(秒)转日期
        /// </summary>
        /// <returns></returns>
        public DateTime TimeStampToDate(int seconds)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(seconds);
            return date.ToLocalTime();
        }
        /// <summary>
        /// Unix时间戳(秒)转日期
        /// </summary>
        /// <returns></returns>
        public DateTime TimeStampToDate(string seconds)
        {
            try
            {
                var date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToInt32(seconds));
                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }

        }
        /// <summary>
        /// 获取当前时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public int GetNowSeconds()
        {
            return DateToTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 获取当前时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public long GetNowTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            long seconds = Convert.ToInt64(ts.TotalMilliseconds);
            return seconds;
        }
        /// <summary>
        /// 初始化标题栏
        /// </summary>
        public void SetTitleBarColor(string theme = "Light")
        {
            var view = ApplicationView.GetForCurrentView();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            if (theme == "Dark")
            {
                // active
                view.TitleBar.BackgroundColor = Colors.Transparent;
                view.TitleBar.ForegroundColor = Colors.White;

                // inactive
                view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.InactiveForegroundColor = Colors.Gray;
                // button
                view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonForegroundColor = Colors.White;

                view.TitleBar.ButtonHoverBackgroundColor = _options.DarkButtonHoverColor;
                view.TitleBar.ButtonHoverForegroundColor = Colors.White;

                view.TitleBar.ButtonPressedBackgroundColor = _options.DarkButtonPressColor;
                view.TitleBar.ButtonPressedForegroundColor = Colors.White;

                view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
            else
            {
                // active
                view.TitleBar.BackgroundColor = Colors.Transparent;
                view.TitleBar.ForegroundColor = Colors.Black;

                // inactive
                view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.InactiveForegroundColor = Colors.Gray;
                // button
                view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonHoverBackgroundColor = _options.LightButtonHoverColor;
                view.TitleBar.ButtonHoverForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonPressedBackgroundColor = _options.LightButtonPressColor;
                view.TitleBar.ButtonPressedForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
        }
        /// <summary>
        /// 获取预先定义的线性画笔资源
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Brush GetThemeBrush(Enum key)
        {
            return (Brush)Application.Current.Resources[key.ToString()];
        }
        /// <summary>
        /// 获取预先定义的字体资源
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public FontFamily GetFontFamily(string key)
        {
            return (FontFamily)Application.Current.Resources[key];
        }
        /// <summary>
        /// 获取预先定义的样式
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Style GetStyle(string key)
        {
            return (Style)Application.Current.Resources[key];
        }
    }
}
