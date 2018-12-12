using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class EditExerciseRequestModel
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public Int64 WorkOutId { get; set; }
        public string ExerciseName { get; set; }
        public Int64 ExerciseTypeId { get; set; }

    }
}
