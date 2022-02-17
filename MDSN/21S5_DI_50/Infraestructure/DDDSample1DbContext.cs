using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Mission;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Connections;
using DDDSample1.Infrastructure.Missions;
using DDDSample1.Infrastructure.Users;
using DDDSample1.Infrastructure.ConnectionRequests;
using DDDSample1.Infrastructure.Connections;

namespace DDDSample1.Infrastructure
{
    public class DDDSample1DbContext : DbContext
    {
        
        public DbSet<Mission> Missions { get; set; }

        public DbSet<User> Users {get; set;}

        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
        
        public DbSet<Connection> Connections { get; set; }

        public DDDSample1DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MissionsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConnectionEntityTypeConfiguration());
        }

    }
}