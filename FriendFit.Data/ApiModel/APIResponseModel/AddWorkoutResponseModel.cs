using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class AddWorkoutResponseModel
    {
        public Int64 WorkoutId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
