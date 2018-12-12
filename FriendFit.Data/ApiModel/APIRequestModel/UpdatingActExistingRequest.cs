using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class UpdatingActExistingRequest
    {
       public string ExerciseName { get; set; }
        public int? SetsNumber { get; set; }
        public List<ActualWeightModel1> updateWeightList { get; set; }
    }

    public class ActualWeightModel1
    {
        public Int64? TotalWeight { get; set; }
        public Int64? TotalRaps { get; set; }
    }
}
