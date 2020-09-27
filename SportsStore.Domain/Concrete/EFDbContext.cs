using Microsoft.AspNet.Identity.EntityFramework;
using SportsStore.Domain.Entities;
using System;
using System.Data.Entity;

namespace SportsStore.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext<ApplicationUser>
    {
        public EFDbContext() : base("EFDbContext") { }

        public DbSet<Product> Products { get; set; }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }
    }
}
