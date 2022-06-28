using CodeGeneration.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;
using CodeGeneration.Configurations.Entities;

namespace CodeGeneration.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>,
     IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Code> Codes { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>();
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.Entity<Code>().Property("Status").HasDefaultValue(0);
        }

    }
}
