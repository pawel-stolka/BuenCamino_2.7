using GpsApp_2._5.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace GpsApp_2._5.DAL
{
    public class FileRepository : IPointRepository
    {
        public string path { get; set; }
        public string fileName { get; set; }
        public string FullPath { get; set; }

        // takes by default
        // path: "D:\CODE\Libraries\tcx"
        // & filename "Pawe_Stolka_2016-01-27_14-06-18.tcx"

        public FileRepository()
        {
            this.path = @"D:\CODE\Libraries\tcx";
            this.fileName = "Pawe_Stolka_2016-01-27_14-06-18.tcx";
            this.FullPath = Path.Combine(
                path,
                fileName
                );
        }

        public FileRepository(string filename, string path = "")
        {
            if (path == string.Empty)
            {
                this.path = @"D:\CODE\Libraries\tcx"; 
            }
            else
            {
                this.path = path;

            }

            this.fileName = filename;
            this.FullPath = System.IO.Path.Combine(
                this.path,
                fileName
                );
        }


        public IEnumerable<GpsPoint> GetPoints(string file)
        {
            var points = GetTracks(file);//.Take(10);
            return points;
        }

        public IEnumerable<GpsPoint> GetPointsByRoute(int routeId)
        {
            throw new NotImplementedException();
        }

        public GpsPoint GetPoint(int id)
        {
            throw new NotImplementedException();
        }

        /// --------------------------------------------
        /// ---------------- private -------------------
        /// --------------------------------------------
        /// 
        private XDocument loadXml(string path)
        {
            XDocument xml = XDocument.Load(path);
            return xml;
        }

        public IEnumerable<int> GetPointsCount()
        {
            var files = getAllFileNamesFromFolder(path);
            var counter = new List<int>();
            int i = 0;
            foreach (var file in files)
            {
                var trackList = GetTracks(file).Count;
                counter.Add(trackList);
                i++;
            }
            return counter;
        }

        public IEnumerable<GpsPoint> GetPoints()
        {
            var file = getAllFileNamesFromFolder(path);
            var thisFile = file[1];
            var ret = GetTracks(thisFile);
            return ret;
            //var files = getAllFileNamesFromFolder(_path);
            //List<int> pointList = new List<int>();
            //foreach (var file in files)
            //{
            //    var point = GetPoints(file).Count();
            //    pointList.Add(
            //        point
            //        );
            //}
        }

        public List<string> getAllFileNamesFromFolder(string folder)
        {
            var list = Directory.GetFiles(
                folder,
                "*.tcx")
                .ToList();
            return list;
        }

        public List<GpsPoint> GetEveryNPoints(string filename, int n)
        {
            var _path = Path.Combine(
                this.path, 
                filename);

            var pointsList = GetTracks(_path);
            var ret = new List<GpsPoint>();
            for (int i = 0; i < pointsList.Count; i++)
            {
                if (i%n == 0)
                {
                    ret.Add(
                        new GpsPoint
                        {
                            DateTime = pointsList[i].DateTime,
                            Latitude = pointsList[i].Latitude,
                            Longitude = pointsList[i].Longitude
                        });
                }
            }
            return ret;
        } 

        

        public List<GpsPoint> GetTracks(string fullPath)
        {
            var xml = loadXml(fullPath);
            List<GpsPoint> tracks = new List<GpsPoint>();
            try
            {
                tracks = (
                    from item in xml.Descendants("Trackpoint")
                    select new GpsPoint()
                    {
                        DateTime = DateTime.Parse(
                            item.Element("Time").Value),
                        Latitude = item.Element("Position").Element("LatitudeDegrees").Value,
                        Longitude = item.Element("Position").Element("LongitudeDegrees").Value
                    }
                    ).ToList();
                return tracks;
            }
            catch (Exception)
            {
                return tracks;
            }
            
        }

        public List<int> GetAllDaysCounter()
        {
            var files = getAllFileNamesFromFolder(path);
            var tracks = new List<int>();
            foreach (var file in files)
            {
                var counter = GetTracks(file).Count;
                tracks.Add(counter);
            }
            return tracks;
        }

        public void Create(GpsPoint point)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(GpsPoint point)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void __Log(GpsPoint obj, string message)
        {
            throw new NotImplementedException();
        }

        public void __Log(object obj, string message)
        {
            throw new NotImplementedException();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FileRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        


        #endregion
    }
}