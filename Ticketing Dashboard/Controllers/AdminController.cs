// Decompiled with JetBrains decompiler
// Type: Ticketing_Dashboard.Controllers.AdminController
// Assembly: Ticketing Dashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E2BA73D6-734D-47BB-8E89-378BFC2CE176
// Assembly location: C:\Users\PenTester07\wwwroot\bin\Ticketing Dashboard.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web.Mvc;
using Ticketing_Dashboard.Models;
using Ticketing_Dashboard.Utility;

namespace Ticketing_Dashboard.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult MailManager()
        {
            if(Session["UserRole"] == null)
            {
                return Redirect("/Admin/AdminLogin");
            }
            if(Session["UserRole"].ToString() == "admin")
            {
                TicketingToolDBEntities db = new TicketingToolDBEntities();
                ViewBag.users = db.Mail_Master.ToList();
                return View();
            }
            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMail(Mail_Master data)
        {
            if (Session["UserRole"] == null)
            {
                return Redirect("/Admin/AdminLogin");
            }
            if (Session["UserRole"].ToString() == "admin")
            {
                TicketingToolDBEntities db = new TicketingToolDBEntities();
                db.Mail_Master.Add(data);
                db.SaveChanges();
            }
            else
            {
                return Redirect("/Admin/AdminLogin");
            }
            return Redirect("/Admin/MailManager");
        }

        [HttpGet]
        public ActionResult DeleteMail(string id)
        {
            if (Session["UserRole"] == null)
            {
                return Redirect("/Admin/AdminLogin");
            }
            if (Session["UserRole"].ToString() == "admin")
            {
                try
                {
                    int pkID = Convert.ToInt32(id);
                    TicketingToolDBEntities db = new TicketingToolDBEntities();
                    var mail = db.Mail_Master.Where(x => x.id == pkID).ToList();
                    if (mail.Count == 1)
                    {
                        db.Mail_Master.Remove(mail[0]);
                        db.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(404);
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(404);
                }
            }
            else
            {
                return Redirect("/Admin/AdminLogin");
            }
            return Redirect("/Admin/MailManager");
        }


        public ActionResult AdminLogin(string emailID, string password)
        {
            if (emailID != null && password != null)
            {
                TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
                try
                {
                    string passwordHash = CryptographyManager.ComputeSha256Hash(password);
                    List<Login_Master> list = ticketingToolDbEntities.Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>)(x => x.emailID == emailID && x.loginPassword == passwordHash && x.loginStatus == "active")).ToList<Login_Master>();
                    if (list.Count != 1)
                    {
                        this.TempData["AdminMessage"] = (object)"Invalid Username Or Password";
                        this.TempData["AdminMessageType"] = (object)"warning";
                        this.TempData.Keep("AdminMessage");
                        this.TempData.Keep("AdminMessageType");
                    }
                    else
                    {
                        TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
                        this.Session["UserID"] = (object)Convert.ToInt32(list[0].loginPk);
                        this.Session["UserName"] = (object)textInfo.ToTitleCase(list[0].username);
                        this.Session["EmailID"] = (object)list[0].emailID;
                        this.Session["UserRole"] = (object)list[0].loginRole;
                        return (ActionResult)this.Redirect("/Admin/AdminDashboard");
                    }
                }
                catch (Exception ex)
                {
                    this.TempData["AdminMessage"] = (object)"Invalid Username Or Password";
                    this.TempData["AdminMessageType"] = (object)"warning";
                    this.TempData.Keep("AdminMessage");
                    this.TempData.Keep("AdminMessageType");
                }
            }
            return (ActionResult)this.View();
        }

        public ActionResult CreateUser(Login_Master data = null)
        {
            if (this.HttpContext.Request.HttpMethod == "POST")
            {
                TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
                Login_Master entity = new Login_Master();
                DbSet<Login_Master> loginMaster1 = ticketingToolDbEntities.Login_Master;
                Expression<Func<Login_Master, bool>> predicate = (Expression<Func<Login_Master, bool>>)(x => x.isDeleted == "No");
                foreach (Login_Master loginMaster2 in loginMaster1.Where<Login_Master>(predicate).ToList<Login_Master>())
                {
                    if (loginMaster2.username == data.username)
                    {
                        this.TempData["CreateUserMessageType"] = (object)"warning";
                        this.TempData["CreateUserMessage"] = (object)"Username Already Exists";
                        return (ActionResult)this.View();
                    }
                    if (loginMaster2.emailID == data.emailID)
                    {
                        this.TempData["CreateUserMessageType"] = (object)"warning";
                        this.TempData["CreateUserMessage"] = (object)"An account already exists with this email ID";
                        return (ActionResult)this.View();
                    }
                }
                entity.loginPk = ticketingToolDbEntities.Login_Master.ToList<Login_Master>().Count;
                entity.username = data.username;
                entity.emailID = data.emailID;
                string str = CryptographyManager.ValidatedPasswordChecks(data.loginPassword);
                if (str != "Correct")
                {
                    this.TempData["CreateUserMessageType"] = (object)"warning";
                    this.TempData["CreateUserMessage"] = (object)str;
                    return (ActionResult)this.View();
                }
                entity.loginPassword = CryptographyManager.ComputeSha256Hash(data.loginPassword);
                entity.loginRole = data.loginRole;
                entity.loginStatus = "active";
                entity.isDeleted = "No";
                entity.createdDate = new DateTime?(Convert.ToDateTime((object)DateTime.Now, (IFormatProvider)new CultureInfo("hi-IN")));
                ticketingToolDbEntities.Login_Master.Add(entity);
                ticketingToolDbEntities.SaveChanges();
                this.TempData["CreateUserMessageType"] = (object)"success";
                this.TempData["CreateUserMessage"] = (object)"User Created Successfully";
                this.TempData.Keep("CreateUserMessageType");
                this.TempData.Keep("CreateUserMessage");
                return (ActionResult)this.Redirect("/Admin/ListUsers?type=all");
            }
            if (!(this.HttpContext.Request.HttpMethod == "GET"))
                return (ActionResult)this.View();
            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            return this.Session["UserRole"].ToString() == "admin" ? (ActionResult)this.View() : (ActionResult)this.Redirect("/Admin/AdminLogin");
        }

        public ActionResult ListUsers()
        {
            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (this.Session["UserRole"].ToString() == "admin")
            {
                TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
                List<Login_Master> loginMasterList = new List<Login_Master>();
                List<Login_Master> list = ticketingToolDbEntities.Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>)(x => x.isDeleted == "No")).ToList<Login_Master>();
                ViewBag.users = list;
                return (ActionResult)this.View();
            }
            return new HttpStatusCodeResult(403);
        }
            public ActionResult AdminDashboard()
            {
                if (this.Session["UserRole"] == null)
                    return (ActionResult)this.Redirect("/Admin/AdminLogin");
                if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin") && !(this.Session["UserRole"].ToString() == "laptopadmin") && !(this.Session["UserRole"].ToString() == "onboardingadmin"))
                    return (ActionResult)this.Redirect("/Admin/AdminLogin");
                TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
                List<SR_Request> list1 = ticketingToolDbEntities.SR_Request.Where<SR_Request>((Expression<Func<SR_Request, bool>>)(x => x.isDeleted == (bool?)false)).ToList<SR_Request>();
                List<Laptop_Master> list2 = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>)(x => x.isDeleted == "No")).ToList<Laptop_Master>();
                List<Onboarding_Master> list3 = ticketingToolDbEntities.Onboarding_Master.Where(x=> x.isDeleted == "No").ToList();
            var upcoming = new List<Onboarding_Master>();
                
                foreach (var entry in list3)
                {
                    if (entry.actualJoiningDate == null)
                    {
                        var diff = entry.expectedJoiningDate.Value - DateTime.Now;
                        if ((diff.Days <= 7) && (diff.Days >= 0))
                        {
                            upcoming.Add(entry);
                        }
                    }
                    else
                    {
                        var diff = entry.actualJoiningDate.Value - DateTime.Now;
                        if ((diff.Days <= 7) && (diff.Days >= 0))
                        {
                            upcoming.Add(entry);
                        }
                    }
                }

            ViewBag.TotalRequestCount = Convert.ToString(list1.Count);
                ViewBag.PendingRequestsCount = Convert.ToString(list1.Where(x => x.status == "Pending").ToList().Count);
                ViewBag.SLARequestsCount = Convert.ToString(SLAManager.getSLASRRequests().Count);
                ViewBag.ClosedRequestsCount = Convert.ToString(list1.Where(x => x.status == "Closed").ToList().Count);

                ViewBag.TotalLaptopRequestsCount = Convert.ToString(list2.Count);
                ViewBag.PendingLaptopRequestsCount = Convert.ToString(list2.Where(x => x.status == "Pending").ToList().Count);
                ViewBag.SLABreakFixCount = Convert.ToString(SLAManager.getSLABreakFixRequests().Count);
                ViewBag.SLAOnshoreCount = Convert.ToString(SLAManager.getSLAEmployeeMovingOnshoreRequests().Count);
                ViewBag.ClosedLaptopRequestsCount = Convert.ToString(list2.Where(x => x.status == "Closed").ToList().Count);

            ViewBag.TotalOnboardingRequests = Convert.ToString(list3.Count);
            ViewBag.UpcomingOnboardingRequests = Convert.ToString(upcoming.Count);
            ViewBag.RENEGEOnboardingRequests = Convert.ToString(list3.Where(x=> x.status == 2).ToList().Count);
            ViewBag.FulfilledOnboardingRequests = Convert.ToString(list3.Where(x => x.status == 13).ToList().Count);

            return View();
            } 

        public ActionResult ForgotPassword(string emailID = null)
        {
            if (this.HttpContext.Request.HttpMethod == "GET")
                return (ActionResult)this.View();
            if (!(this.HttpContext.Request.HttpMethod == "POST"))
                return (ActionResult)new HttpStatusCodeResult(404);
            if (new TicketingToolDBEntities().Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>)(x => x.isDeleted == "No" && x.emailID == emailID)).ToList<Login_Master>().Count == 1)
                Mail.SendForgotPasswordMail(emailID);
           return (ActionResult)this.View();
        }

        public ActionResult ResetForgotPassword(
          string token = null,
          string newPassword = null,
          string confirmNewPassword = null)
        {
            if (this.HttpContext.Request.HttpMethod == "GET")
            {
                string str = CryptographyManager.DecryptString(token);
                if (!(str != ""))
                    return (ActionResult)new HttpStatusCodeResult(404);
                string emailID = str.Split('~')[0];
                DateTime dateTime = new DateTime(1970, 1, 1).Add(TimeSpan.FromSeconds((double)Convert.ToInt64(str.Split('~')[1])));
                if (Convert.ToDateTime((object)DateTime.Now, (IFormatProvider)new CultureInfo("hi-IN")).Subtract(dateTime).TotalMinutes > 30.0)
                    return (ActionResult)new HttpStatusCodeResult(404);
                if (new TicketingToolDBEntities().Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>)(x => x.isDeleted == "No" && x.emailID == emailID)).ToList<Login_Master>().Count != 1)
                    return (ActionResult)new HttpStatusCodeResult(404);
               return (ActionResult) this.View();
      }
      if (!(this.HttpContext.Request.HttpMethod == "POST"))
        return (ActionResult) new HttpStatusCodeResult(404);
      TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
      string emailID1 = CryptographyManager.DecryptString(token).Split('~')[0];
      Login_Master entity = ticketingToolDbEntities.Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>) (x => x.emailID == emailID1)).ToList<Login_Master>()[0];
      if (newPassword == confirmNewPassword)
      {
        string str = CryptographyManager.ValidatedPasswordChecks(newPassword);
        if (str == "Correct")
        {
          entity.loginPassword = CryptographyManager.ComputeSha256Hash(newPassword);
          ticketingToolDbEntities.Entry<Login_Master>(entity).State = EntityState.Modified;
          ticketingToolDbEntities.SaveChanges();
        }
                else
                {
                    ViewBag.resetMsg = str;
                    ViewBag.resetMsgType = "warning";
                }
      }
      else
      {
                ViewBag.resetMsg = "Old and New password cannot be same";
                ViewBag.resetMsgType = "warning";

      }
      return (ActionResult) this.View();
    }

        public ActionResult ResetPassword(
      string oldPassword = null,
      string newPassword = null,
      string confirmNewPassword = null)
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (this.HttpContext.Request.HttpMethod == "GET")
        return (ActionResult) this.View();
      if (this.HttpContext.Request.HttpMethod == "POST")
      {
        TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
        string current_username = this.Session["UserName"].ToString();
        Login_Master entity = ticketingToolDbEntities.Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>) (x => x.username == current_username)).ToList<Login_Master>()[0];
                if (CryptographyManager.ComputeSha256Hash(oldPassword) == entity.loginPassword)
                {
                    if (newPassword == confirmNewPassword)
                    {
                        if (oldPassword == newPassword)
                        {
                            ViewBag.resetMsgType = "warning";
                            ViewBag.resetMsg = "Old and New password cannot be same";
                        }
                        else
                        {
                            string str = CryptographyManager.ValidatedPasswordChecks(newPassword);
                            if (str == "Correct")
                            {
                                entity.loginPassword = CryptographyManager.ComputeSha256Hash(newPassword);
                                ticketingToolDbEntities.Entry<Login_Master>(entity).State = EntityState.Modified;
                                ticketingToolDbEntities.SaveChanges();
                            }
                            else
                            {
                                ViewBag.resetMsgType = "warning";
                                ViewBag.resetMsg = str;

                            }
                        }
                    }
                    else
                    {
                        ViewBag.resetMsgType = "warning";
                        ViewBag.resetMsg = "New and Confirm New password are not same";
                    }
                }
                else
                {
                    ViewBag.resetMsgType = "warning";
                    ViewBag.resetMsg = "Incorrect Old Password";
                }
      }
      return (ActionResult) this.View();
    }

    public ActionResult DeleteUser(string id)
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (this.Session["UserRole"].ToString() == "admin")
      {
        try
        {
          TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
          int userID = Convert.ToInt32(id);
          List<Login_Master> list = ticketingToolDbEntities.Login_Master.Where<Login_Master>((Expression<Func<Login_Master, bool>>) (x => x.isDeleted == "No" && x.loginPk == userID)).ToList<Login_Master>();
          if (list.Count == 1)
          {
            list[0].isDeleted = "Yes";
            this.TempData["CreateUserMessage"] = (object) "User Successfully Deleted";
            this.TempData["CreateUserMessageType"] = (object) "success";
            this.TempData.Keep("CreateUserMessageType");
            this.TempData.Keep("CreateUserMessage");
            ticketingToolDbEntities.Entry<Login_Master>(list[0]).State = EntityState.Modified;
            ticketingToolDbEntities.SaveChanges();
          }
        }
        catch
        {
          return (ActionResult) new HttpStatusCodeResult(404);
        }
      }
      return (ActionResult) this.Redirect("/Admin/ListUsers");
    }

    public ActionResult LogOut()
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin"))
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      this.Session.RemoveAll();
      return (ActionResult) this.Redirect("/Admin/AdminLogin");
    }
  }
}
