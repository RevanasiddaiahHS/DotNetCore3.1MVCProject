using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CHAND_sPAPERService;
using CHAND_sPAPERService.DAL;
using CHANDsPAPERService.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MimeKit;
using MimeKit.Text;
using System.Configuration;
using Dapper;
using Microsoft.AspNetCore.Authorization;

namespace CHANDsPAPER.Controllers
{
    [Authorize]
    public class MentorController : Controller
    {
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostingEnvironment hostingEnvironment;
        readonly Home _home;
        SqlDALBase sql;
        public MentorController(IHttpContextAccessor _httpcontextaccessor, IHostingEnvironment _hostingenvironment)
        {
            this.httpContextAccessor = _httpcontextaccessor;
            this.hostingEnvironment = _hostingenvironment;
            _home = new Home(_httpcontextaccessor);
            sql = new SqlDALBase();
        }

        public IActionResult Index()
        {
           ViewBag.val= HttpContext.Session.GetString("MentorID");

            //if (x == null)
            //{
            //    return RedirectToAction("Logout", "Login");
            //}
            //else
            //{
                return View();
           // }
        }
        #region Files
        public IActionResult AddFiles()
        {
            if (@TempData["filesattachment"] != null)
            {
                ViewBag.fileaddedmessage = @TempData["filesattachment"];
                @TempData["filesattachment"] = null;
            }

            HomeModel item = new HomeModel();
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            //item.MentorID = 1;
            item.filetype = "files";
            return View(item);
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = 1000000000, MultipartBodyLengthLimit = 1000000000)]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Uploadfiles(HomeModel item)
        public async Task<IActionResult> Uploadfiles(HomeModel item)
        {
            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Files");
            string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            string fullfilepath = Path.Combine(filepath, uniquefilename);

            using (var localFile = System.IO.File.OpenWrite(fullfilepath))
            using (var uploadedFile = item.UploadFIle.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            //item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            item.Filepath = "/" + "Files" + "/" + uniquefilename;

            int response = await _home.InsertFiles(item);
            if (response > 0)
            {
                TempData["filesattachment"] = "File uploaded successfully";
            }
            else
            {
                TempData["filesattachment"] = "Failed to upload,try again!";
            }

            return RedirectToAction("AddFiles", "Mentor");
        }

        public async Task<IActionResult> Uploadfilestest(IFormFile[] files)
        {


            foreach (var file in files)
            {
                // Get the file name from the browser
                var fileName = System.IO.Path.GetFileName(file.FileName);

                // Get file path to be uploaded
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "upload", fileName);

                // Check If file with same name exists and delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Create a new local file and copy contents of uploaded file
                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = file.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
            //string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Files");
            //string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            //string fullfilepath = Path.Combine(filepath, uniquefilename);

            //using (var localFile = System.IO.File.OpenWrite(fullfilepath))
            //using (var uploadedFile = item.UploadFIle.OpenReadStream())
            //{
            //    uploadedFile.CopyTo(localFile);
            //}
            ////item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            //item.Filepath = "/" + "Files" + "/" + uniquefilename;

            //int response = await _home.InsertFiles(item);
            //if (response > 0)
            //{
            //    TempData["filesattachment"] = "File uploaded successfully";
            //}
            //else
            //{
            //    TempData["filesattachment"] = "Failed to upload,try again!";
            //}

            return RedirectToAction("AddFiles", "Mentor");
        }
        public async Task<IActionResult> LocalFiles()
        {

            if (@TempData["Lacalfilesdmessage"] != null)
            {
                @ViewBag.Lacalfilesdmessage = TempData["Lacalfilesdmessage"];
                @TempData["Lacalfilesdmessage"] = null;
            }
            HomeModel item = new HomeModel();
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            //item.MentorID = 1;
            
                return View(item);
           
        }

        [HttpPost]
        public async Task<IActionResult> GetLocalFilesList(int mentorid, int pageNumber, int pageSize, string startdate, string enddate)
        {
            var recordsTotal = 0;
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            string searchinputtext = ReplaceSingleQuote(HttpContext.Request.Form["search[value]"].FirstOrDefault());

            var sortingOrder = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var sortBy = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();

            var start = Request.Form["start"].FirstOrDefault();

            var length = Request.Form["length"].FirstOrDefault();
            List<LocalfilesModel> list = new List<LocalfilesModel>();

            list = await _home.GetLocalFilesList(mentorid, pageNumber, pageSize, sortBy, sortingOrder, searchinputtext, startdate + " 00:00:00", enddate + " 23:59:59");
            if (list != null && list.Count > 0)
            {
                foreach (LocalfilesModel item in list)
                {

                    item.uploadeddate = item.CreatedDate.ToString("dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    // item.uploadeddate=item.CreatedDate.ToString("h:mm tt", CultureInfo.InvariantCulture);

                }
            }
            if (list != null && list.Count > 0)
            {
                recordsTotal = list[0].TotalrowCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = list });
        }
        public static string ReplaceSingleQuote(string CompleteString)
        {
            CompleteString = CompleteString.Replace("'", "''");
            return CompleteString;
        }

        public async Task<IActionResult> Downloadfile(string filename)
        {
            //var path = Path.Combine(
            //         Directory.GetCurrentDirectory(),
            //         "wwwroot", filename);
            //  string fullfilename = Path.Combine(hostingEnvironment.WebRootPath, filename);
            string fullfilename = hostingEnvironment.WebRootPath + filename;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fullfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(fullfilename), Path.GetFileName(fullfilename));
        }
        public async Task<IActionResult> DeletefileLocal(int fileuniqueid, int mentorid, string fileuniquepath)
        {
            string filepath = hostingEnvironment.WebRootPath + "" + fileuniquepath;
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            int response = await _home.DeletefileLocal(fileuniqueid, mentorid);
            if (response != -1)
            {
                TempData["Lacalfilesdmessage"] = "File deleted successfully";
            }
            else
            {
                TempData["Lacalfilesdmessage"] = "Something went wrong, Please try again!y";
            }

            return Json(response);
        }

