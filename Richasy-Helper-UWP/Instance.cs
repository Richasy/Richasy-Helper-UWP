using Richasy.Helper.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP
{
    public class Instance
    {
        private Options _options;
        public AppHelper App;
        public IOHelper IO;
        public MD5Helper MD5;
        public NotificationHelper Notification;
        public WebHelper Web;


        public Instance(Options options)
        {
            _options = options;
            App = new AppHelper(options);
            IO = new IOHelper();
            MD5 = new MD5Helper();
            Notification = new NotificationHelper(options);
            Web = new WebHelper();
        }
    }
}
