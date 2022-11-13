using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CHAND_sPAPERService;
using CHAND_sPAPERService.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace CHANDsPAPER.Controllers
{
    public class LoginController : Controller
    {
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostingEnvironment hostingEnvironment;
        private Login _login;
        public LoginController(IHttpContextAccessor _httpcontextaccessor, IHostingEnvironment _hostingenvironment)
        {
            this.httpContextAccessor = _httpcontextaccessor;
            this.hostingEnvironment = _hostingenvironment;
            _login = new Login(_httpcontextaccessor);
        }
        public IActionResult Index()
        {
            if(TempData["successmessage"]!=null)
            {
                ViewBag.successmessage = TempData["successmessage"];
                TempData["successmessage"] = null;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifingEmailandPassword(LoginModel item)
        {
            //item.Password = Encrypt(item.Password);
            //item.Password = "NjM2NDA4MTYzNioj";
            var response = await _login.VerifingEmailandPassword(item);
            if(response>0)
            {
                HttpContext.Session.SetString("MentorID", response.ToString());

                var userclaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Actor,response.ToString())
                };
                var grantidentity = new ClaimsIdentity(userclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var userprinciple = new ClaimsPrincipal(grantidentity);
                

                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userprinciple, props).Wait();
                Thread.CurrentPrincipal = userprinciple;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["successmessage"] = "Invalid credentials, please try with valid credentials";
                return RedirectToAction("Index", "Login");
            }
            return Json(response);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            //_cookieManager.Remove("Key");
         foreach(var cookie in HttpContext.Request.Cookies )
            {
                Response.Cookies.Delete(cookie.Key);
            }
            HttpContext.Session.Remove("TriedCookie");
            HttpContext.Session.Remove("B1paymentDB");
            HttpContext.SignOutAsync();

            HttpContext.Session.Clear();
            var response = 1;
            return Json(response);
        }

        public async Task<IActionResult> Emailverification(int MentorID,string EmailvarificationID)
        {
            var result = await _login.Emailverification(MentorID, EmailvarificationID);
            if(result>0)
            {
                HttpContext.Session.SetString("MentorID", result.ToString());

                var userclaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Actor,result.ToString())
                };
                var grantidentity = new ClaimsIdentity(userclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var userprinciple = new ClaimsPrincipal(grantidentity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userprinciple, props).Wait();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["successmessage"] = "Something went wrong!,Please try again";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> emailverificationsendOTP(LoginModel item)
        {
            var response = await _login.emailverificationsendOTP(item);
            if(response>0)
            {
                //var DefaultIPHostUserName = Common.GetConnenctionKey("DefaultIPHostUserName");
                var DefaultIPHostUserName = "revanasiddaiah22@gmail.com";
                //var sAttrFromName = Common.GetConnenctionKey("FromAddress");
                var sAttrFromName ="CHAND's PAPER";
                //var DefaultIPHost = Common.GetConnenctionKey("DefaultIPHost");
                var DefaultIPHost = "smtp.gmail.com";
                //var DefaultIPPort = Common.GetConnenctionKey("DefaultIPPort");
                var DefaultIPPort ="587";
               // var DefaultIPHostPassword = Common.GetConnenctionKey("DefaultIPHostPassword");
                var DefaultIPHostPassword = "revanasiddaiah026@gmail.com";


                string Subject = "Password reset";
                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(item.forgot.emailid));
                message.From.Add(new MailboxAddress(sAttrFromName, DefaultIPHostUserName));
                message.Subject = Subject;
                var url = Request.Scheme + "://" + Request.Host.Value;
               // string pagelink = url + "/Login/Emailverification?MentorID=" + result + "&EmailvarificationID=" + item.EmailverificationID;
                var Body = Convert.ToString(response);
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
            }
            item.response = response;
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> otpverificationwithemail(LoginModel item)
        {
            var response = await _login.otpverificationwithemail(item);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> savenewpassword(LoginModel item)
        {
            TempData["successmessage"] = "Password changed successfully, Please login with New Password to continue";
            var response = await _login.savenewpassword(item);
            return Json(response);
        }

    }
}
