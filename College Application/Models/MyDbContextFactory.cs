using College_Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace College_Application.Data
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            // Updated connection string to trust certificate
            optionsBuilder.UseSqlServer(
                "Server=.;Database=CollegeDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
