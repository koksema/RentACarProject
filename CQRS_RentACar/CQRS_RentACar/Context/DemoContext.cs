using CQRS_RentACar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.Context
{
    public class DemoContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-G6TTPQ6;initial catalog=CQRSRentACarDb;integrated security=true;trust server certificate=true");
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}
