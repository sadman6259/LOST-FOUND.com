﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LOFDbEntities6 : DbContext
    {
        public LOFDbEntities6()
            : base("name=LOFDbEntities6")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminApprovalTbl> AdminApprovalTbls { get; set; }
        public virtual DbSet<AllProductsTbl> AllProductsTbls { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Foundtbl> Foundtbls { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LoginTbl> LoginTbls { get; set; }
        public virtual DbSet<Losttbl> Losttbls { get; set; }
        public virtual DbSet<RegisterTbl> RegisterTbls { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<SubLocation> SubLocations { get; set; }
        public virtual DbSet<Topfoundtbl> Topfoundtbls { get; set; }
        public virtual DbSet<TopLosttbl> TopLosttbls { get; set; }
        public virtual DbSet<AdminLoginTbl> AdminLoginTbls { get; set; }
    }
}