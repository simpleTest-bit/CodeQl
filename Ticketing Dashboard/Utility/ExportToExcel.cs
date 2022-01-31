using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using Ticketing_Dashboard.Models;

namespace Ticketing_Dashboard.Utility
{
    public static class ExportToExcel
    {
        static string ComputeStatus(int status)
        {
            string output = "";
            switch(status)
            {
                case 2:
                    output = "RENEGE";
                    break;
                case 4:
                    output = "Laptop Request Pending";
                    break;
                case 6:
                    output = "SAP Code Pending";
                    break;
                case 8:
                    output = "Pending With GIT";
                    break;
                case 10:
                    output = "Pending Laptop Dispatch";
                    break;
                case 12:
                    output = "Pending Laptop Collection";
                    break;
                case 13:
                    output = "Fulfilled";
                    break;
            }
            return output;
        }

        public static ExcelPackage ExportSRDataToExcel(List<SR_Request> collection)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            CultureInfo provider = new CultureInfo("hi-IN");
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excel.Workbook.Worksheets.Add("SR Request Data");
            excelWorksheet.Cells["A1"].Value = (object)"#";
            excelWorksheet.Cells["B1"].Value = (object)"Request ID";
            excelWorksheet.Cells["C1"].Value = (object)"Requisition Source";
            excelWorksheet.Cells["D1"].Value = (object)"Resource Name";
            excelWorksheet.Cells["E1"].Value = (object)"SAP ID";
            excelWorksheet.Cells["F1"].Value = (object)"Project Name";
            excelWorksheet.Cells["G1"].Value = (object)"Position Type";
            excelWorksheet.Cells["H1"].Value = (object)"Experience";
            excelWorksheet.Cells["I1"].Value = (object)"No. of Positions";
            excelWorksheet.Cells["J1"].Value = (object)"Designation";
            excelWorksheet.Cells["K1"].Value = (object)"Band";
            excelWorksheet.Cells["L1"].Value = (object)"Bill Start Date";
            excelWorksheet.Cells["M1"].Value = (object)"Onboarding Date";
            excelWorksheet.Cells["N1"].Value = (object)"RM SAP ID";
            excelWorksheet.Cells["O1"].Value = (object)"Billing Loss Start Date";
            excelWorksheet.Cells["P1"].Value = (object)"Location";
            excelWorksheet.Cells["Q1"].Value = (object)"Role";
            excelWorksheet.Cells["R1"].Value = (object)"Employee Group";
            excelWorksheet.Cells["S1"].Value = (object)"Is Contract";
            excelWorksheet.Cells["T1"].Value = (object)"Contract From Date";
            excelWorksheet.Cells["U1"].Value = (object)"Contract To Date";
            excelWorksheet.Cells["V1"].Value = (object)"Buy Rate";
            excelWorksheet.Cells["W1"].Value = (object)"Sell Rate";
            excelWorksheet.Cells["X1"].Value = (object)"Skill Set";
            excelWorksheet.Cells["Y1"].Value = (object)"Primary Skills";
            excelWorksheet.Cells["Z1"].Value = (object)"Secondary Skills";
            excelWorksheet.Cells["AA1"].Value = (object)"Interviewer 1 SAP ID";
            excelWorksheet.Cells["AB1"].Value = (object)"Interviewer 2 SAP ID";
            excelWorksheet.Cells["AC1"].Value = (object)"Created Date";
            excelWorksheet.Cells["AD1"].Value = (object)"Status";
            excelWorksheet.Cells["AE1"].Value = (object)"Remarks";
            excelWorksheet.Cells["AF1"].Value = (object)"SRNO";
            excelWorksheet.Cells["AG1"].Value = (object)"Closing Date";
            excelWorksheet.Cells["AH1"].Value = (object)"RPF Updated";
            excelWorksheet.Cells["AI1"].Value = (object)"Requester Email ID";
            int num1 = 2;
            int num2 = 1;
            foreach (SR_Request srRequest in collection)
            {
                excelWorksheet.Cells[string.Format("A{0}", (object)num1)].Value = (object)num2;
                excelWorksheet.Cells[string.Format("B{0}", (object)num1)].Value = (object)srRequest.srPk;
                excelWorksheet.Cells[string.Format("C{0}", (object)num1)].Value = (object)srRequest.requisitionSource;
                excelWorksheet.Cells[string.Format("D{0}", (object)num1)].Value = (object)srRequest.name;
                excelWorksheet.Cells[string.Format("E{0}", (object)num1)].Value = srRequest.sapid == null ? (object)"NA" : (object)srRequest.sapid;
                excelWorksheet.Cells[string.Format("F{0}", (object)num1)].Value = (object)srRequest.projectName;
                excelWorksheet.Cells[string.Format("G{0}", (object)num1)].Value = (object)srRequest.positionType;
                excelWorksheet.Cells[string.Format("H{0}", (object)num1)].Value = (object)srRequest.experience;
                excelWorksheet.Cells[string.Format("I{0}", (object)num1)].Value = (object)srRequest.noOfPosition;
                excelWorksheet.Cells[string.Format("J{0}", (object)num1)].Value = (object)srRequest.designation;
                excelWorksheet.Cells[string.Format("K{0}", (object)num1)].Value = (object)srRequest.band;
                excelWorksheet.Cells[string.Format("L{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)srRequest.billStartDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("M{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)srRequest.onboardingDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("N{0}", (object)num1)].Value = (object)srRequest.RMSapid;
                excelWorksheet.Cells[string.Format("O{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)srRequest.billingLossStartDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("P{0}", (object)num1)].Value = (object)srRequest.location;
                excelWorksheet.Cells[string.Format("Q{0}", (object)num1)].Value = (object)srRequest.role;
                excelWorksheet.Cells[string.Format("R{0}", (object)num1)].Value = (object)srRequest.employeeGroup;
                excelWorksheet.Cells[string.Format("S{0}", (object)num1)].Value = (object)srRequest.isContract;
                excelWorksheet.Cells[string.Format("T{0}", (object)num1)].Value = !srRequest.contractFromDate.HasValue ? (object)"NA" : (object)Convert.ToDateTime((object)srRequest.contractFromDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("U{0}", (object)num1)].Value = !srRequest.contratToDate.HasValue ? (object)"NA" : (object)Convert.ToDateTime((object)srRequest.contratToDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("V{0}", (object)num1)].Value = !srRequest.buyRate.HasValue ? (object)"NA" : (object)srRequest.buyRate.ToString();
                excelWorksheet.Cells[string.Format("W{0}", (object)num1)].Value = !srRequest.sellRate.HasValue ? (object)"NA" : (object)srRequest.sellRate.ToString();
                excelWorksheet.Cells[string.Format("X{0}", (object)num1)].Value = (object)srRequest.skillSet;
                excelWorksheet.Cells[string.Format("Y{0}", (object)num1)].Value = (object)srRequest.primarySkill;
                excelWorksheet.Cells[string.Format("Z{0}", (object)num1)].Value = (object)srRequest.secondarySkill;
                excelWorksheet.Cells[string.Format("AA{0}", (object)num1)].Value = (object)srRequest.interviewerSapid1;
                excelWorksheet.Cells[string.Format("AB{0}", (object)num1)].Value = (object)srRequest.interviewerSapid2;
                excelWorksheet.Cells[string.Format("AC{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)srRequest.createdDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("AD{0}", (object)num1)].Value = (object)srRequest.status;
                excelWorksheet.Cells[string.Format("AE{0}", (object)num1)].Value = (object)srRequest.remarks;
                excelWorksheet.Cells[string.Format("AF{0}", (object)num1)].Value = (object)srRequest.srNo;
                excelWorksheet.Cells[string.Format("AG{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)srRequest.closingDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("AH{0}", (object)num1)].Value = (object)srRequest.RPFUpdated;
                excelWorksheet.Cells[string.Format("AI{0}", (object)num1)].Value = (object)srRequest.requesterEmailID;
                ++num1;
                ++num2;
            }
            excelWorksheet.Cells["A:AZ"].AutoFitColumns();
            return excel;
        }

        public static ExcelPackage ExportLaptopDataToExcel(List<Laptop_Master> collection)
        {
            ExcelPackage.LicenseContext = new LicenseContext?(LicenseContext.NonCommercial);
            CultureInfo provider = new CultureInfo("hi-IN");
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excel.Workbook.Worksheets.Add("Laptop Request Data");
            excelWorksheet.Cells["A1"].Value = (object)"#";
            excelWorksheet.Cells["B1"].Value = (object)"Request ID";
            excelWorksheet.Cells["C1"].Value = (object)"Name";
            excelWorksheet.Cells["D1"].Value = (object)"SAP ID";
            excelWorksheet.Cells["E1"].Value = (object)"Location";
            excelWorksheet.Cells["F1"].Value = (object)"From Location";
            excelWorksheet.Cells["G1"].Value = (object)"To Location";
            excelWorksheet.Cells["H1"].Value = (object)"Email ID";
            excelWorksheet.Cells["I1"].Value = (object)"Request Type";
            excelWorksheet.Cells["J1"].Value = (object)"Issue Details";
            excelWorksheet.Cells["K1"].Value = (object)"Date Of Travel";
            excelWorksheet.Cells["L1"].Value = (object)"Laptop Type";
            excelWorksheet.Cells["M1"].Value = (object)"Action Taken";
            excelWorksheet.Cells["N1"].Value = (object)"Type";
            excelWorksheet.Cells["O1"].Value = (object)"Date Of Closing";
            excelWorksheet.Cells["P1"].Value = (object)"Status";
            excelWorksheet.Cells["Q1"].Value = (object)"Created Date";
            int num1 = 2;
            int num2 = 1;
            foreach (Laptop_Master laptopMaster in collection)
            {
                excelWorksheet.Cells[string.Format("A{0}", (object)num1)].Value = (object)num2;
                excelWorksheet.Cells[string.Format("B{0}", (object)num1)].Value = (object)laptopMaster.srPk;
                excelWorksheet.Cells[string.Format("C{0}", (object)num1)].Value = laptopMaster.name == null ? (object)"NA" : (object)laptopMaster.name;
                excelWorksheet.Cells[string.Format("D{0}", (object)num1)].Value = !laptopMaster.sapid.HasValue ? (object)"NA" : (object)Convert.ToString((object)laptopMaster.sapid);
                excelWorksheet.Cells[string.Format("E{0}", (object)num1)].Value = laptopMaster.requesterLocation == null ? (object)"NA" : (object)laptopMaster.requesterLocation;
                excelWorksheet.Cells[string.Format("F{0}", (object)num1)].Value = laptopMaster.fromLocation == null ? (object)"NA" : (object)laptopMaster.fromLocation;
                excelWorksheet.Cells[string.Format("G{0}", (object)num1)].Value = laptopMaster.toLocation == null ? (object)"NA" : (object)laptopMaster.toLocation;
                excelWorksheet.Cells[string.Format("H{0}", (object)num1)].Value = laptopMaster.requesterEmailID == null ? (object)"NA" : (object)laptopMaster.requesterEmailID;
                excelWorksheet.Cells[string.Format("I{0}", (object)num1)].Value = laptopMaster.requestType == null ? (object)"NA" : (object)laptopMaster.requestType;
                excelWorksheet.Cells[string.Format("J{0}", (object)num1)].Value = laptopMaster.issueDetails == null ? (object)"NA" : (object)laptopMaster.issueDetails;
                excelWorksheet.Cells[string.Format("K{0}", (object)num1)].Value = !laptopMaster.dateOfTravel.HasValue ? (object)"NA" : (object)Convert.ToDateTime((object)laptopMaster.dateOfTravel, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("L{0}", (object)num1)].Value = laptopMaster.laptopType == null ? (object)"NA" : (object)laptopMaster.laptopType;
                excelWorksheet.Cells[string.Format("M{0}", (object)num1)].Value = laptopMaster.actionTaken == null ? (object)"NA" : (object)laptopMaster.actionTaken;
                excelWorksheet.Cells[string.Format("N{0}", (object)num1)].Value = laptopMaster.replacementType == null ? (object)"NA" : (object)laptopMaster.replacementType;
                excelWorksheet.Cells[string.Format("O{0}", (object)num1)].Value = !laptopMaster.dateOfClosing.HasValue ? (object)"NA" : (object)Convert.ToDateTime((object)laptopMaster.dateOfClosing, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("P{0}", (object)num1)].Value = (object)laptopMaster.status;
                excelWorksheet.Cells[string.Format("Q{0}", (object)num1)].Value = (object)Convert.ToDateTime((object)laptopMaster.createdDate, (IFormatProvider)provider).ToString("dd/MM/yyyy");
                ++num1;
                ++num2;
            }
            excelWorksheet.Cells["A:M"].AutoFitColumns();
            return excel;
        }

        public static ExcelPackage ExportOnboardingData(List<Onboarding_Master> collection)
        {
            ExcelPackage.LicenseContext = new LicenseContext?(LicenseContext.NonCommercial);
            CultureInfo provider = new CultureInfo("hi-IN");
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excel.Workbook.Worksheets.Add("Onboarding Request Data");
            excelWorksheet.Cells["A1"].Value = (object)"#";
            excelWorksheet.Cells["B1"].Value = (object)"Request ID";
            excelWorksheet.Cells["C1"].Value = (object)"Expected Joining Date";
            excelWorksheet.Cells["D1"].Value = (object)"Band";
            excelWorksheet.Cells["E1"].Value = (object)"Personal Sub Area";
            excelWorksheet.Cells["F1"].Value = (object)"Joining L4";
            excelWorksheet.Cells["G1"].Value = (object)"Reporting Manager";
            excelWorksheet.Cells["H1"].Value = (object)"Recuriter";
            excelWorksheet.Cells["I1"].Value = (object)"SR Number";
            excelWorksheet.Cells["J1"].Value = (object)"Contact Number";
            excelWorksheet.Cells["K1"].Value = (object)"Auto Req ID";
            excelWorksheet.Cells["L1"].Value = (object)"First Name";
            excelWorksheet.Cells["M1"].Value = (object)"Last Name";
            excelWorksheet.Cells["N1"].Value = (object)"Email ID";
            excelWorksheet.Cells["O1"].Value = (object)"Created Date";
            excelWorksheet.Cells["P1"].Value = (object)"Status";
            excelWorksheet.Cells["Q1"].Value = (object)"Actual Joining Date";

            excelWorksheet.Cells["R1"].Value = (object)"Order ID";
            excelWorksheet.Cells["S1"].Value = (object)"Laptop Request Updated Date";
            excelWorksheet.Cells["T1"].Value = (object)"Laptop Request Updated Remarks";
            excelWorksheet.Cells["U1"].Value = (object)"SAP ID";
            excelWorksheet.Cells["V1"].Value = (object)"SAP ID Updated Remarks";
            excelWorksheet.Cells["W1"].Value = (object)"SAP ID Remarks";
            excelWorksheet.Cells["X1"].Value = (object)"GIT Updated Date";
            excelWorksheet.Cells["Y1"].Value = (object)"Laptop Dispatch Date";
            excelWorksheet.Cells["Z1"].Value = (object)"Laptop Collection Date";
            excelWorksheet.Cells["AA1"].Value = (object)"Previous Status";
            excelWorksheet.Cells["AB1"].Value = (object)"RENEGE Updated Date";
            excelWorksheet.Cells["AC1"].Value = (object)"RENEGE Remarks";

            int num1 = 2;
            foreach (Onboarding_Master onboarding in collection)
            {
                excelWorksheet.Cells[string.Format("A{0}", (object)num1)].Value = (object)num1;
                excelWorksheet.Cells[string.Format("B{0}", (object)num1)].Value = (object)onboarding.pkID;
                excelWorksheet.Cells[string.Format("C{0}", (object)num1)].Value = Convert.ToDateTime(onboarding.expectedJoiningDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("D{0}", (object)num1)].Value = onboarding.band;
                excelWorksheet.Cells[string.Format("E{0}", (object)num1)].Value = onboarding.personalSubArea;
                excelWorksheet.Cells[string.Format("F{0}", (object)num1)].Value = onboarding.joiningL4;
                excelWorksheet.Cells[string.Format("G{0}", (object)num1)].Value = onboarding.reportingManager;
                excelWorksheet.Cells[string.Format("H{0}", (object)num1)].Value = onboarding.recruiter;
                excelWorksheet.Cells[string.Format("I{0}", (object)num1)].Value = onboarding.srNumber;
                excelWorksheet.Cells[string.Format("J{0}", (object)num1)].Value = onboarding.contactNumber;
                excelWorksheet.Cells[string.Format("K{0}", (object)num1)].Value = onboarding.autoReqID;
                excelWorksheet.Cells[string.Format("L{0}", (object)num1)].Value = onboarding.firstName;
                excelWorksheet.Cells[string.Format("M{0}", (object)num1)].Value = onboarding.lastName;
                excelWorksheet.Cells[string.Format("N{0}", (object)num1)].Value = onboarding.emailID;
                excelWorksheet.Cells[string.Format("O{0}", (object)num1)].Value = Convert.ToDateTime(onboarding.createdDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("P{0}", (object)num1)].Value = ComputeStatus(Convert.ToInt32(onboarding.status));
                excelWorksheet.Cells[string.Format("Q{0}", (object)num1)].Value = Convert.ToDateTime(onboarding.actualJoiningDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("R{0}", (object)num1)].Value = !onboarding.orderID.HasValue ? "NA" : Convert.ToString(onboarding.orderID);
                excelWorksheet.Cells[string.Format("S{0}", (object)num1)].Value = !onboarding.laptopRequestUpdatedDate.HasValue ? "NA" : Convert.ToDateTime(onboarding.laptopRequestUpdatedDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("T{0}", (object)num1)].Value = !(onboarding.laptopRequestUpdateRemarks == null) ? "NA" : onboarding.laptopRequestUpdateRemarks;
                excelWorksheet.Cells[string.Format("U{0}", (object)num1)].Value = !onboarding.sapID.HasValue ? "NA" : Convert.ToString(onboarding.sapID);
                excelWorksheet.Cells[string.Format("V{0}", (object)num1)].Value = !onboarding.sapIDUpdatedDate.HasValue ? "NA" : Convert.ToDateTime(onboarding.sapIDUpdatedDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("W{0}", (object)num1)].Value = !(onboarding.sapIDRemarks == null) ? "NA" : Convert.ToString(onboarding.sapIDRemarks);
                excelWorksheet.Cells[string.Format("X{0}", (object)num1)].Value = !(Convert.ToDateTime(onboarding.gitUpdatedDate, provider) == null) ? "NA" : Convert.ToDateTime(onboarding.gitUpdatedDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("Y{0}", (object)num1)].Value = !(Convert.ToDateTime(onboarding.laptopDispatchDate, provider) == null) ? "NA" : Convert.ToDateTime(onboarding.laptopDispatchDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("Z{0}", (object)num1)].Value = !(Convert.ToDateTime(onboarding.laptopCollectionDate, provider) == null) ? "NA" : Convert.ToDateTime(onboarding.laptopCollectionDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("AA{0}", (object)num1)].Value = !onboarding.prevStatus.HasValue ? "NA" : Convert.ToString(onboarding.prevStatus.HasValue) ;
                excelWorksheet.Cells[string.Format("AB{0}", (object)num1)].Value = !(Convert.ToDateTime(onboarding.RENEGEUpdatedDate, provider) == null ) ? "NA" : Convert.ToDateTime(onboarding.RENEGEUpdatedDate, provider).ToString("dd/MM/yyyy");
                excelWorksheet.Cells[string.Format("AC{0}", (object)num1)].Value = !(onboarding.RENEGERemarks == null) ? "NA" : Convert.ToString(onboarding.RENEGERemarks);
                num1++;
            }


            return excel;
        }
    }
}
