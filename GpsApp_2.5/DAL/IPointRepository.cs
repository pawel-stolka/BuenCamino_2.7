using GpsApp_2._5.Models;
using System;
using System.Collections.Generic;

namespace GpsApp_2._5.DAL
{
    public interface IPointRepository : IDisposable, IPsLog<GpsPoint>, IPsLog<object>
    {
        IEnumerable<GpsPoint> GetPoints();
        IEnumerable<GpsPoint> GetPointsByRoute(int routeId);
        GpsPoint GetPoint(int id);
        void Create(GpsPoint point);
        void Edit(GpsPoint point);
        void Delete(int id);
        void Save();
    }
}

