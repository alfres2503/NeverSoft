﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NeverlandDBEntities : DbContext
    {
        public NeverlandDBEntities()
            : base("name=NeverlandDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Incidence> Incidence { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCategory> NewsCategory { get; set; }
        public virtual DbSet<PaymentItem> PaymentItem { get; set; }
        public virtual DbSet<PaymentPlan> PaymentPlan { get; set; }
        public virtual DbSet<PlanAssignment> PlanAssignment { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Residence> Residence { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
    }
}
