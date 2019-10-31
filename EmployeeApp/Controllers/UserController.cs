using EmployeeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace EmployeeApp.Controllers
{
    public class UserController : ApiController
    {
        private EmployeeDBEntities db = new EmployeeDBEntities();

        [HttpPost]
        public HttpResponseMessage Login(UserLogin login)
        {
            string message = "";
            using (EmployeeDBEntities dc = new EmployeeDBEntities())
            {
                var v = dc.UserLogins.Where(a =>a.UserName  == login.UserName).FirstOrDefault();
               
                if (v != null)
                {
                    if (string.Compare(Hash(login.Password), v.Password) == 0)
                    {
                       // AuthenticationHeaderValue authentiticationvalue = new AuthenticationHeaderValue();
                        //authentiticationvalue.Parameter=input

                        UserLogin user=db.UserLogins.Find(v.UserId);
                        user.LoginCount++;
                        db.SaveChanges();
                        return Request.CreateResponse<string>(HttpStatusCode.OK, "loginsuccess"); 
                    }
                    else
                    {
                        message = "Invalid";
                    }
                }
                else
                {
                    if (login.UserType == 3)
                    {
                        var user = dc.Registers.Where(a => a.Email == login.UserName).FirstOrDefault();
                        if (user==null)
                        {
                            user.Password = Hash(login.Password);
                            user.TermAndCondition = true;
                            user.UserType = login.UserType;
                            user.ConfirmPassword = Hash(login.Password);
                            db.Registers.Add(user);
                            db.SaveChanges();

                            login.LoginCount = 1;
                            login.Password = Hash(login.Password);
                            login.RegisterId = user.Id;
                            db.UserLogins.Add(login);
                            db.SaveChanges();
                        }
                       else
                        {
                            login.LoginCount = 1;
                            login.Password = Hash(login.Password);
                            login.RegisterId = user.Id;
                            db.UserLogins.Add(login);
                            db.SaveChanges();
                        }

                        message = "loginsuccess";
                    }
                    else if(login.UserType==1)
                    {
                        
                        var user = dc.Registers.Where(a => a.Email == login.UserName).FirstOrDefault();
                        if(user!=null)
                        {
                            if (string.Compare(Hash(login.Password), user.Password) == 0)
                            {
                                login.LoginCount = 1;
                                login.Password = user.Password;
                                login.RegisterId = user.Id;
                                db.UserLogins.Add(login);
                                db.SaveChanges();
                                message = "loginsuccess";
                            }
                            message = "Error";
                        }
                        message = "Error";

                    }
                    else
                    {
                         message = "Invalid";
                    }
                   
                }
            }
            return Request.CreateResponse<string>(HttpStatusCode.OK, message); 
        }


        [HttpPost]
        //[Authorize(Users = "Amit")]
        public IHttpActionResult Registration(Register user)
        {
           //bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email is already Exist
                var isExist = IsEmailExist(user.Email);
                //var isExist = true;
                if (isExist && user.UserType == 1)//UserType =1 click on Sign Up button
                {
                    message = "Emailalreadyexist";
                    return Ok(message);
                }
                else if(isExist && user.UserType==3)
                {
                    message = "loginsuccess";
                    return Ok(message);
                }
                else if (!(isExist) && user.UserType == 3)
                {
                    user.ConfirmPassword = user.Password;
                }
                //else
                //{

                //}
                #endregion

                #region Generate Activation Code
                //user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing
                user.Password = Hash(user.Password);
                user.ConfirmPassword = Hash(user.ConfirmPassword); //
                #endregion
                //user.IsEmailVerified = false;

                #region Save to Database
                using (EmployeeDBEntities dc = new EmployeeDBEntities())
                {
                    //dc.Registrations.Add(user);
                    dc.Registers.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    //SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    //message = "Registration successfully done. Account activation link " +
                    //    " has been sent to your email id:" + user.Email;
                    //Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            //ViewBag.Message = message;
            //ViewBag.Status = Status;
            return Ok(user);
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (EmployeeDBEntities dc = new EmployeeDBEntities())
            {
                var v = dc.Registers.Where(a => a.Email == emailID).FirstOrDefault();
               // var v = dc.SignUpUser(0, "", "", emailID.ToString(), "", true, "","select");
               // int ll =1002;
                
                
               // var l = dc.SignUpUser(ll, "", "", "", "", true, "", "delete");
                return v != null;
            }
        }


        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var link = "DummyLinke";
            var fromEmail = new MailAddress("dotnetawesome@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "********"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }


        [HttpGet]
        public IHttpActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (EmployeeDBNewEntities dc = new EmployeeDBNewEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                // Confirm password does not match issue on save changes
                var v = dc.Registrations.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    return Ok("InvalidRequest");
                }
            }
            //ViewBag.Status = Status;
            return Ok("success");
        }
        


    }
}
