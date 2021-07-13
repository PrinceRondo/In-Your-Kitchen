using InYourFridge.Data;
using InYourFridge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        //public virtual DbSet<MasterCity> MasterCities { get; set; }
        //public virtual DbSet<MasterCountry> MasterCountries { get; set; }
        //public virtual DbSet<MasterProduct> MasterProducts { get; set; }
        public virtual DbSet<MailConfiguration> MailConfigurations { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<UserPoint> UserPoints { get; set; }
        public virtual DbSet<EntryType> EntryTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<KitchenItemCategory> KitchenItemCategories { get; set; }
        public virtual DbSet<KitchenItem> KitchenItems { get; set; }
        public virtual DbSet<KitchenItemMarket> KitchenItemMarkets { get; set; }
        public virtual DbSet<KitchenProduct> KitchenProducts { get; set; }
        public virtual DbSet<KitchenProductItem> KitchenProductItems { get; set; }
    }
}
