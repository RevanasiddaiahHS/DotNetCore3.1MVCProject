using CHAND_sPAPERService.Models;
using CHANDsPAPERService.Models;
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
    public class HomeDAL: SqlDALBase
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public HomeDAL(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #region Files
        public async Task<int> InsertFiles(HomeModel item)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @filename = item.UploadFIle.FileName,
                        @Fileuniquepath = item.Filepath,
                        @MentorID=item.MentorID,
                        @filetype=item.filetype
                    };
                  var result=await con.ExecuteScalarAsync<int>("insertuniquefilename", param, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        public async Task<List<ListFile>> FetchAllFiles(int MentorID)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param = new
                    {
                        @mentorid = MentorID
                    };
                    return (await con.QueryAsync<ListFile>("fetchallfiles", param, commandType: CommandType.StoredProcedure)).ToList();
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public async Task<List<LocalfilesModel>> GetLocalFilesList(int mentorid,int pageNumber, int pageSize, string sortBy, string sortingOrder, string searchinputtext, string startdate, string enddate)
        {
            try
            {
                if(searchinputtext != "")
                {
                    searchinputtext = "and Filename like '%" + searchinputtext + "%'";
                }
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {
                        @Montorid= mentorid,
                        @sortBy = sortBy,
                        @sortingOrder = sortingOrder,
                        @searchinputtext = searchinputtext,
                        @pageNumber = pageNumber,
                        @pageSize = pageSize,
                        @TaskStartDate = (Convert.ToString(startdate) == null ? "" : Convert.ToString(startdate)),
                        @TaskEndDate = (Convert.ToString(enddate) == null ? "" : Convert.ToString(enddate)),

                    };
                    return (await con.QueryAsync<LocalfilesModel>("GetLocalFiles", param, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return null;
            }
        }

        public async Task<int> DeletefileLocal(int fileuniqueid, int mentorid)
        {
            try
            {
                
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {
                        @mentorid = mentorid,
                        @fileuniqueid = fileuniqueid,
                     

                    };
                    var result=await con.ExecuteScalarAsync<int>("FilesLocalDelete", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return -1;
            }
        }
        #endregion Files


        #region Images
        public async Task<List<LocalfilesModel>> Getlocalimageslist(int mentorid)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {
                        @mentorid = mentorid

                    };
                   return (await con.QueryAsync<LocalfilesModel>("GetlocalImages", param, commandType: CommandType.StoredProcedure)).ToList();                 
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return null;
            }
        }
        #endregion Images

        #region videos
        public async Task<List<LocalfilesModel>> GetLocalVideosList(int mentorid)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {
                        @mentorid = mentorid

                    };
                    return (await con.QueryAsync<LocalfilesModel>("GetLocalVideos", param, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return null;
            }
        }

        #endregion videos
        #region Audios
        public async Task<List<LocalfilesModel>> GetLocalAudiosList(int mentorid)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {
                        @mentorid = mentorid

                    };
                    return (await con.QueryAsync<LocalfilesModel>("GetLocalAudios", param, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return null;
            }
        }
        #endregion Audios

        #region PostContent
        public async Task<int> PostTexTemplate(TextContentSendModel item)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {

                        @PostTitle = item.TitleforTextContent,
                        @PostContent = item.textcontent,
                        @PostCaption = item.CaptionforTextContent,
                        //@PostDate = DateTime.Now,
                        @MentorID=item.MentorID,
                        @PostType =item.PostType
                    };
                    var result=await con.ExecuteScalarAsync<int>("insertTempararyContent", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return -1;
            }
        }

        public async Task<string> Approvepostedcontent(int TempararyID)
        {
            try
            {

                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    var param =
                    new
                    {

                        @TemparayID = TempararyID

                    };
                    var result = await con.ExecuteScalarAsync<string>("Approvepostedcontent", param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Common.SendExcepToDB(ex);
                return null;
            }
        }   
        #endregion PostContent
    }
}
