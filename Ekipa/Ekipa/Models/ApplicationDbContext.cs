using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Ekipa.Models.DB;

namespace Ekipa.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyTag> CompanyTags { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Term> CompanyTerm { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Reservation> Rezervations { get; set; }
        public virtual DbSet<Opinion> Opinions { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<Ekipa.Models.ViewModel.Company.CompanyImagesVM> CompanyImagesVMs { get; set; }
    }
}