using CHANDsPAPERService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace CHAND_sPAPERService.DAL
{
    public class RegisterDAL : SqlDALBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegisterDAL(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> MentorRegistration(RegisterModel item)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @firstname = item.FirstName,
                        @lastname = item.LastName,
                        @emilid = item.EmailID,
                        @mobilenumber = item.MobileNumber,
                        @gender = item.Gender,
                        @password = item.Password,
                        @profilepath = item.PhotoPath,
                        @EmailverificationID = item.EmailverificationID
                    };
                    var result = await con.ExecuteScalarAsync<int>("CP_Mentor_Add", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception EX)
            {
                return -1;
            }
        }


       

    }
}
