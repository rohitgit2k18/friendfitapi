using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIRequestModel
{
   public class SignUpModelRequset
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "length err")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile Number")]
        
        //[StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Country Name")]
        public Int64 CountryId { get; set; }        
        
    }
}
