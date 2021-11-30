using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace T4EmployeesMVC.Models
{
    public class Classmodal : DbContext
    {
        public DbSet<user> users
        {
            get;
            set;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>().ToTable("pub.webUsers");
        }
    }
}





