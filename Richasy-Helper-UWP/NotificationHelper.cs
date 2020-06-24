using Microsoft.Toolkit.Uwp.Notifications;
using Richasy.Helper.UWP.Models;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace Richasy.Helper.UWP
{
    public class NotificationHelper
    {
        private Options _options;
        internal NotificationHelper(Options options)
        {
            _options = options;
        }
        public void SendToast(List<NotificationItem> items, string overflowText, int maxNum = 2, string groupName = "", string tagName = "", string attribute = "", ToastActivationType activeType = ToastActivationType.Foreground)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (index >= maxNum)
                {
                    var overflow = GetOverflowToast(overflowText);
                    ShowToast(overflow, tagName, groupName);
                    break;
                }
                var content = new ToastContent
                {
                    Launch = item.Args,
                    ActivationType = activeType,
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text=item.Title,
                                    HintMaxLines=2,
                                    HintStyle=AdaptiveTextStyle.Header
                                },
                                new AdaptiveText()
                                {
                                    Text = item.Description,
                                    HintMaxLines=2,
                                    HintStyle=AdaptiveTextStyle.Default
                                },
                            },
                            AppLogoOverride = new ToastGenericAppLogo()
                            {
                                Source = item.Logo,
                                HintCrop = ToastGenericAppLogoCrop.Circle
                            },
                            Attribution = new ToastGenericAttributionText()
                            {
                                Text = attribute
                            }
                        },
                    }
                };
                if (!string.IsNullOrEmpty(item.HeroImage))
                {
                    content.Visual.BindingGeneric.HeroImage = new ToastGenericHeroImage()
                    {
                        Source = item.HeroImage,
                        AlternateText = item.Title
                    };
                }
                index++;
                ShowToast(content, tagName, groupName);
            }
        }
        public ToastContent GetOverflowToast(string title, string appIcon = "")
        {
            if (string.IsNullOrEmpty(appIcon))
                appIcon = _options.DefaultNotificationLogoPath;
            var content = new ToastContent
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text=title,
                                HintMaxLines=2,
                                HintStyle=AdaptiveTextStyle.Header
                            },
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = appIcon
                        }
                    },
                }
            };
            return content;
        }
        public void ShowToast(ToastContent content, string tag = null, string group = "")
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var notification = new ToastNotification(content.GetXml());
            if (!string.IsNullOrEmpty(group))
                notification.Group = group;
            if (!string.IsNullOrEmpty(tag))
                notification.Tag = tag;
            notifier.Show(notification);
        }
        public void HideToast(string tag)
        {
            ToastNotificationManager.History.Remove(tag);
        }
    }
}
