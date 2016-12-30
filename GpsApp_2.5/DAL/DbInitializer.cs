using GpsApp_2._5.Models;
using System;
using System.Collections.Generic;

namespace GpsApp_2._5.DAL
{
    public class DbInitializer : System.Data.Entity
        .CreateDatabaseIfNotExists<GpsContext>//.DropCreateDatabaseIfModelChanges<GpsContext>
    {
        public override void InitializeDatabase(GpsContext context)
        {
            // do nothing
            //Seed(context);
        }

        protected override void Seed(GpsContext context)
        {
            //
            var routes = new List<Route>
            {
                new Route
                {
                    RouteId = 1,
                    Description = "First point",
                    StartTime = DateTime.Parse("2001-09-01"),
                    LastModified = DateTime.Parse("2001-09-01"),
                    PointsList = new List<GpsPoint>()
                },
                new Route
                {
                    RouteId = 2,
                    Description = "Second point",
                    StartTime = DateTime.Parse("2002-09-01"),
                    LastModified = DateTime.Parse("2002-09-01"),
                    PointsList = new List<GpsPoint>()
                },
                new Route
                {
                    RouteId = 3,
                    Description = "Third point",
                    StartTime = DateTime.Parse("2003-09-01"),
                    LastModified = DateTime.Parse("2003-09-01"),
                    PointsList = new List<GpsPoint>()
                }
            };

            routes.ForEach(r => context.Routes.Add(r));
            context.SaveChanges();

            var points = new List<GpsPoint>
            {
                new GpsPoint
                {
                    Id = 1,
                    RouteId = 1,
                    PlaceName = "50,17",
                    DateTime = DateTime.Parse("2005-09-01"),
                    Latitude = "50",
                    Longitude = "17"
                },
                new GpsPoint
                {
                    Id = 2,
                    RouteId = 1,
                    PlaceName = "51,18",
                    DateTime = DateTime.Parse("2006-09-01"),
                    Latitude = "51",
                    Longitude = "18"
                },
                new GpsPoint
                {
                    Id = 3,
                    RouteId = 1,
                    PlaceName = "52,19",
                    DateTime = DateTime.Parse("2007-09-01"),
                    Latitude = "52",
                    Longitude = "19"
                }
            };

            points.ForEach(p => context.GpsPoints.Add(p));
            context.SaveChanges();

            //base.Seed(context);
        }
    }
}