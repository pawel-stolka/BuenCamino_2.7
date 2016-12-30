using GpsApp_2._5.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GpsApp_2._5.DAL
{
    public class GpsContext : DbContext
    {

        public GpsContext() : base("GpsContext_12_29")
        {
            Database.SetInitializer(new DbInitializer()); //new CreateDatabaseIfNotExists<GpsContext>());// DbInitializer());

        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<GpsPoint> GpsPoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //base.OnModelCreating(modelBuilder);
        }
    }
}