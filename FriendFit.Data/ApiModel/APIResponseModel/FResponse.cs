using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Entity.ApiModel.APIResponseModel
{
   public class FResponse
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Int64 WorkoutId { get; set; }

    }

}
