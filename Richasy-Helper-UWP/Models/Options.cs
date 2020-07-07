using System;
using System.Collections.Generic;
using Windows.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP.Models
{
    public class Options
    {
        public string SettingContainerName { get; set; }
        public string DefaultNotificationLogoPath { get; set; }
        public Color DarkButtonHoverColor { get; set; }
        public Color DarkButtonPressColor { get; set; }
        public Color LightButtonHoverColor { get; set; }
        public Color LightButtonPressColor { get; set; }
        public string LocalizationStringPrefix { get; set; }
        public Options()
        {
            DarkButtonHoverColor = Color.FromArgb(255, 33, 42, 67);
            DarkButtonPressColor = Color.FromArgb(255, 255, 86, 86);
            LightButtonHoverColor = Color.FromArgb(255, 63, 63, 63);
            LightButtonPressColor = Color.FromArgb(255, 63, 63, 63);
            LocalizationStringPrefix = "Tip_";
        }
        public Options(string settingContainerName, string defaultLogoPath = null):this()
        {
            SettingContainerName = settingContainerName;
            DefaultNotificationLogoPath = defaultLogoPath;
        }
    }
}
