using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendFit.API.PayPal
{
    public class ProductInfo
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public string SKU { get; set; }
        public int OrderQty { get; set; }
    }
}