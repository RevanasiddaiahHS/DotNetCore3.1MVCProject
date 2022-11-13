using CHAND_sPAPERService.DAL;
using CHANDsPAPERService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CHAND_sPAPERService
{
    public class Register
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        private RegisterDAL _registerDAL;
        public Register(IHttpContextAccessor _httpcontextaccessor)
        {
            this._httpContextAccessor = _httpcontextaccessor;
            _registerDAL = new RegisterDAL(_httpcontextaccessor);
        }
        public async Task<int> MentorRegistration(RegisterModel item)
         {
            var result = await _registerDAL.MentorRegistration(item);
            return result;
         }
    }
}
