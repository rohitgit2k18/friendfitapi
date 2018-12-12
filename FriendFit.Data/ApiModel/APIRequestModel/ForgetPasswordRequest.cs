using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class ForgetPasswordRequest
    {
        [Required(ErrorMessage = "Email address Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
