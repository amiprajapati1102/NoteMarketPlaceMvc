using NoteMarkelPlace.DBModel;
using NoteMarkelPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NoteMarkelPlace.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        NoteMarketPlaceHtmlEntities db = new NoteMarketPlaceHtmlEntities();
        public ActionResult Index()
        {
            UserModel model = new UserModel();
            return View(model);
        }
       
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModel model)
        {

            if (ModelState.IsValid)
            {
                if ((db.Users.Where(m => m.EmailID == model.EmailID).FirstOrDefault() == null))
                {
                    User user = new User();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.EmailID = model.EmailID;
                    user.Password = model.Password;
                    user.IsEmailVerified = false;
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = model.Id;

                    user.RoleID = 3;
                    user.Code = Guid.NewGuid();
                    db.Users.Add(user);
                    db.SaveChanges();

                    SendEmailToUser(user.EmailID, user.Code.ToString());
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }
        public ActionResult Activation(string id)
        {


            var IsVerify = db.Users.Where(u => u.Code == new Guid(id)).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.IsEmailVerified = true;

                db.SaveChanges();
                ViewBag.Message = "Email Verification completed";

            }
            else
            {
                ViewBag.Message = "Invalid Request...Email not verify";

            }

            return View();
        }

        private void SendEmailToUser(string Email, string activationCode)
        {
            var GenarateUserVerificationLink = "/Register/UserVerification/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("amiprajapati1102@gmail.com", "AMi"); // set your email    
            var fromEmailpassword = "rajesh@1102";// Set your password     
            var toEmail = new MailAddress(Email);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Registration Completed-Demo";
            Message.Body = "<br /><a href = '" + string.Format("{0}://{1}/Account/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }

        public ActionResult Login()
        {
            LoginModel login = new LoginModel();
            return View(login);
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            User us = new User();
            us.EmailID = model.EmailID;
            us.Password = model.Password;
            
            if (ModelState.IsValid)
            {

                if ((db.Users.Where(m => m.EmailID== model.EmailID && m.Password == model.Password &&
                m.IsEmailVerified==true).FirstOrDefault()== null))
                {
                    if ((db.Users.Where(m => 
                m.IsEmailVerified == true).FirstOrDefault() == null))
                    {
                        ModelState.AddModelError("Error", "Email  not Confirm");

                    }

                        ModelState.AddModelError("Error", "Email Password Do not Match");
                    
                }
                else
                {
                    Session["EmailID"] = model.EmailID;
                   return  RedirectToAction("Index","Home");

                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


    }
}