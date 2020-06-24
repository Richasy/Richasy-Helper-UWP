using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP.Models
{
    public class JsonConvertException:Exception
    {
        public JsonConvertException():base()
        {

        }
        public JsonConvertException(string message):base(message)
        {

        }
    }
}
