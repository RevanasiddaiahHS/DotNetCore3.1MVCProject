using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CHANDsPAPERService.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { set; get; }

        public string LastName { set; get; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string EmailID { set; get; }

        [Required(ErrorMessage = "Please enter mobile number")]
        public long MobileNumber { set; get; }

        [Required(ErrorMessage = "Please enter password")]
        [MinLength(6, ErrorMessage = "Required atleast 6 character")]

        public string Password { set; get; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password",ErrorMessage= "Password and confirm password mis match")]
        public string Confirmpassword { set; get; }

        public string Gender { set; get; }
        public IFormFile Photo { set; get; }
        public string PhotoPath { set; get; }
        public string uniqueid { set; get; }
        public string @EmailverificationID { set; get; }
    }
}
