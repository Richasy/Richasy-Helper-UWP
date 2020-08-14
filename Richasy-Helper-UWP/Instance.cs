using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Richasy.Helper.UWP
{
    public class Instance
    {
        private Options _options;
        public AppHelper App;
        public IOHelper IO;
        public NotificationHelper Notification;
        public WebHelper Web;
        public NetHelper Net;

        private List<Tuple<Guid, Action<Size>>> WindowSizeChangedNotify { get; set; } = new List<Tuple<Guid, Action<Size>>>();

        public Instance()
        {
            _options = new Options();
            App = new AppHelper(_options);
            IO = new IOHelper();
            Notification = new NotificationHelper(_options);
            Web = new WebHelper();
            Net = new NetHelper();

            try
            {
                Window.Current.SizeChanged += WindowSizeChangedHandle;
            }
            catch (Exception){}
        }

        public Instance(Options options) : this()
        {
            _options = options;
            App = new AppHelper(options);
            Notification = new NotificationHelper(options);
        }

        public Instance(string settingContainerName) : this()
        {
            _options = new Options(settingContainerName);
            App = new AppHelper(_options);
            Notification = new NotificationHelper(_options);
        }

        public void WindowSizeChangedHandle(object sender, WindowSizeChangedEventArgs e)
        {
            if (WindowSizeChangedNotify.Count > 0)
            {
                WindowSizeChangedNotify.ForEach(p => p.Item2?.Invoke(e.Size));
            }
        }

        public void AddWindowSizeChangeAction(Guid guid, Action<Size> changeAction)
        {
            WindowSizeChangedNotify.Add(new Tuple<Guid, Action<Size>>(guid, changeAction));
        }
        public void RemoveWindowSizeChangeAction(Guid guid)
        {
            WindowSizeChangedNotify.RemoveAll(p => p.Item1 == guid);
        }
    }
}
