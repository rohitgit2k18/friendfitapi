using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class LoginModelRequest
    {
        [Required(ErrorMessage = "Email address Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "length err")]
        public string Password { get; set; }
    }
}
