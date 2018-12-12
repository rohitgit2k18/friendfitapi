using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendFit.API.Models
{
    public class EmailModel
    {
        public string ToEmail { get; set; }
        public string messagebody { get; set; }
        public string Subject { get; set; }
    }
}