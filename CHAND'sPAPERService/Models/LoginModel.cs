using CHANDsPAPERService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CHAND_sPAPERService.Models
{
  public class LoginModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]

        public string EmailID { set; get; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { set; get; }

        public Forgotpassword forgot { set; get; }
        public int response { set; get; }
        public RegisterModel registration { set; get; }
    }
    public class Forgotpassword
    {
        [Required(ErrorMessage ="Please enter the email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string emailid { set; get; }

        [Required(ErrorMessage = "Please enter the otp sent to ur registered email")]
        public string otp { set; get; }
        public int memberid { set; get; }
    }
}
