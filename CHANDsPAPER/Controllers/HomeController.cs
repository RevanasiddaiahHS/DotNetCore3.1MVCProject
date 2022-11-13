using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CHANDsPAPER.Models;
using Microsoft.AspNetCore.Authorization;
using CHANDsPAPERService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Claims;
using System.Threading;
using CHAND_sPAPERService;

namespace CHANDsPAPER.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostingEnvironment hostingEnvironment;
        readonly Home _home;
        //  public HomeController(ILogger<HomeController> logger)
        public HomeController(IHttpContextAccessor _httpcontextaccessor, IHostingEnvironment _hostingenvironment, ILogger<HomeController> logger)
        {
            this.httpContextAccessor = _httpcontextaccessor;
            this.hostingEnvironment = _hostingenvironment;
            _logger = logger;
            _home = new Home(_httpcontextaccessor);
        }



        public async  Task<IActionResult> Index()
        {
            HomeModel item = new HomeModel();
            
            item.MentorID =Convert.ToInt32(HttpContext.Session.GetString("MentorID"));
         // item.MentorID=1;

            item.extractfiles = await _home.FetchAllFiles(item.MentorID);
             
            return View(item);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = 1000000000, MultipartBodyLengthLimit = 1000000000)]
        [Consumes("multipart/form-data")]
        protected async Task<IActionResult> Uploadfiles(HomeModel item)
        {
            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Files");
            string uniquefilename = Guid.NewGuid().ToString() + "_" + item.UploadFIle.FileName;
            string fullfilepath = Path.Combine(filepath, uniquefilename);
            item.UploadFIle.CopyTo(new FileStream(fullfilepath, FileMode.Create));
            item.Filepath = "/" + "Files" + "/" + uniquefilename;

         int response = await _home.InsertFiles(item);
            


            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Downloadfile(string filename)
        {
            //var path = Path.Combine(
            //         Directory.GetCurrentDirectory(),
            //         "wwwroot", filename);
          //  string fullfilename = Path.Combine(hostingEnvironment.WebRootPath, filename);
            string fullfilename = hostingEnvironment.WebRootPath+filename;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fullfilename, FileMode.Open,FileAccess.Read, FileShare.ReadWrite))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(fullfilename), Path.GetFileName(fullfilename));
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

    }
}
