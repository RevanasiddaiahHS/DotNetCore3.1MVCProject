using CHAND_sPAPERService.DAL;
using CHAND_sPAPERService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CHAND_sPAPERService
{
    public class Login
    {
        private LoginDAL _loginDAL;
            readonly IHttpContextAccessor _httpContextAccessor;
        public Login(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _loginDAL = new LoginDAL(_httpContextAccessor);
        }
        public async Task<int> VerifingEmailandPassword(LoginModel item)
        {
            try
            {
                var response = await _loginDAL.VerifingEmailandPassword(item);
                return response;
            }
            catch (Exception ex)
            {
                return -2;
            }
        }

        public async Task<int> Emailverification(int MentorID,string Emailverificationid)
        {
            var result = await _loginDAL.Emailverication(MentorID, Emailverificationid);
            var response = await _loginDAL.MentprGUIDUpdate(MentorID);

            return result;
        }

        public async Task<int> emailverificationsendOTP(LoginModel item)
        {
            var result = await _loginDAL.emailverificationsendOTP(item);      
            if(result>0)
            {
                item.forgot.memberid = result;
                Random r = new Random();
                string randomotp = (r.Next(1000, 9999)).ToString();
                var response = await _loginDAL.Procedure_AddOtp(result, randomotp);
                return response;
            }
            return result;
        }  
        
        public async Task<int> otpverificationwithemail(LoginModel item)
        {
            var result = await _loginDAL.otpverificationwithemail(item);      
           
            return result;
        }


        public async Task<int> savenewpassword(LoginModel item)
        {
            var result = await _loginDAL.savenewpassword(item);

            return result;
        }
    }
}
