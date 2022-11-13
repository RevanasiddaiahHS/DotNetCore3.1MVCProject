using CHAND_sPAPERService.DAL;
using CHANDsPAPERService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CHAND_sPAPERService
{
    public class Home
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly HomeDAL _homeDAL;
        public Home(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _homeDAL = new HomeDAL(httpContextAccessor);
        }

        #region Files
        public async Task<int> InsertFiles(HomeModel item)
        {
            try
            {
                var response =await _homeDAL.InsertFiles(item);
                return response;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }  
        public async Task<List<ListFile>> FetchAllFiles(int Mentorid)
        {
            try
            {
                var response =await _homeDAL.FetchAllFiles(Mentorid);
                return response;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Task<List<LocalfilesModel>> GetLocalFilesList(int mentorid,int pageNumber, int pageSize, string sortBy, string sortingOrder, string searchinputtext,string startdate, string enddate)
        {
            try
            {
                var response = _homeDAL.GetLocalFilesList(mentorid,pageNumber, pageSize, sortBy, sortingOrder, searchinputtext,startdate, enddate);
                return response;
            }
            catch (Exception ex)
            {
                //_KeyValue.SendExcepToDB(ex);
                return null;
            }
        }

        public async Task<int> DeletefileLocal(int fileuniqueid,int mentorid)
        {
            var response = await _homeDAL.DeletefileLocal(fileuniqueid,mentorid);
            return response;
        }

        #endregion Files

        #region Images
        public async Task<List<LocalfilesModel>> Getlocalimageslist(int mentorid)
        {
            var response = await _homeDAL.Getlocalimageslist(mentorid);
            return response;

        }
        #endregion Images

        #region videos
        public async Task<List<LocalfilesModel>> GetLocalVideosList(int mentorid)
        {
            var response = await _homeDAL.GetLocalVideosList(mentorid);
            return response;

        }
        #endregion videos

        #region Audios
        public async Task<List<LocalfilesModel>> GetLocalAudiosList(int mentorid)
        {
            var response = await _homeDAL.GetLocalAudiosList(mentorid);
            return response;

        }
        #endregion Audios
        #region TextPost
        public async Task<int> PostTexTemplate(TextContentSendModel item)
        {
            var response = await _homeDAL.PostTexTemplate(item);
            return response;

        }
        public async Task<string> Approvepostedcontent(int TempararyID)
        {
            var response = await _homeDAL.Approvepostedcontent(TempararyID);
            return response;

        }

        #endregion TextPost
    }
}
