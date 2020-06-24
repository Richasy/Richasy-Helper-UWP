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
        public Enum Settings { get; set; }
        public Enum Colors { get; set; }
        public Enum Languages { get; set; }
        public Color DarkButtonHoverColor { get; set; }
        public Color DarkButtonPressColor { get; set; }
        public Color LightButtonHoverColor { get; set; }
        public Color LightButtonPressColor { get; set; }
    }
}
