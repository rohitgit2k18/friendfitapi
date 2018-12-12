using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendFit.API.Models
{
    public class twilioModel
    {
        public long countryCode { get; set; }
        public string  mobileNo { get; set; }
        public string messagebody { get; set; }
    }
}