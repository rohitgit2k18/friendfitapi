using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
   public class ExerciseTypeListResponseModel
    {
        public ExerciseTypeListResponseModel()
        {
            Response = new ExerciseTypeListResponse();
        }
        public ExerciseTypeListResponse Response { get; set; }
    }
    public class ExerciseTypeList
    {
        public Int64 Id { get; set; }
        public string TypeName { get; set; }
    }
    public class ExerciseTypeListResponse
    {
        public List<ExerciseTypeList> exerciseList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
