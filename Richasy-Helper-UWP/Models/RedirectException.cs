using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP.Models
{
    public class RedirectException:Exception
    {
        public RedirectException() : base()
        {

        }
        public RedirectException(string message):base(message)
        {

        }
    }
}
