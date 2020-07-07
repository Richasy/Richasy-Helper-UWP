using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP.Models
{
    public class NotificationItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string HeroImage { get; set; }
        /// <summary>
        /// 启动参数
        /// </summary>
        public string Args { get; set; }
        public string Logo { get; set; }
        public string Tag { get; set; }
        public string AttributeText { get; set; }
        public ToastActivationType ActiveType { get; set; }
        public NotificationItem()
        {

        }
        public NotificationItem(string title, string description="", string heroImage="", string args="", string logo="", string tag = "", string attribute = "", ToastActivationType actType = ToastActivationType.Background)
        {
            Title = title;
            Description = description;
            HeroImage = heroImage;
            Args = args;
            Logo = logo;
            Tag = tag;
            AttributeText = attribute;
            ActiveType = actType;
        }
    }
}
