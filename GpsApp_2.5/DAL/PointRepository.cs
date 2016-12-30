using GpsApp_2._5.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GpsApp_2._5.DAL
{
    public class PointRepository : IPointRepository
    {
        private GpsContext context;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public PointRepository(GpsContext gpsContext)
        {
            this.context = gpsContext;
        }

        public GpsPoint GetPoint(int id)
        {
            removeJsonCirculation();
            var data = context.GpsPoints
                .Where(p => p.Id == id);

            return data.ToList().FirstOrDefault();
        }

        public IEnumerable<GpsPoint> GetPoints()
        {
            removeJsonCirculation(); // in order to use it as a base to Json query <--------------
            return context.GpsPoints
                .ToList();
        }

        public IEnumerable<GpsPoint> GetPointsByRoute(int id)
        {
            removeJsonCirculation();
            var data = context.GpsPoints
                .Where(c => c.RouteId == id);
            return data;
        }

        public void Create(GpsPoint point)
        {
            point.DateTime = DateTime.UtcNow;
            var id = point.RouteId;
            var route = context.Routes.Find(id);
            route.LastModified = DateTime.UtcNow;

            context.GpsPoints
                .Add(point);

            __Log(point, "Point created.");
        }

        public void Edit(GpsPoint point)
        {
            var id = point.RouteId;
            var route = context.Routes.Find(id);
            route.LastModified = DateTime.UtcNow;

            context.Entry(point)
                .State = EntityState.Modified;

            __Log(point, "Point edited.");
        }

        public void Delete(int id)
        {
            GpsPoint point = context.GpsPoints
                .Find(id);
            __Log(point, "Point's going to be deleted.");
            context.GpsPoints
                .Remove(point);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private void removeJsonCirculation()
        {
            context.Configuration.ProxyCreationEnabled = false;
        }

        public void __Log(GpsPoint point, string message)
        {
            logger.Info(message);
            logger.Info(" id: {0} |{1} - {2} ({3},{4})", point.Id, point.DateTime, point.PlaceName, point.Latitude, point.Longitude);
            logger.Info(" {0} | routeId {1}", point.Note, point.RouteId);
            logger.Info("---------------------------------------------");
        }

        public void __Log(object obj, string message)
        {
            logger.Fatal(message);
            logger.Fatal(" {0} ", obj);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        


        #endregion
    }
}