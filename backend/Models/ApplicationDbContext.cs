using Microsoft.EntityFrameworkCore;

namespace backend.Models {
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options) {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        //public virtual DbSet<Caterer> Caterers { get; set; }
        //public virtual DbSet<Message> Messages { get; set; }
        //public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        //public virtual DbSet<Item> Items { get; set; }
        //public virtual DbSet<CuisineType> CuisineTypes { get; set; }
        //public virtual DbSet<Booking> Bookings { get; set; }

    }
}
