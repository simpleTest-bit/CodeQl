﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TicketingToolDBEntities : DbContext
    {
        public TicketingToolDBEntities()
            : base("name=TicketingToolDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Band_Master> Band_Master { get; set; }
        public virtual DbSet<Laptop_Master> Laptop_Master { get; set; }
        public virtual DbSet<Location_Master> Location_Master { get; set; }
        public virtual DbSet<Login_Master> Login_Master { get; set; }
        public virtual DbSet<Mail_Master> Mail_Master { get; set; }
        public virtual DbSet<Onboarding_Master> Onboarding_Master { get; set; }
        public virtual DbSet<SR_Request> SR_Request { get; set; }
        public virtual DbSet<Status_Master> Status_Master { get; set; }
        public virtual DbSet<VW_OnboardingMaster> VW_OnboardingMaster { get; set; }
    }
}
