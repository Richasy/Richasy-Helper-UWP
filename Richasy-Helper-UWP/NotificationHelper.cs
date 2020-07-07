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
        public ToastContent GetToastContent(NotificationItem item)
        {
            var content = new ToastContent
            {
                Launch = item.Args,
                ActivationType = item.ActiveType,
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
                            Text = item.AttributeText
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
            return content;
        }
        public void ShowToast(NotificationItem item, string tag = null, string groupName = "")
        {
            var content = GetToastContent(item);
            ShowToast(content, tag, groupName);
        }
        public void ShowToast(List<NotificationItem> items, string overflowText = "", int maxNum = 2, string groupName = "")
        {
            int index = 0;
            foreach (var item in items)
            {
                if (index >= maxNum)
                {
                    if (!string.IsNullOrEmpty(overflowText))
                    {
                        var overflow = GetOverflowToast(overflowText);
                        ShowToast(overflow, "", groupName);
                    }
                    break;
                }
                var content = GetToastContent(item);
                index++;
                ShowToast(content, item.Tag, groupName);
            }
        }
        public void ShowToast(List<ToastContent> items, string overflowText = "", int maxNum = 2, string groupName = "")
        {
            int index = 0;
            foreach (var item in items)
            {
                if (index >= maxNum)
                {
                    if (!string.IsNullOrEmpty(overflowText))
                    {
                        var overflow = GetOverflowToast(overflowText);
                        ShowToast(overflow, "", groupName);
                    }
                    break;
                }
                index++;
                ShowToast(item, "", groupName);
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
        public void ShowToast(ToastContent content, string tag = null, string groupName = "")
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var notification = new ToastNotification(content.GetXml());
            if (!string.IsNullOrEmpty(groupName))
                notification.Group = groupName;
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