        private string GetContentType(string path)
        {

            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4", "MP4 File"}
            };
        }
        #endregion Files


        #region Images

        public IActionResult AddImages()
        {
            HomeModel item = new HomeModel();


            if (@TempData["imagesuccessmessage"] != null)
            {
                ViewBag.imagesuccessmessage = @TempData["imagesuccessmessage"];
                @TempData["imagesuccessmessage"] = null;
            }
            //item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.filetype = "Images";
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(HomeModel item)
        {
            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Images");
            string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            string fullfilepath = Path.Combine(filepath, uniquefilename);

            using (var localFile = System.IO.File.OpenWrite(fullfilepath))
            using (var uploadedFile = item.UploadFIle.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            //item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            item.Filepath = "/" + "Images" + "/" + uniquefilename;

            int response = await _home.InsertFiles(item);
            if (response > 0)
            {
                TempData["imagesuccessmessage"] = "Image uploaded successfully";
            }
            else
            {
                TempData["imagesuccessmessage"] = "Failed to upload,try again!";
            }

            return RedirectToAction("AddImages", "Mentor");
        }

        public async Task<IActionResult> LocalImages()
        {
            HomeModel item = new HomeModel();
            //item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.GetLocalImages = await _home.Getlocalimageslist(item.MentorID);

            item.filetype = "Images";


            if (@TempData["Localimagesuccessmessage"] != null)
            {
                ViewBag.Localimagesuccessmessage = @TempData["Localimagesuccessmessage"];
                @TempData["Localimagesuccessmessage"] = null;
            }
            return View(item);
        }

        public async Task<IActionResult> DelteLocalImage(int fileuniqueid, int mentorid, string fileunquepath)
        {
            string filepath = hostingEnvironment.WebRootPath + "" + fileunquepath;
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            int response = await _home.DeletefileLocal(fileuniqueid, mentorid);
            if (response != -1)
            {
                TempData["Localimagesuccessmessage"] = "Image deleted successfully";
            }
            else
            {
                TempData["Localimagesuccessmessage"] = "Something went wrong, Please try again!y";
            }

            return Json(response);
        }
        #endregion Images

        #region Videos
        public async Task<IActionResult> AddVideos()
        {
            HomeModel item = new HomeModel();


            if (@TempData["videosuccessmessage"] != null)
            {
                ViewBag.videosuccessmessage = @TempData["videosuccessmessage"];
                @TempData["videosuccessmessage"] = null;
            }
            //item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.filetype = "Videos";
            return View(item);
        }


        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = 1000000000, MultipartBodyLengthLimit = 1000000000)]
        public async Task<IActionResult> Uploadvideos(HomeModel item)
        {
            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Videos");
            string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            string fullfilepath = Path.Combine(filepath, uniquefilename);

            using (var localFile = System.IO.File.OpenWrite(fullfilepath))
            using (var uploadedFile = item.UploadFIle.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            //item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            item.Filepath = "/" + "videos" + "/" + uniquefilename;

            int response = await _home.InsertFiles(item);
            if (response > 0)
            {
                TempData["videosuccessmessage"] = "File uploaded successfully";
            }
            else
            {
                TempData["videosuccessmessage"] = "Failed to upload,try again!";
            }

            return RedirectToAction("AddVideos", "Mentor");
        }

        public async Task<IActionResult> LocalVideos()
        {
            HomeModel item = new HomeModel();
           // item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.GetLocalImages = await _home.GetLocalVideosList(item.MentorID);

            item.filetype = "videos";


            if (@TempData["Localvideosuccessmessage"] != null)
            {
                ViewBag.Localvideosuccessmessage = @TempData["Localvideosuccessmessage"];
                @TempData["Localvideosuccessmessage"] = null;
            }
            return View(item);
        }
        public async Task<IActionResult> DelteLocalVideo(int fileuniqueid, int mentorid, string fileunquepath)
        {
            string filepath = hostingEnvironment.WebRootPath + "" + fileunquepath;
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            int response = await _home.DeletefileLocal(fileuniqueid, mentorid);
            if (response != -1)
            {
                TempData["Localvideosuccessmessage"] = "video deleted successfully";
            }
            else
            {
                TempData["Localvideosuccessmessage"] = "Something went wrong, Please try again!y";
            }

            return Json(response);
        }
        #endregion videos
        #region Text
        public async Task<IActionResult> ContentPost()
        {
            //@ViewBag.templatecontent = "<span class='text-danger'>Helllo world</span>";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostTexTemplate(TextContentSendModel item)
        {
            try
            {

            int response = 0;
            if (item.textcontent == null)
            {
                response = 0;
            }
            else
            {
                item.PostType = "Content";
                item.MentorID = Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
                response = await _home.PostTexTemplate(item);

                SqlConnection con = sql.GetConnection();
                //RegisterModel register = new RegisterModel();
              string query= "SELECT FirstName,LastName,ProfilePath as PhotoPath,EmailID FROM tb_mentors where MemberID=" + item.MentorID;
                //SqlDataReader sdr =SqlHelper.ExecuteNonQuery(con, CommandType.Text, query);
                RegisterModel register = new RegisterModel();
                 register = (await con.QueryAsync<RegisterModel>(query, commandType: CommandType.Text)).FirstOrDefault();

                var uniqueid = Guid.NewGuid();
                await con.ExecuteScalarAsync<int>("insert into GuidverificationforClick(GUIDCreated,Postid) values('" + uniqueid + "',"+ response + ")", commandType: CommandType.Text);

                var url = Request.Scheme + "://" + Request.Host.Value;

                StringBuilder myStringBuilder = new StringBuilder();
                myStringBuilder.Append("<html><div class='page-container'>");
                myStringBuilder.Append("<div class='page-content'>");
                myStringBuilder.Append("<div class='col-md-3'>");
                myStringBuilder.Append("</div>");
                myStringBuilder.Append("<div class='col-md-6'>");
                myStringBuilder.Append("<div class='card'>");
                myStringBuilder.Append("<div class='d-flex justify-content-between p-2 px-3'>");
                myStringBuilder.Append("<div class='d-flex flex-row align-items-center'>");

                myStringBuilder.Append("<img src = '"+ register.PhotoPath + "' style ='border-radius: 50% !important;");
   myStringBuilder.Append("display: flex;justify - content: center;align - items: center;height: 50px;width: 50px'>");
                myStringBuilder.Append("<div class='d-flex flex-column ml-2'><span class='font-weight-bold'>"+ register.FirstName+" "+register.LastName+ "</span></div></div>");
                myStringBuilder.Append("<div class='d-flex flex-row mt-1 ellipsis'><small class='mr-2'>20 mins</small><i class='fa fa-ellipsis-h'></i></div></div>");
                myStringBuilder.Append("<div class='row'> <span class='text-primary pl-10'>"+item.TitleforTextContent + "</span></div>");
                //< img src = "https://i.imgur.com/xhzhaGA.jpg" class="img-fluid">
                myStringBuilder.Append("<div>"+item.textcontent+"</div>");
myStringBuilder.Append("<div class='p-2'><p class='text-justify'>"+item.CaptionforTextContent + "</p> </div>");
                myStringBuilder.Append("</div></div><div class='col-md-3'></div></div></div>");
                myStringBuilder.Append("<div><a href ="+ url + "/Mentor/canceleedpostedcontent?EmailID=" + register.EmailID + "&GUID="+ uniqueid + "&TempararyID="+ response + " style ='color:red'>Cancel</a> <a href =" + url + "/Mentor/Approvepostedcontent?TempararyID=" + response+ "&GUID=" + uniqueid + " style ='color:green;'> Approve </a></div></html>");
                //myStringBuilder.Append("<div class='d-flex flex-row align-items-center'>");
                //myStringBuilder.Append("<div class='d-flex flex-row align-items-center'>");

                    

                   

               
                
                
           // ");

               await EmailSendfumction(Convert.ToString(myStringBuilder),"Content for Approval","revanasiddaiah22@gmail.com");

               await EmailSendfumction("You'r Posted content is sent for author for approval,as soon as it is approved,you will receive mailregarding this.","Requested message", register.EmailID);
            }
            return Json(response);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion Text
        #region Audio
        public IActionResult AddAudio()
        {
            HomeModel item = new HomeModel();


            if (@TempData["audiosuccessmessage"] != null)
            {
                ViewBag.audiosuccessmessage = @TempData["audiosuccessmessage"];
                @TempData["audiosuccessmessage"] = null;
            }
            //item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.filetype = "Audios";
            return View(item);
        }
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = 1000000000, MultipartBodyLengthLimit = 1000000000)]
        public async Task<IActionResult> UploadAudio(HomeModel item)
        {
            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Audios");
            string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            string fullfilepath = Path.Combine(filepath, uniquefilename);

            using (var localFile = System.IO.File.OpenWrite(fullfilepath))
            using (var uploadedFile = item.UploadFIle.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            //item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            item.Filepath = "/" + "Audios" + "/" + uniquefilename;

            int response = await _home.InsertFiles(item);
            if (response > 0)
            {
                TempData["audiosuccessmessage"] = "Audio uploaded successfully";
            }
            else
            {
                TempData["audiosuccessmessage"] = "Failed to upload,try again!";
            }

            return RedirectToAction("AddAudio", "Mentor");
        }

        public async Task<IActionResult> LocalAudios()
        {
            HomeModel item = new HomeModel();
            //item.MentorID = 1;
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
            item.GetLocalImages = await _home.GetLocalAudiosList(item.MentorID);

            item.filetype = "Audios";


            if (@TempData["LocalAudiosuccessmessage"] != null)
            {
                ViewBag.LocalAudiosuccessmessage = @TempData["LocalAudiosuccessmessage"];
                @TempData["LocalAudiosuccessmessage"] = null;
            }
            return View(item);
        }

        public async Task<IActionResult> DeleteLocalAudios(int fileuniqueid, int mentorid, string fileunquepath)
        {
            string filepath = hostingEnvironment.WebRootPath + "" + fileunquepath;
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            int response = await _home.DeletefileLocal(fileuniqueid, mentorid);
            if (response != -1)
            {
                TempData["LocalAudiosuccessmessage"] = "Audio deleted successfully";
            }
            else
            {
                TempData["LocalAudiosuccessmessage"] = "Something went wrong, Please try again!y";
            }

            return Json(response);
        }
        #endregion Audio

        public async Task<bool> EmailSendfumction(string body,string subject,string To)
        {
            
            //var DefaultIPHostUserName = Common.GetConnenctionKey("DefaultIPHostUserName");
            var DefaultIPHostUserName = "revanasiddaiah22@gmail.com";
            //var sAttrFromName = Common.GetConnenctionKey("FromAddress");
            var sAttrFromName = "CHAND's PAPER";
            //var DefaultIPHost = Common.GetConnenctionKey("DefaultIPHost");
            var DefaultIPHost = "smtp.gmail.com";
            //var DefaultIPPort = Common.GetConnenctionKey("DefaultIPPort");
            var DefaultIPPort = "587";
            // var DefaultIPHostPassword = Common.GetConnenctionKey("DefaultIPHostPassword");
            var DefaultIPHostPassword = "revanasiddaiah026@gmail.com";

            string Subject = subject;
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(To));
            message.From.Add(new MailboxAddress(sAttrFromName, DefaultIPHostUserName));
            message.Subject = Subject;
            var url = Request.Scheme + "://" + Request.Host.Value;
            // string pagelink = url + "/Login/Emailverification?MentorID=" + result + "&EmailvarificationID=" + item.EmailverificationID;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {

                try
                {
                    client.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                    client.Connect(DefaultIPHost, Convert.ToInt16(DefaultIPPort));

                    client.Authenticate(DefaultIPHostUserName, DefaultIPHostPassword);

                    client.Send(message);

                    client.Disconnect(true);
                }

                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message.ToString());
                }
            }
            return true;
        }


        public async Task<string> Approvepostedcontent(int TempararyID,string GUID)
        {
            SqlConnection con = sql.GetConnection();
            var guidselected = (await con.QueryAsync<string>("select GUIDCreated from GuidverificationforClick where Postid="+ TempararyID, commandType: CommandType.Text)).FirstOrDefault();
            if (guidselected == GUID)
            {
                var NewGUID = Guid.NewGuid();
                await con.ExecuteScalarAsync<int>("Update GuidverificationforClick set GUIDCreated='"+ NewGUID+ "' where Postid="+ TempararyID, commandType: CommandType.Text);

                var response = await _home.Approvepostedcontent(TempararyID);
                await EmailSendfumction("Your Posted content Approved and now all users can watch your post", "Content Approval Confirmation", response);
                return "Thank you";
            }
            return "You can Approve or Cancel only once";
        }

        public async Task<string> canceleedpostedcontent(string EmailID,string GUID,int TempararyID)
        {
            SqlConnection con = sql.GetConnection();
            var guidselected = (await con.QueryAsync<string>("select GUIDCreated from GuidverificationforClick where Postid=" + TempararyID, commandType: CommandType.Text)).FirstOrDefault();
            if (guidselected == GUID)
            {
                var NewGUID = Guid.NewGuid();
                await con.ExecuteScalarAsync<int>("Update GuidverificationforClick set GUIDCreated='" + NewGUID + "' where Postid=" + TempararyID, commandType: CommandType.Text);

                await EmailSendfumction("Your Posted content was rejected by author, due to some reasons", "Content Approval Confirmation", EmailID);
                return "Thank you";
            }
            return "You can Approve or Cancel only once";
        }
    }
}

