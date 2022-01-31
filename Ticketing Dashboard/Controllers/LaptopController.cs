// Decompiled with JetBrains decompiler
// Type: Ticketing_Dashboard.Controllers.LaptopController
// Assembly: Ticketing Dashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E2BA73D6-734D-47BB-8E89-378BFC2CE176
// Assembly location: C:\Users\PenTester07\wwwroot\bin\Ticketing Dashboard.dll

using Microsoft.CSharp.RuntimeBinder;
using OfficeOpenXml;
using System;
using System.Data;
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
    public class LaptopController : Controller
    {
        private CultureInfo cult = new CultureInfo("hi-IN");
        TicketingToolDBEntities db = new TicketingToolDBEntities();

        public ActionResult LaptopRequest()
        {
            if (HttpContext.Request.HttpMethod == "GET")
            {
                ViewBag.LocationList = db.Location_Master.ToList();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRequest(string id, string remarks, DateTime laptopUpdatedDate)
        {
            if (Session["UserRole"] == null)
            {
                return Redirect("/Admin/AdminLogin");
            }
            if ((Session["UserRole"].ToString() == "admin") || (Session["UserRole"]).ToString() == "laptopadmin")
            {
                try
                {
                    int pkID = Convert.ToInt32(id); 
                    var laptop = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.requestType == "Break Fix" && x.srPk == pkID).ToList();
                    if (laptop.Count == 0)
                    {
                        return new HttpStatusCodeResult(404);
                    }
                    else
                    {
                        laptop[0].isLaptopRequired = true;
                        laptop[0].remarks = remarks;
                        laptop[0].laptopUpdatedDate = Convert.ToDateTime(laptopUpdatedDate, new CultureInfo("hi-IN"));
                        laptop[0].slaDate = Convert.ToDateTime(laptopUpdatedDate.AddDays(14), new CultureInfo("hi-IN"));
                        db.Entry(laptop[0]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(404);
                }
            }
            return Redirect("/Laptop/LaptopDetail/" + id);
        }

        [HttpGet]
        public ActionResult UserLaptopDetail(string token, int id)
        {
            try
            {
                var laptops = db.Laptop_Master.Where(x => x.isDeleted == "No").ToList();
                foreach (var laptop in laptops)
                {
                    if (CryptographyManager.VerifySha265HashWithSalt(laptop.requesterEmailID, token) == true)
                    {
                        ViewBag.requestObject = laptop;
                        return View();
                    }
                }
                return (ActionResult)new HttpStatusCodeResult(404);
            }
            catch (Exception ex)
            {
                return (ActionResult)new HttpStatusCodeResult(404);
            }
        }

        [HttpGet]
        public ActionResult LaptopDetails(string type, DateTime? fromDate = null, DateTime? toDate = null)
        {
            ViewBag.requestType = type;
            ViewBag.fromDate = DateTime.Now.AddDays(-30);
            ViewBag.toDate = DateTime.Now;
            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "laptopadmin"))
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (type == "all")
            {
                var list = db.Laptop_Master.Where(x=> x.isDeleted == "No").ToList();
                var output = new List<Laptop_Master>();
                CultureInfo cult = new CultureInfo("hi-IN");
                if ((fromDate == null) && (toDate == null))
                {
                    output = list.Where(x => x.createdDate >= Convert.ToDateTime(DateTime.Now.AddDays(-30), cult)).ToList();
                }
                else if ((fromDate != null) && (toDate != null))
                {
                    output = list.Where(x => x.createdDate >= Convert.ToDateTime(fromDate, cult) && x.createdDate <= Convert.ToDateTime(toDate, cult)).ToList();
                    ViewBag.fromDate = Convert.ToDateTime(fromDate, cult);
                    ViewBag.toDate = Convert.ToDateTime(toDate, cult);
                }
                else
                {
                    return new HttpStatusCodeResult(404);
                }
                ViewBag.requestDetails = output;
            }
            else if (type == "pending")
            {
                ViewBag.requestDetails = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.status == "Pending").ToList();
            }
            else if (type == "sla-breakfix")
            {
                ViewBag.requestDetails = SLAManager.getSLABreakFixRequests();
            }
            else if(type == "sla-employeemovingonshore")
            {
                ViewBag.requestDetails = SLAManager.getSLAEmployeeMovingOnshoreRequests();
            }
            else if (type == "closed")
            {
                ViewBag.requestDetails = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.status == "Closed").ToList();
            }
            return View();
        }

        public ActionResult LaptopDetail(string id)
        {
            if (Session["UserRole"] == null)
            {
                return Redirect("/Admin/AdminLogin");
            }
            if ((Session["UserRole"].ToString() == "admin") || (Session["UserRole"].ToString() == "laptopadmin"))
            {
                try
                {
                    int pkID = Convert.ToInt32(id);
                    var laptop = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.srPk == pkID).ToList();
                    if (laptop.Count == 0)
                    {
                        return new HttpStatusCodeResult(404);
                    }
                    else
                    {
                        ViewBag.requestObject = laptop[0];
                        return View();
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(404);
                }
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CloseLaptopRequest(
      string id,
      string actionTaken,
      string dateOfClosing)
    {
      if (this.Session["UserRole"] == null)
            {
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            }
      if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "laptopadmin"))
            {
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            }
      try
      {
        int srpk_id = Convert.ToInt32(id);
        TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
        List<Laptop_Master> list1 = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.srPk == srpk_id)).ToList<Laptop_Master>();
        try
        {
          if (list1.Count == 0 || list1 == null)
          {
            this.TempData["DetailMessage"] = (object) "Invalid ID. Please Try Again";
            this.TempData["DetailMessageType"] = (object) "warning";
            this.TempData.Keep("DetailMessage");
            this.TempData.Keep("DetailMessageType");
            return (ActionResult) this.View("Error");
          }
          if (list1[0].status == "Closed")
          {
            this.TempData["DetailMessage"] = (object) "Request Already Closed";
            this.TempData["DetailMessageType"] = (object) "warning";
            this.TempData.Keep("DetailMessage");
            this.TempData.Keep("DetailMessageType");
            return (ActionResult) this.View("Error");
          }
          list1[0].status = "Closed";
          list1[0].actionTaken = actionTaken;
          list1[0].dateOfClosing = new DateTime?(Convert.ToDateTime(dateOfClosing, (IFormatProvider) this.cult));
          ticketingToolDbEntities.Entry<Laptop_Master>(list1[0]).State = EntityState.Modified;
          ticketingToolDbEntities.SaveChanges();
          this.TempData["DetailMessage"] = (object) "Request Closed Successfully";
          this.TempData["DetailMessageType"] = (object) "success";
          this.TempData.Keep("DetailMessage");
          this.TempData.Keep("DetailMessageType");
          List<Mail_Master> list2 = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
          List<string> ccMailIDs = new List<string>();
          foreach (Mail_Master mailMaster in list2)
          {
            if (mailMaster.type.Contains<char>('L'))
              ccMailIDs.Add(mailMaster.email);
          }
          Mail.SendLaptopCloseMail(list1[0], list1[0].requesterEmailID, ccMailIDs, this.cult);
        }
        catch (Exception ex)
        {
          this.TempData["DetailMessage"] = (object) "Unable to Save Data. Please Try Again";
          this.TempData["DetailMessageType"] = (object) "warning";
          this.TempData.Keep("DetailMessage");
          this.TempData.Keep("DetailMessageType");
          return View();
        }
      }
      catch (Exception ex)
      {
         return View();
      }
      return (ActionResult) this.Redirect("/Laptop/LaptopDetail/" + id);
    }

    public ActionResult PendingRequests()
    {
      try
      {
        if (this.Session["UserRole"] == null)
          return (ActionResult) this.Redirect("/Admin/AdminLogin");
        if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "laptopadmin"))
          return (ActionResult) this.Redirect("/Admin/AdminLogin");
        TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
        List<Mail_Master> list = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
        List<string> toMailIDs = new List<string>();
        foreach (Mail_Master mailMaster in list)
        {
          if (mailMaster.type.Contains<char>('L'))
            toMailIDs.Add(mailMaster.email);
        }
        Mail.SendLaptopPendingMail(ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.isDeleted == "No" && x.status == "Pending")).ToList<Laptop_Master>(), toMailIDs, this.cult);
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

    public ActionResult ExportExcel(string type)
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "laptopadmin"))
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
      List<Laptop_Master> laptopMasterList = new List<Laptop_Master>();
      List<Laptop_Master> collection;
      if (type == "all")
        collection = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.isDeleted == "No")).ToList<Laptop_Master>();
      else if (type == "pending")
        collection = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.isDeleted == "No" && x.status == "Pending")).ToList<Laptop_Master>();
      else if (type == "closed")
        collection = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.isDeleted == "No" && x.status == "Closed")).ToList<Laptop_Master>();
      else if (type == "sla-breakfix")
        collection = SLAManager.getSLABreakFixRequests();
      else if (type == "sla-employeemovingonshore")
        collection = SLAManager.getSLAEmployeeMovingOnshoreRequests();
      else
        collection = ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>) (x => x.isDeleted == "No" && x.status == default (string))).ToList<Laptop_Master>();
      ExcelPackage excel = ExportToExcel.ExportLaptopDataToExcel(collection);
      this.Response.Clear();
      this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      this.Response.AddHeader("content-disposition", "attachment: filename=LaptopDetails.xlsx");
      this.Response.BinaryWrite(excel.GetAsByteArray());
      this.Response.End();
      return (ActionResult) this.Redirect("/Laptop/LaptopDetails?type=" + type);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddLaptopRequest(
      Laptop_Master data,
      string otherLocation,
      string otherFromLocation,
      string otherToLocation)
    {
      TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
      Laptop_Master laptopMaster = new Laptop_Master();
            CultureInfo cult = new CultureInfo("hi-IN");
      try
      {
        laptopMaster.sapid = new int?(Convert.ToInt32((object) data.sapid));
      }
      catch
      {
        this.TempData["Message"] = (object) "Invalid SAP ID";
        this.TempData["MessageType"] = (object) "warning";
        this.TempData.Keep("Message");
        this.TempData.Keep("MessageType");
      }
      if (data.requestType == "Break Fix")
      {
        laptopMaster.requesterEmailID = data.requesterEmailID;
        laptopMaster.sapid = data.sapid;
        laptopMaster.issueDetails = data.issueDetails;
        laptopMaster.requestType = data.requestType;
        laptopMaster.name = data.name;
        laptopMaster.replacementType = data.replacementType;
        laptopMaster.slaDate = Convert.ToDateTime(DateTime.Now.AddDays(2), cult);
        laptopMaster.requesterLocation = !(data.requesterLocation == "Other") ? data.requesterLocation : Location.SetLocation(otherLocation);
        laptopMaster.dateOfTravel = new DateTime?();
        laptopMaster.laptopType = (string) null;
        laptopMaster.fromLocation = (string) null;
        laptopMaster.toLocation = (string) null;
      }
      else if (data.requestType == "Moving To Onshore / Offshore")
      {
        laptopMaster.name = data.name;
        laptopMaster.sapid = data.sapid;
        laptopMaster.requesterEmailID = data.requesterEmailID;
        laptopMaster.dateOfTravel = new DateTime?(Convert.ToDateTime((object) data.dateOfTravel, (IFormatProvider) this.cult));
        laptopMaster.laptopType = data.laptopType;
        laptopMaster.requestType = data.requestType;
        laptopMaster.fromLocation = data.fromLocation == "Other" ? otherFromLocation : Location.SetLocation(otherFromLocation);
        laptopMaster.toLocation = data.toLocation == "Other" ? otherToLocation : Location.SetLocation(otherToLocation);
                laptopMaster.slaDate = Convert.ToDateTime(DateTime.Now.AddDays(15), cult);
                laptopMaster.issueDetails = (string) null;
        laptopMaster.requesterLocation = (string) null;
        laptopMaster.replacementType = (string) null;
      }
      laptopMaster.isLaptopRequired = false;
      laptopMaster.replacementType = data.replacementType;
      laptopMaster.actionTaken = (string) null;
      laptopMaster.dateOfClosing = new DateTime?();
      laptopMaster.status = "Pending";
      laptopMaster.isDeleted = "No";
      laptopMaster.createdDate = new DateTime?(Convert.ToDateTime((object) DateTime.Now, (IFormatProvider) this.cult));
      ticketingToolDbEntities.Laptop_Master.Add(laptopMaster);
      ticketingToolDbEntities.SaveChanges();
      this.TempData["Message"] = (object) "Request Created Successfully";
      this.TempData["MessageType"] = (object) "success";
      this.TempData.Keep("Message");
      this.TempData.Keep("MessageType");
      List<Mail_Master> list = ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>();
      List<string> ccMailIDs = new List<string>();
      foreach (Mail_Master mailMaster in list)
      {
        if (mailMaster.type.Contains<char>('L'))
          ccMailIDs.Add(mailMaster.email);
      }
      Mail.SendLaptopMail(laptopMaster, laptopMaster.requesterEmailID, ccMailIDs, this.cult);
      return (ActionResult) this.Redirect("/Laptop/LaptopRequest");
    }
  }
}
