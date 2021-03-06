//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ticketing_Dashboard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SR_Request
    {
        public int srPk { get; set; }
        public string requisitionSource { get; set; }
        public string name { get; set; }
        public string sapid { get; set; }
        public string projectName { get; set; }
        public string positionType { get; set; }
        public Nullable<decimal> experience { get; set; }
        public Nullable<int> noOfPosition { get; set; }
        public string designation { get; set; }
        public string band { get; set; }
        public Nullable<System.DateTime> billStartDate { get; set; }
        public Nullable<System.DateTime> onboardingDate { get; set; }
        public string RMSapid { get; set; }
        public Nullable<System.DateTime> billingLossStartDate { get; set; }
        public string location { get; set; }
        public string role { get; set; }
        public string employeeGroup { get; set; }
        public string isContract { get; set; }
        public Nullable<System.DateTime> contratToDate { get; set; }
        public Nullable<System.DateTime> contractFromDate { get; set; }
        public Nullable<decimal> buyRate { get; set; }
        public string contractType { get; set; }
        public Nullable<decimal> sellRate { get; set; }
        public string skillSet { get; set; }
        public string primarySkill { get; set; }
        public string secondarySkill { get; set; }
        public string interviewerSapid1 { get; set; }
        public string interviewerSapid2 { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string status { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string remarks { get; set; }
        public string srNo { get; set; }
        public Nullable<System.DateTime> closingDate { get; set; }
        public string RPFUpdated { get; set; }
        public string requesterEmailID { get; set; }
        public Nullable<System.DateTime> slaDate { get; set; }
    }
}
