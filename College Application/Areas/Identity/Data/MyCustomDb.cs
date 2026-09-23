using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College_Application.Areas.Identity.Data
{
    public class MyCustomDb : IdentityDbContext<ApplicationUser>
    {
        public MyCustomDb(DbContextOptions<MyCustomDb> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            ModelBuilder modelBuilder = builder.ApplyConfiguration(new ApplicationUserEntityData());
        }
    }

    internal class ApplicationUserEntityData : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(30);
            builder.Property(x => x.DateOfBirth);
        }
    }
}
