﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace project.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class onlinetestEntities : DbContext
    {
        public onlinetestEntities()
            : base("name=onlinetestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ADMIN> ADMIN { get; set; }
        public DbSet<QUESTION> QUESTIONS { get; set; }
        public DbSet<STUDENT> STUDENT { get; set; }
        public DbSet<CATEGORY> CATEGORY { get; set; }
    }
}
