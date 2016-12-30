using GpsApp_2._5.Models;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GpsApp_2._5.DAL
{
    public interface IPsLog<T>
    {
        void __Log(T obj, string message);
    }

    public interface IPsPager : IPagedList<GpsPoint>
    {
        
    }

    public class PsPager : IPsPager
    {
        public GpsPoint this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<GpsPoint> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IPagedList GetMetaData()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int FirstItemOnPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasNextPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFirstPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsLastPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int LastItemOnPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PageCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PageNumber
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PageSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TotalItemCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
    }
}
