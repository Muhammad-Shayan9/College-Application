using Microsoft.EntityFrameworkCore;

namespace College_Application.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> dbcontext) : base(dbcontext)
        {

        }

        public DbSet<Admission> AdmissonDetails { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Event> Events { get; set; }


    }
}
