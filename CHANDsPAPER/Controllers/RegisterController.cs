using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CHAND_sPAPERService;
using CHANDsPAPERService.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

namespace CHANDsPAPER.Controllers
{
    public class RegisterController : Controller
    {
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostingEnvironment hostingEnvironment;
        private Register _register;
        public RegisterController(IHttpContextAccessor _httpcontextaccessor, IHostingEnvironment _hostingenvironment)
        {
            this.httpContextAccessor = _httpcontextaccessor;
            this.hostingEnvironment = _hostingenvironment;
            _register = new Register(_httpcontextaccessor);
        }

        public IActionResult Mentor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MentorRegistration(RegisterModel item)
        {
            if(item.Photo!= null)
            {
                string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                 string  uniquefilename=Guid.NewGuid().ToString()+"_" + item.Photo.FileName;
                string fullfilepath = Path.Combine(filepath, uniquefilename);
                item.Photo.CopyTo(new FileStream(fullfilepath,FileMode.Create));
                item.PhotoPath ="/Images/" +uniquefilename;
            }
            item.EmailverificationID = Guid.NewGuid().ToString();
            //item.EmailverificationID = "sfsdf2342sdfsdfsd23423fsdf";
            var result = await _register.MentorRegistration(item);
            if(result  !=-1)
            {
                string ActivationUrl;
                var userclaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Actor,result.ToString())
                };
                //var grantidentity = new ClaimsIdentity(userclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var userprinciple = new ClaimsPrincipal(grantidentity);
                //var props = new AuthenticationProperties();
                //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userprinciple, props).Wait();

                //MailMessage message = new MailMessage();
                //message.From = new MailAddress("demo@gmail.com", "Saklani");
                //message.To.Add("revanasiddaiah026@gmail.com");
                //message.Subject = "Verification Email";
                //ActivationUrl = "http://localhost:43200/Important_Testing/Verification.aspx?USER_ID=" + result;
                //message.Body = "<a href='" + ActivationUrl + "'>Click Here to verify your acount</a>";
                //message.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.Credentials = new System.Net.NetworkCredential("demo@gmail.com", "demo123");
                //smtp.EnableSsl = true;
                //smtp.Send(message);

                // var DefaultIPHostUserName = Common.GetConnenctionKey("DefaultIPHostUserName");
                // var sAttrFromName = Common.GetConnenctionKey("FromAddress");
                // var DefaultIPHost = Common.GetConnenctionKey("DefaultIPHost");
                //var  DefaultIPPort = Common.GetConnenctionKey("DefaultIPPort");
                // var DefaultIPHostPassword = Common.GetConnenctionKey("DefaultIPHostPassword");
                // var DefaultEnableSSL = Convert.ToBoolean(Common.GetConnenctionKey("DefaultEnableSSL"));
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

                //SmtpClient ss = new SmtpClient();
                //ss.Host = "smtpout.secureserver.net";
                //ss.Port = 80;
                //ss.Timeout = 10000;
                //ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                //ss.UseDefaultCredentials = false;
                //ss.EnableSsl = false;
                //ss.Credentials = new NetworkCredential("abc@yourdomain.com", "password");

                //MailMessage mailMsg = new MailMessage("abc@yourdomain.com", email, "subject here", "my body");
                //mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                //ss.Send(mailMsg);

                string Subject = "Email Verification";
                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(item.EmailID));
                message.From.Add(new MailboxAddress(sAttrFromName, DefaultIPHostUserName));
                message.Subject = Subject;
                var url = Request.Scheme + "://" + Request.Host.Value;
                string pagelink = url + "/Login/Emailverification?MentorID=" + result + "&EmailvarificationID="+ item.EmailverificationID;
                var Body = "<a href ='" + pagelink + "'>Click Here to verify your email</a>";
                     message.Body = new TextPart(TextFormat.Html)
                {
                    Text = Body
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
                //return RedirectToAction("Index", "Home");
            }

            return Json(result);
        }
    }
}
