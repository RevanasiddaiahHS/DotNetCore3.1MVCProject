using CHAND_sPAPERService.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHAND_sPAPERService.DAL
{
  public class LoginDAL:SqlDALBase
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public LoginDAL(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> VerifingEmailandPassword(LoginModel item)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {

                        @emailID = item.EmailID,
                        @password = item.Password

                    };
                    var result=await con.ExecuteScalarAsync<int>("Login_verificaion", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception Ex)
            {
                return -2;
            }
        }
        public async Task<int> Emailverication(int mentorid,string Emailverifictionid)
        {
            try
            {
                using (SqlConnection con=GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @MentorID = mentorid,
                        @googleEmailverificationID = Emailverifictionid
                    };
                    var result = await con.ExecuteScalarAsync<int>("GoogleEmailverification", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
              
            }
            catch(Exception Ex)
            {
                return -3;
            }
        }
        public async Task<int> MentprGUIDUpdate(int mentorid)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {

                        @mentorid = mentorid
                    };
                    var result = await con.ExecuteScalarAsync<int>("Procedure_deletementorid", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception EX)
            {
                return -1;
            }
        }

        public async Task<int> emailverificationsendOTP(LoginModel item)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @emailid = item.forgot.emailid
                    };
                    var result = await con.ExecuteScalarAsync<int>("Procedure_emailverification_OTP", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception Ex)
            {
                return -2;
            }
        }

        public async Task<int> Procedure_AddOtp(int memberid,string otp)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @memberid = memberid,
                        @otp = otp
                    };
                    var result = await con.ExecuteScalarAsync<int>("Procedur_Add_OTP", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception Ex)
            {
                return -3;
            }
        }

        public async Task<int> otpverificationwithemail(LoginModel item)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @emailid = item.forgot.emailid,
                        @otp = item.forgot.otp,
                        @memberid = item.forgot.memberid

                    };
                    var result = await con.ExecuteScalarAsync<int>("Procedure_OTP_Verification", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception Ex)
            {
                return -2;
            }
        }

        public async Task<int> savenewpassword(LoginModel item)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {

                        @memberid = item.forgot.memberid,
                        @Password = item.registration.Password,

                    };
                    var result = await con.ExecuteScalarAsync<int>("Add_NewPassword", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception Ex)
            {
                return -2;
            }
        }
    }
}
