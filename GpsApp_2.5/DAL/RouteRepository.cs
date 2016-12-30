using GpsApp_2._5.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GpsApp_2._5.DAL
{
    public class RouteRepository : IRouteRepository
    {
        private GpsContext context;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public RouteRepository(GpsContext gpsContext)
        {
            this.context = gpsContext;
        }



        public IEnumerable<Route> GetRoutes()
        {
            return context.Routes
                .ToList();
        }

        public Route GetRoute(int id)
        {
            return context.Routes
                .Find(id);
        }

        public void CreateRoute(Route route)
        {
            route.StartTime = route.LastModified = DateTime.UtcNow;
            context.Routes
                .Add(route);
            __Log(route, "Route created.");
        }

        public void EditRoute(Route route)
        {
            __Log(route, "Route edited. - before ");
            route.LastModified = DateTime.UtcNow;
            context.Entry(route)
                .State = EntityState.Modified;
            __Log(route, "Route edited. - after ");
        }

        public void DeleteRoute(int id)
        {
            Route route = context.Routes
                .Find(id);
            __Log(route, "Route is going to be deleted.");
            context.Routes
                .Remove(route);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void __Log(Route route, string message)
        {
            logger.Info(message);
            logger.Info(" routeId: {0} |{1} | {2}", route.RouteId, route.StartTime, route.LastModified);
            logger.Info(" - {0}", route.Description);
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

        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

        #endregion

    }
}