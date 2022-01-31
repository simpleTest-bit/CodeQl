using Microsoft.CSharp.RuntimeBinder;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Ticketing_Dashboard.Models;
using Ticketing_Dashboard.Utility;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Ticketing_Dashboard.Controllers
{
    public class OnboardingController : Controller
    {
        TicketingToolDBEntities db = new TicketingToolDBEntities();


        [HttpGet]
        public ActionResult ExportExcel(string type)
        {
            if (this.Session["UserRole"] == null)
                return (ActionResult)this.Redirect("/Admin/AdminLogin");
            if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "laptopadmin"))
                return (ActionResult)this.Redirect("/Admin/AdminLogin");

            TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
            List<Onboarding_Master> onboardingMasterList = new List<Onboarding_Master>();
            List<Onboarding_Master> collection = new List<Onboarding_Master>();
            CultureInfo cult = new CultureInfo("hi-IN");

            if (type == "all")
                collection = ticketingToolDbEntities.Onboarding_Master.Where(x=> x.isDeleted == "No").ToList();
            else if (type == "upcoming")
            {
                var upcoming = new List<Onboarding_Master>();
                foreach (var entry in ticketingToolDbEntities.Onboarding_Master.Where(x => x.isDeleted == "No").ToList())
                {
                    if (entry.actualJoiningDate != null)
                    {
                        var jDate = Convert.ToDateTime(entry.actualJoiningDate, cult);
                        var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                        if ((diff >= 0) && (diff <= 7))
                        {
                            upcoming.Add(entry);
                        }
                    }
                    else
                    {
                        var jDate = Convert.ToDateTime(entry.expectedJoiningDate, cult);
                        var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                        if ((diff >= 0) && (diff <= 7))
                        {
                            upcoming.Add(entry);
                        }
                    }
                }
                collection = upcoming;
            }
            else if (type == "fulfilled")
            {
                collection = ticketingToolDbEntities.Onboarding_Master.Where(x=> x.isDeleted == "No" && x.status == 13).ToList();
            }
            else if (type == "RENEGE")
            {
                collection = ticketingToolDbEntities.Onboarding_Master.Where(x => x.isDeleted == "No" && x.status == 2).ToList();
            }


            ExcelPackage excel = ExportToExcel.ExportOnboardingData(collection);
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "attachment: filename=LaptopDetails.xlsx");
            this.Response.BinaryWrite(excel.GetAsByteArray());
            this.Response.End();
            return (ActionResult)this.Redirect("/Onboarding/OnboardingDetails?type=" + type);
        }


        [HttpGet]
        public ActionResult SendPendingMail()
        {
            if(Session["UserRole"]==null)
            {
                return Redirect("/");
            }
            if ((Session["UserRole"].ToString() == "admin") || (Session["UserRole"].ToString() == "onboardingadmin"))
            {
                TicketingToolDBEntities ticketingToolDBEntities = new TicketingToolDBEntities();
                CultureInfo cult = new CultureInfo("hi-IN");
                var toMailIDs3 = new List<string>();
                var upcoming = new List<Onboarding_Master>();
                foreach (var entry in ticketingToolDBEntities.Onboarding_Master.Where(x => x.isDeleted == "No").ToList())
                {
                    if (entry.actualJoiningDate != null)
                    {
                        var jDate = Convert.ToDateTime(entry.actualJoiningDate, cult);
                        var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                        if ((diff >= 0) && (diff <= 7))
                        {
                            upcoming.Add(entry);
                        }
                    }
                    else
                    {
                        var jDate = Convert.ToDateTime(entry.expectedJoiningDate, cult);
                        var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                        if ((diff >= 0) && (diff <= 7))
                        {
                            upcoming.Add(entry);
                        }
                    }
                }
                foreach (Mail_Master mailMaster in ticketingToolDBEntities.Mail_Master.ToList<Mail_Master>())
                {
                    foreach (char ch in mailMaster.type)
                    {
                        switch (ch)
                        {
                            case 'O':
                                toMailIDs3.Add(mailMaster.email);
                                break;
                        }
                    }
                }
                Mail.SendOnboardingPendingMail(upcoming, toMailIDs3, cult);
                return Redirect("/");
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult UploadOnboardingData(HttpPostedFileBase excelFile)
        {
            if (this.HttpContext.Request.HttpMethod == "GET")
                return (ActionResult)this.View();
            if (!(this.HttpContext.Request.HttpMethod == "POST"))
                return (ActionResult)new HttpStatusCodeResult(404);
            List<string> stringList = new List<string>()
              {
                "Expected Joining Date [IST]",
                "E Band",
                "Personal Sub Area",
                "Joining L4",
                "Reporting Manager",
                "Recruiter",
                "SR Number",
                "Contact Number",
                "Auto req ID",
                "First name",
                "Last name",
                "E-mail"
              };
            List<Onboarding_Master> onboardingMasterList = new List<Onboarding_Master>();
            TicketingToolDBEntities ticketingToolDbEntities1 = new TicketingToolDBEntities();
            HttpPostedFileBase httpPostedFileBase = excelFile;
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength > 0 && !string.IsNullOrEmpty(httpPostedFileBase.FileName))
            {
                byte[] buffer = new byte[httpPostedFileBase.ContentLength];
                httpPostedFileBase.InputStream.Read(buffer, 0, Convert.ToInt32(httpPostedFileBase.ContentLength));
                using (ExcelPackage excelPackage = new ExcelPackage(httpPostedFileBase.InputStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    try
                    {
                        int count = excelPackage.Workbook.Worksheets.Count;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.message = "Excel has no worksheets";
                        ViewBag.messagetype = "danger";
                        return (ActionResult)this.View();
                    }
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.First<ExcelWorksheet>();
                    int column = excelWorksheet.Dimension.End.Column;
                    int row = excelWorksheet.Dimension.End.Row;
                    List<Onboarding_Master> list = ticketingToolDbEntities1.Onboarding_Master.ToList<Onboarding_Master>();
                    if (column != 12)
                    {
                        ViewBag.message = "Excel should have 12 columns";
                        ViewBag.messagetype = "danger";
                        return (ActionResult)this.View();
                    }
                    for (int Col = 1; Col <= 12; ++Col)
                    {
                        if (excelWorksheet.Cells[1, Col].Value.ToString().Trim() != stringList[Col - 1])
                        {
                            ViewBag.message = "Column names are invalid";
                            ViewBag.messagetype = "danger";
                            return (ActionResult)this.View();
                        }
                    }
                    int count1 = ticketingToolDbEntities1.Onboarding_Master.ToList<Onboarding_Master>().Count;
                    for (int Row = 2; Row <= row; ++Row)
                    {
                        Onboarding_Master onboardingMaster = new Onboarding_Master();
                        DateTime now = DateTime.Now;
                        DateTime dateTime;
                        try
                        {
                            dateTime = Convert.ToDateTime(excelWorksheet.Cells[Row, 1].Value, (IFormatProvider)new CultureInfo("hi-IN"));
                        }
                        catch
                        {
                            ViewBag.message = "Invalid datetime format, row: " + Row;
                            ViewBag.messagetype = "danger";
                            return (ActionResult)this.View();
                        }
                        onboardingMaster.pkID = count1;
                        onboardingMaster.expectedJoiningDate = new DateTime?(dateTime);
                        onboardingMaster.band = excelWorksheet.Cells[Row, 2].Value.ToString().Trim();
                        onboardingMaster.personalSubArea = excelWorksheet.Cells[Row, 3].Value.ToString().Trim();
                        onboardingMaster.joiningL4 = excelWorksheet.Cells[Row, 4].Value.ToString().Trim();
                        onboardingMaster.reportingManager = excelWorksheet.Cells[Row, 5].Value.ToString().Trim();
                        onboardingMaster.recruiter = excelWorksheet.Cells[Row, 6].Value.ToString().Trim();
                        onboardingMaster.srNumber = excelWorksheet.Cells[Row, 7].Value.ToString().Trim();
                        try
                        {
                            string contactNum = excelWorksheet.Cells[Row, 8].Value.ToString().Trim().Replace(" ", string.Empty);
                            if (list.Where<Onboarding_Master>((Func<Onboarding_Master, bool>)(x => x.contactNumber == contactNum)).ToList<Onboarding_Master>().Count == 0)
                            {
                                foreach (var entry in onboardingMasterList)
                                {
                                    if (entry.contactNumber == contactNum)
                                    {
                                        ViewBag.message = "Excel contains duplicate entries, row: " + Row;
                                        ViewBag.messagetype = "danger";
                                        return (ActionResult)this.View();
                                    }
                                }
                                Convert.ToInt64(contactNum);
                                onboardingMaster.contactNumber = contactNum;
                                if (onboardingMaster.contactNumber.ToString().Length != 10)
                                {
                                    ViewBag.message = "Contact number not of 10 digits, row: " + Row;
                                    ViewBag.messagetype = "danger";
                                    return (ActionResult)this.View();
                                }
                            }
                            else
                            {
                                foreach (var entry in onboardingMasterList)
                                {
                                    if (entry.contactNumber == contactNum)
                                    {
                                        ViewBag.message = "Excel contains duplicate entries, row: " + Row;
                                        ViewBag.messagetype = "danger";
                                        return (ActionResult)this.View();
                                    }
                                }
                                var onboarding = ticketingToolDbEntities1.Onboarding_Master.Where(x => x.isDeleted == "No" && x.contactNumber == contactNum).ToList()[0];
                                if (onboarding.expectedJoiningDate == onboardingMaster.expectedJoiningDate)
                                {
                                    continue;
                                }
                                else
                                {
                                    onboarding.actualJoiningDate = onboardingMaster.expectedJoiningDate;
                                }
                                ticketingToolDbEntities1.Entry(onboarding).State = System.Data.Entity.EntityState.Modified;
                                ticketingToolDbEntities1.SaveChanges();
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.message = "Invalid contact number format, row: " + Row;
                            ViewBag.messagetype = "danger";
                            return (ActionResult)this.View();
                        }
                        onboardingMaster.autoReqID = excelWorksheet.Cells[Row, 9].Value.ToString().Trim();
                        onboardingMaster.firstName = excelWorksheet.Cells[Row, 10].Value.ToString().Trim();
                        onboardingMaster.lastName = excelWorksheet.Cells[Row, 11].Value.ToString().Trim();
                        onboardingMaster.emailID = excelWorksheet.Cells[Row, 12].Value.ToString().Trim();
                        onboardingMaster.isDeleted = "No";
                        onboardingMaster.createdDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("hi-IN"));
                        onboardingMaster.status = 4;
                        onboardingMasterList.Add(onboardingMaster);
                        ++count1;
                    }

                    using (TicketingToolDBEntities db = new TicketingToolDBEntities())
                    {
                        foreach (var entry in onboardingMasterList)
                        {
                            db.Onboarding_Master.Add(entry);
                        }
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.message = "Data contains duplicate numbers";
                            ViewBag.messagetype = "danger";
                        }
                    }
                }
            }
            ViewBag.message = "Data added successfully";
            ViewBag.messagetype = "success";
            return View();
        }


    public ActionResult SetRENEGEStatus(string id, DateTime RENEGEUpdatedDate, string RENEGERemarks)
    {
        if(Session["UserRole"] == null)
        {
                return Redirect("/Admin/AdminLogin");
        }
        if ((Session["UserRole"].ToString() == "admin") || Session["UserRole"].ToString() == "onboardingadmin")
        {
                try
                {
                    int pkID = Convert.ToInt32(id);
                    var onboarding = db.Onboarding_Master.Where(x => x.isDeleted == "No" && x.pkID == pkID).ToList();
                    if (onboarding.Count == 0)
                    {
                        return new HttpStatusCodeResult(404);
                    }
                    else
                    {
                        var req = onboarding[0];
                        req.prevStatus= req.status;
                        req.status = 2;
                        req.RENEGEUpdatedDate = RENEGEUpdatedDate;
                        req.RENEGERemarks = RENEGERemarks;
                        db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Redirect("/Onboarding/OnboardingDetail/" + id);
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
    }

    public ActionResult UpdateStatus(string id, int orderID=0, DateTime? laptopRequestUpdatedDate=null, string laptopRequestUpdateRemarks=null, int sapID=0, DateTime? sapIDUpdatedDate=null, string sapIDRemarks=null, DateTime? gitUpdatedDate=null, DateTime? laptopDispatchDate=null, DateTime? laptopCollectionDate=null)
    {
        if (Session["UserRole"] == null)
        {
                return Redirect("/Admin/AdminLogin");
        }
        if ((Session["UserRole"].ToString() != "admin") && (Session["UserRole"].ToString() != "onboardingadmin"))
        {
                return Redirect("/Admin/AdminLogin");
        }
            try
            {
                int pkID = Convert.ToInt32(id);
                var onboarding = db.Onboarding_Master.Where(x => x.isDeleted == "No" && x.pkID == pkID).ToList();
                if (onboarding.Count == 0)
                {
                    return new HttpStatusCodeResult(404);
                }
                else
                {
                    var req = onboarding[0];
                    if (req.status == 4)
                    {
                        if ((orderID != 0) || (laptopRequestUpdatedDate != null) || (laptopRequestUpdateRemarks != null))
                        {
                            req.orderID = orderID;
                            req.laptopRequestUpdatedDate = laptopRequestUpdatedDate;
                            req.laptopRequestUpdateRemarks = laptopRequestUpdateRemarks;
                            req.status = 6;
                            db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = "Please fill all fields";
                            TempData["MessageType"] = "warning";
                        }
                    }
                    else if (req.status == 6)
                    {
                        if ((sapID != 0) || (sapIDUpdatedDate != null) || (sapIDRemarks != null))
                        {
                            req.sapID = sapID;
                            req.sapIDUpdatedDate = sapIDUpdatedDate;
                            req.sapIDRemarks = sapIDRemarks;
                            req.status = 8;
                            db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = "Please fill all fields";
                            TempData["MessageType"] = "warning";
                            
                        }
                    }
                    else if (req.status == 8)
                    {
                        if (gitUpdatedDate != null)
                        {
                            req.gitUpdatedDate = gitUpdatedDate;
                            req.status = 10;
                            db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = "Please fill all fields";
                            TempData["MessageType"] = "warning";
                        }
                    }
                    else if (req.status == 10)
                    {
                        if (laptopDispatchDate != null)
                        {
                            req.laptopDispatchDate = laptopDispatchDate;
                            req.status = 12;
                            db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = "Please fill all fields";
                            TempData["MessageType"] = "warning";
                        }
                    }
                    else if (req.status == 12)
                    {
                        if (laptopCollectionDate != null)
                        {
                            req.laptopCollectionDate = laptopCollectionDate;
                            req.status = 13;
                            db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = "Please fill all fields";
                            TempData["MessageType"] = "warning";
                        }
                    }
                }
            }
            catch
            {
                return new HttpStatusCodeResult(404);
            }
            TempData["Message"] = "Status Updated Successfully";
            TempData["MessageType"] = "success";
            TempData.Keep("Message");
            TempData.Keep("MessageType");
            return Redirect("/Onboarding/OnboardingDetail/" + id);
    }

    public ActionResult DownloadSampleExcelFile()
    {
      FilePathResult filePathResult = new FilePathResult("~/App_Data/Sample.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
      filePathResult.FileDownloadName = "Sample.xlsx";
      return (ActionResult) filePathResult;
    }

    [HttpGet]
    public ActionResult OnboardingDetails(string type, DateTime? fromDate=null, DateTime? toDate=null, string filterBy=null)
    {
          if (this.Session["UserRole"] == null)
            return (ActionResult) this.Redirect("/Admin/AdminLogin");
          if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "onboardingadmin"))
            return (ActionResult) this.Redirect("/Admin/AdminLogin");
            TicketingToolDBEntities db = new TicketingToolDBEntities();
            List<Onboarding_Master> list = db.Onboarding_Master.Where(x => x.isDeleted == "No").ToList();
            ViewBag.requestType = type;
            ViewBag.fromDate = DateTime.Now.AddDays(-30);
            ViewBag.toDate = DateTime.Now;
            if (type == "all")
            {
                var output = new List<Onboarding_Master>();
                CultureInfo cult = new CultureInfo("hi-IN");
                if ((fromDate == null) && (toDate == null) && (filterBy == null))
                {
                    output = list.Where(x => x.createdDate >= Convert.ToDateTime(DateTime.Now.AddDays(-30), cult)).ToList();
                }
                else if ((fromDate != null) && (toDate != null) && (filterBy != null))
                {
                    if (filterBy == "createdDate")
                    {
                        output = list.Where(x => x.createdDate >= Convert.ToDateTime(fromDate, cult) && x.createdDate <= Convert.ToDateTime(toDate, cult)).ToList();
                    }
                    else if (filterBy == "joiningDate")
                    {
                        var newList = list.ToList();
                        foreach (var entry in newList)
                        {
                            if ((entry.actualJoiningDate != null) && (entry.actualJoiningDate <= Convert.ToDateTime(toDate, cult)) && (entry.actualJoiningDate >= Convert.ToDateTime(fromDate, cult)))
                            {
                                output.Add(entry);
                            }
                            else if ((entry.expectedJoiningDate != null) && (entry.expectedJoiningDate <= Convert.ToDateTime(toDate, cult)) && (entry.expectedJoiningDate >= Convert.ToDateTime(fromDate, cult)))
                            {
                                output.Add(entry);
                            }
                        }
                    }
                    ViewBag.fromDate = Convert.ToDateTime(fromDate, cult);
                    ViewBag.toDate = Convert.ToDateTime(toDate, cult);
                    ViewBag.filterBy = filterBy;
                }
                else
                {
                    return new HttpStatusCodeResult(404);
                }
                ViewBag.onboarding = output;
            }
          else if (type == "upcoming")
            {
                List<Onboarding_Master> list2 = new List<Onboarding_Master>();
                foreach (var entry in list)
                {
                    if (entry.actualJoiningDate == null)
                    {
                        var diff = entry.expectedJoiningDate.Value - DateTime.Now;
                        if ((diff.Days <= 7) && (diff.Days >= 0))
                        {
                            list2.Add(entry);
                        }
                    }
                    else
                    {
                        var diff = entry.actualJoiningDate.Value - DateTime.Now;
                        if ((diff.Days <= 7) && (diff.Days >= 0))
                        {
                            list2.Add(entry);
                        }
                    }
                }
                ViewBag.onboarding = list2;
            }
          else if (type == "fulfilled")
            {
                ViewBag.onboarding = list.Where(x => x.status == 13).ToList();
            }
            else if (type == "RENEGE")
            {
                ViewBag.onboarding = list.Where(x => x.status == 2).ToList();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
          return (ActionResult) this.View();
    }

    public ActionResult OnboardingDetail(string id)
    {
      if (this.Session["UserRole"] == null)
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      if (!(this.Session["UserRole"].ToString() == "admin") && !(this.Session["UserRole"].ToString() == "onboardingadmin"))
        return (ActionResult) this.Redirect("/Admin/AdminLogin");
      try
      {
        TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
        int pkID = Convert.ToInt32(id);
        List<Onboarding_Master> list = ticketingToolDbEntities.Onboarding_Master.Where<Onboarding_Master>((Expression<Func<Onboarding_Master, bool>>) (x => x.isDeleted == "No" && x.pkID == pkID)).ToList<Onboarding_Master>();
        if (list.Count != 1)
          return (ActionResult) new HttpStatusCodeResult(404);
        ViewBag.requestObject = list[0];
        return (ActionResult) this.View();
      }
      catch
      {
        return (ActionResult) new HttpStatusCodeResult(404);
      }
    }
  }
}
