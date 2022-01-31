// Decompiled with JetBrains decompiler
// Type: Ticketing_Dashboard.Controllers.HomeController
// Assembly: Ticketing Dashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E2BA73D6-734D-47BB-8E89-378BFC2CE176
// Assembly location: C:\Users\PenTester07\wwwroot\bin\Ticketing Dashboard.dll

using Microsoft.CSharp.RuntimeBinder;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Ticketing_Dashboard.Models;
using Ticketing_Dashboard.Utility;

namespace Ticketing_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private CultureInfo cult = new CultureInfo("hi-IN");
        TicketingToolDBEntities db = new TicketingToolDBEntities();

        public ActionResult Index()
        {
            if (Scheduler.CheckSchedulerStatus())
                Scheduler.Main();
            return (ActionResult)this.View();
        }

        public ActionResult SendTestMail()
        {
            Mail.TestMail();
            return Redirect("/");
        }

        public ActionResult SR_Request()
        {
            if(HttpContext.Request.HttpMethod == "GET")
            {
                var location = db.Location_Master.ToList();
                var band = db.Band_Master.ToList();
                ViewBag.LocationList = location;
                ViewBag.BandList = band;
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(405);
            }
        }

        public ActionResult SRDetails(string type, DateTime? fromDate = null, DateTime? toDate = null)
        {
            ViewBag.requestType = type;
            ViewBag.fromDate = DateTime.Now.AddDays(-30);
            ViewBag.toDate = DateTime.Now;

            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin"))
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (type == "all")
            {
                var list = db.SR_Request.Where(x => x.isDeleted == false).ToList();
                var output = new List<SR_Request>();
                CultureInfo cult = new CultureInfo("hi-IN");
                if ((fromDate == null) && (toDate == null))
                {
                    output = list.Where(x => x.createdDate >= Convert.ToDateTime(DateTime.Now.AddDays(-30), cult)).ToList();
                }
                else if ((fromDate != null) && (toDate != null))
                {
                    output = list.Where(x => x.createdDate >= Convert.ToDateTime(fromDate, cult) && x.createdDate <= Convert.ToDateTime(toDate, cult)).ToList();
                }
                else
                {
                    return new HttpStatusCodeResult(404);
                }
                ViewBag.requestDetails = output;
                ViewBag.fromDate = Convert.ToDateTime(fromDate, cult);
                ViewBag.toDate = Convert.ToDateTime(toDate, cult);
            }
            else if (type == "pending")
            {
                ViewBag.requestDetails = db.SR_Request.Where(x => x.isDeleted == false && x.status == "Pending").ToList();
            }
            else if (type == "sla")
            {
                ViewBag.requestDetails = SLAManager.getSLASRRequests();
            }
            else if (type == "closed")
            {
                ViewBag.requestDetails = db.SR_Request.Where(x => x.isDeleted == false && x.status == "Closed").ToList();
            }
            return View();
        }

        [HttpPost]
        public ActionResult CloseSRRequest(
          string id,
          string remarks,
          string srNo,
          string closingDate)
        {
            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin"))
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            try
            {
                int srpk_id = Convert.ToInt32(id);
                TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
                List<Ticketing_Dashboard.Models.SR_Request> list1 = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>)(x => x.srPk == srpk_id)).ToList<Ticketing_Dashboard.Models.SR_Request>();
                try
                {
                    if (list1.Count == 0 || list1 == null)
                    {
                        this.TempData["DetailMessage"] = (object)"Invalid ID. Please Try Again";
                        this.TempData["DetailMessageType"] = (object)"warning";
                        this.TempData.Keep("DetailMessage");
                        this.TempData.Keep("DetailMessageType");
                        return (ActionResult)this.View("Error");
                    }
                    if (list1[0].status == "Closed")
                    {
                        this.TempData["DetailMessage"] = (object)"Request Already Closed";
                        this.TempData["DetailMessageType"] = (object)"warning";
                        this.TempData.Keep("DetailMessage");
                        this.TempData.Keep("DetailMessageType");
                        return (ActionResult)this.View("Error");
                    }
                    list1[0].status = "Closed";
                    list1[0].remarks = remarks;
                    list1[0].srNo = srNo;
                    list1[0].closingDate = new DateTime?(Convert.ToDateTime(closingDate, (IFormatProvider)this.cult));
                    ticketingToolDbEntities.Entry<Ticketing_Dashboard.Models.SR_Request>(list1[0]).State = EntityState.Modified;
                    ticketingToolDbEntities.SaveChanges();
                    this.TempData["DetailMessage"] = (object)"Request Closed Successfully";
                    this.TempData["DetailMessageType"] = (object)"success";
                    this.TempData.Keep("DetailMessage");
                    this.TempData.Keep("DetailMessageType");
                    List<Mail_Master> list2 = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
                    List<string> ccMailIDs = new List<string>();
                    foreach (Mail_Master mailMaster in list2)
                    {
                        if (mailMaster.type.Contains<char>('S'))
                            ccMailIDs.Add(mailMaster.email);
                    }
                    Mail.SendCloseMail(list1[0], list1[0].requesterEmailID, ccMailIDs, this.cult);
                }
                catch (Exception ex)
                {
                    this.TempData["DetailMessage"] = (object)"Unable to Save Data. Please Try Again";
                    this.TempData["DetailMessageType"] = (object)"warning";
                    this.TempData.Keep("DetailMessage");
                    this.TempData.Keep("DetailMessageType");
                    return (ActionResult)this.View("Error");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)this.View("Error");
            }
      return (ActionResult) this.Redirect("/Home/SRDetail/" + id);
    }
    
    
    public ActionResult SRDetail(string id)
    {
        if (Session["UserRole"] == null)
        {
                return Redirect("/Admin/AdminLogin");
        }
        if ((Session["UserRole"].ToString() == "admin") || (Session["UserRole"].ToString() == "sradmin"))
        {
                try
                {
                    int pkID = Convert.ToInt32(id);
                    var sr = db.SR_Request.Where(x => x.isDeleted == false && x.srPk == pkID).ToList();
                    if (sr.Count == 0)
                    {
                        return new HttpStatusCodeResult(404);
                    }
                    else
                    {
                        ViewBag.requestObject = sr[0];
                        return View();
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(404);
                }
        }
            return new HttpStatusCodeResult(403);
    }

    public ActionResult UserSRDetail(string token, int id)
    {
      try
      {
                var sr = db.SR_Request.Where(x => x.isDeleted == false).ToList();
                foreach( var s in sr)
                {
                    if (CryptographyManager.VerifySha265HashWithSalt(s.requesterEmailID, token) && s.srPk == id)
                    {
                        ViewBag.requestObject = sr;
                        return View();
                    }
                }
        return (ActionResult) new HttpStatusCodeResult(404);
      }
      catch (Exception ex)
      {
        return (ActionResult) new HttpStatusCodeResult(404);
      }
    }

    public ActionResult ExportExcel(string type)
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin"))
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
      List<Ticketing_Dashboard.Models.SR_Request> srRequestList = new List<Ticketing_Dashboard.Models.SR_Request>();
      List<Ticketing_Dashboard.Models.SR_Request> collection;
      if (type == "all")
        collection = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>) (x => x.isDeleted == (bool?) false)).ToList<Ticketing_Dashboard.Models.SR_Request>();
      else if (type == "pending")
        collection = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>) (x => x.isDeleted == (bool?) false && x.status == "Pending")).ToList<Ticketing_Dashboard.Models.SR_Request>();
      else if (type == "closed")
        collection = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>) (x => x.isDeleted == (bool?) false && x.status == "Closed")).ToList<Ticketing_Dashboard.Models.SR_Request>();
      else if (type == "sla")
        collection = SLAManager.getSLASRRequests();
      else
        collection = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>) (x => x.isDeleted == (bool?) false && x.status == default (string))).ToList<Ticketing_Dashboard.Models.SR_Request>();
      ExcelPackage excel = ExportToExcel.ExportSRDataToExcel(collection);
      this.Response.Clear();
      this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      this.Response.AddHeader("content-disposition", "attachment: filename=SRDetails.xlsx");
      this.Response.BinaryWrite(excel.GetAsByteArray());
      this.Response.End();
      return (ActionResult) this.Redirect("/Home/SRDetails?type=" + type);
    }

    public ActionResult PendingRequests()
    {
      try
      {
        if (this.Session["UserRole"] == null)
          return (ActionResult) this.Redirect("/Admin/AdminLogin");
        if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "sradmin"))
          return (ActionResult) this.Redirect("/Admin/AdminLogin");
        TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
        string str = Convert.ToString((object) Convert.ToDateTime((object) DateTime.Now, (IFormatProvider) this.cult).DayOfWeek);
        DateTime yesterDay = Convert.ToDateTime((object) DateTime.Now.AddDays((double) (!(str == "Saturday") ? (!(str == "Sunday") ? -1 : -3) : -2)), (IFormatProvider) this.cult);
        List<Ticketing_Dashboard.Models.SR_Request> list1 = ticketingToolDbEntities.SR_Request.Where<Ticketing_Dashboard.Models.SR_Request>((Expression<Func<Ticketing_Dashboard.Models.SR_Request, bool>>) (x => x.isDeleted == (bool?) false && x.status == "Pending" && x.createdDate < (DateTime?) yesterDay)).ToList<Ticketing_Dashboard.Models.SR_Request>();
        List<Mail_Master> list2 = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
        List<string> toMailIDs = new List<string>();
        foreach (Mail_Master mailMaster in list2)
        {
          if (mailMaster.type.Contains<char>('S'))
            toMailIDs.Add(mailMaster.email);
        }
        Mail.SendPendingMail(list1, toMailIDs, this.cult);
        this.TempData["Message"] = (object) "Email Sent Successfully";
        this.TempData["MessageType"] = (object) "success";
        this.TempData.Keep("Message");
        this.TempData.Keep("MessageType");
      }
      catch (Exception ex)
      {
      }
      return (ActionResult) this.Redirect("/");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddSRRequest(Ticketing_Dashboard.Models.SR_Request data, string otherLocation)
    {
      try
      {
        if (this.ModelState.IsValid)
        {
          TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
          Ticketing_Dashboard.Models.SR_Request srRequest1 = new Ticketing_Dashboard.Models.SR_Request()
          {
            requisitionSource = data.requisitionSource,
            name = data.name
          };
          srRequest1.sapid = !(srRequest1.requisitionSource == "New") ? data.sapid : (string) null;
          srRequest1.projectName = data.projectName;
          srRequest1.positionType = data.positionType;
          srRequest1.experience = new Decimal?(Convert.ToDecimal((object) data.experience));
          srRequest1.noOfPosition = data.noOfPosition;
          srRequest1.designation = data.designation;
          srRequest1.band = data.band;
          srRequest1.billStartDate = new DateTime?(Convert.ToDateTime((object) data.billStartDate, (IFormatProvider) this.cult).Date);
          DateTime date1 = Convert.ToDateTime((object) data.onboardingDate, (IFormatProvider) this.cult).Date;
          DateTime dateTime = Convert.ToDateTime((object) data.billStartDate, (IFormatProvider) this.cult);
          DateTime date2 = dateTime.Date;
          if (date1 < date2)
          {
            this.TempData["Message"] = (object) "Bill Start Date Cannot Be Less Than Onboarding Date";
            this.TempData["MessageType"] = (object) "warning";
            this.TempData.Keep("Message");
            this.TempData.Keep("MessageType");
            return (ActionResult) this.Redirect("/Home/SR_Request");
          }
          Ticketing_Dashboard.Models.SR_Request srRequest2 = srRequest1;
          dateTime = Convert.ToDateTime((object) data.onboardingDate, (IFormatProvider) this.cult);
          DateTime? nullable1 = new DateTime?(dateTime.Date);
          srRequest2.onboardingDate = nullable1;
          srRequest1.RMSapid = data.RMSapid;
          dateTime = Convert.ToDateTime((object) data.billingLossStartDate, (IFormatProvider) this.cult);
          DateTime date3 = dateTime.Date;
          dateTime = Convert.ToDateTime((object) data.onboardingDate, (IFormatProvider) this.cult);
          DateTime date4 = dateTime.Date;
          if (date3 < date4)
          {
            this.TempData["Message"] = (object) "Bill Loss Start Date Cannot Be Less Than Bill Start Date and Onboarding Date";
            this.TempData["MessageType"] = (object) "warning";
            this.TempData.Keep("Message");
            this.TempData.Keep("MessageType");
            return (ActionResult) this.Redirect("/Home/SR_Request");
          }
          Ticketing_Dashboard.Models.SR_Request srRequest3 = srRequest1;
          dateTime = Convert.ToDateTime((object) data.billingLossStartDate, (IFormatProvider) this.cult);
          DateTime? nullable2 = new DateTime?(dateTime.Date);
          srRequest3.billingLossStartDate = nullable2;
          srRequest1.role = data.role;
          srRequest1.employeeGroup = data.employeeGroup;
          if (srRequest1.employeeGroup == "Contract")
          {
            Ticketing_Dashboard.Models.SR_Request srRequest4 = srRequest1;
            dateTime = Convert.ToDateTime((object) data.contractFromDate, (IFormatProvider) this.cult);
            DateTime? nullable3 = new DateTime?(dateTime.Date);
            srRequest4.contractFromDate = nullable3;
            dateTime = Convert.ToDateTime((object) data.contratToDate, (IFormatProvider) this.cult);
            DateTime date5 = dateTime.Date;
            dateTime = Convert.ToDateTime((object) data.contractFromDate, (IFormatProvider) this.cult);
            DateTime date6 = dateTime.Date;
            if (date5 < date6)
            {
              this.TempData["Message"] = (object) "Contract To Date Cannot Be Less Than Contract From Date";
              this.TempData["MessageType"] = (object) "warning";
              this.TempData.Keep("Message");
              this.TempData.Keep("MessageType");
              return (ActionResult) this.Redirect("/Home/SR_Request");
            }
            Ticketing_Dashboard.Models.SR_Request srRequest5 = srRequest1;
            dateTime = Convert.ToDateTime((object) data.contratToDate, (IFormatProvider) this.cult);
            DateTime? nullable4 = new DateTime?(dateTime.Date);
            srRequest5.contratToDate = nullable4;
            srRequest1.contractType = data.contractType;
            srRequest1.buyRate = new Decimal?(Convert.ToDecimal((object) data.buyRate));
            srRequest1.sellRate = new Decimal?(Convert.ToDecimal((object) data.sellRate));
            srRequest1.isContract = "Yes";
          }
          else
          {
            srRequest1.contractFromDate = new DateTime?();
            srRequest1.contratToDate = new DateTime?();
            srRequest1.contractType = (string) null;
            srRequest1.buyRate = new Decimal?();
            srRequest1.sellRate = new Decimal?();
            srRequest1.isContract = "No";
          }
          srRequest1.slaDate = Convert.ToDateTime(DateTime.Now.AddDays(1), new CultureInfo("hi-IN"));
          srRequest1.skillSet = data.skillSet;
          srRequest1.primarySkill = data.primarySkill;
          srRequest1.secondarySkill = data.secondarySkill;
          srRequest1.interviewerSapid1 = data.interviewerSapid1;
          srRequest1.interviewerSapid2 = data.interviewerSapid2;
          srRequest1.createdDate = new DateTime?(Convert.ToDateTime((object) DateTime.Now, (IFormatProvider) this.cult));
          srRequest1.status = "Pending";
          srRequest1.isDeleted = new bool?(false);
          srRequest1.RPFUpdated = data.RPFUpdated;
          srRequest1.requesterEmailID = data.requesterEmailID;
          srRequest1.location = !(data.location == "Other") ? data.location : Location.SetLocation(otherLocation);
          ticketingToolDbEntities.SR_Request.Add(srRequest1);
          ticketingToolDbEntities.SaveChanges();
          this.TempData["Message"] = (object) "Request Submitted Successfully";
          this.TempData["MessageType"] = (object) "success";
          this.TempData.Keep("Message");
          this.TempData.Keep("MessageType");
          List<Mail_Master> list = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
          List<string> ccMailIDs = new List<string>();
          foreach (Mail_Master mailMaster in list)
          {
            if (mailMaster.type.Contains<char>('S'))
              ccMailIDs.Add(mailMaster.email);
          }
          Mail.SendMail(srRequest1, srRequest1.requesterEmailID, ccMailIDs, this.cult);
        }
        else
        {
          this.TempData["Message"] = (object) "Please Fill All Required Fields";
          this.TempData["MessageType"] = (object) "warning";
          this.TempData.Keep("Message");
          this.TempData.Keep("MessageType");
        }
      }
      catch (Exception ex)
      {
        this.TempData["Message"] = (object) ex.Message;
        this.TempData["MessageType"] = (object) "error";
        this.TempData.Keep("Message");
        this.TempData.Keep("MessageType");
      }
      return (ActionResult) this.Redirect("/Home/SR_Request");
    }
  }
}
