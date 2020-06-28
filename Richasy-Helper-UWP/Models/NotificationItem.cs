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
        public string Args { get; set; }
        public string Logo { get; set; }
        public string Tag { get; set; }
        public string AttributeText { get; set; }
        public ToastActivationType ActiveType { get; set; }
        public NotificationItem()
        {

        }
        public NotificationItem(string t, string d, string img, string arg, string l, string tag = "", string a = "", ToastActivationType actType = ToastActivationType.Background)
        {
            Title = t;
            Description = d;
            HeroImage = img;
            Args = arg;
            Logo = l;
            Tag = tag;
            AttributeText = a;
            ActiveType = actType;
        }
    }
}
