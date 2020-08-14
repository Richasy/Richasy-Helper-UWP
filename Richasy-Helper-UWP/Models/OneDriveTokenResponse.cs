﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Helper.UWP.Models
{
    public class OneDriveTokenResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
