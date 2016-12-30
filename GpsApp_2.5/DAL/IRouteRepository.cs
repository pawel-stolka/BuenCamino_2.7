using GpsApp_2._5.Models;
using System;
using System.Collections.Generic;

namespace GpsApp_2._5.DAL
{
    public interface IRouteRepository : IDisposable, IPsLog<Route>, IPsLog<object>
    {
        IEnumerable<Route> GetRoutes();
        Route GetRoute(int id);
        void CreateRoute(Route route);
        void EditRoute(Route route);
        void DeleteRoute(int id);
        void Save();
    }
}
