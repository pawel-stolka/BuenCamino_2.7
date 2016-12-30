using GpsApp_2._5.DAL;
using GpsApp_2._5.Models;
using GpsApp_2._5.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace GpsApp_2._5.Controllers
{
    public class RouteController : Controller
    {
        private int routesOnPage = 5;
        private IPointRepository pointRepository;
        private IRouteRepository routeRepository;

        public RouteController()
        {
            this.routeRepository = new RouteRepository(new GpsContext());
            this.pointRepository = new PointRepository(new GpsContext());
        }

        public RouteController(IRouteRepository routeRepository)
        {
            this.routeRepository = routeRepository;
        }


        public ActionResult Index(string sortOrder, int? page)
        {
            var routesCounter = routeRepository.GetRoutes().Count();
            ViewBag.RouteDescription = "Routes: " + routesCounter; 
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreatedDateSortParam = String.IsNullOrEmpty(sortOrder) ? "CreatedDate" : "";
            ViewBag.ModifiedDateSortParam = sortOrder == "ModifiedDate" ? "modifiedDate_desc" : "ModifiedDate";
            ViewBag.Page = page;

            var routes = from r in routeRepository.GetRoutes()
                         select r;

            switch (sortOrder)
            {
                case "CreatedDate":
                    routes = routes.OrderBy(r => r.StartTime);
                    break;
                case "ModifiedDate":
                    routes = routes.OrderBy(r => r.LastModified);
                    break;
                case "modifiedDate_desc":
                    routes = routes.OrderByDescending(r => r.LastModified);
                    break;
                default:
                    routes = routes.OrderByDescending(r => r.StartTime);
                    break;
            }
            int pageSize = routesOnPage;
            int pageNumber = (page ?? 1);

            return View(routes
                .ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ShowPoints(int id, int? page = null)
        {
            ViewBag.RouteDescription = routeRepository
                .GetRoute(id)
                .Description;

            ViewBag.RouteId = id;

            IEnumerable<GpsPoint> data = pointRepository
                .GetPointsByRoute(id)
                .OrderBy(d => d.Id)
                .ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            
            if (data != null)
            {
                return View(data.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult GetPointsByDateJson(int id, string dateTime)
        {
            var dateStart = DateTime.Parse(dateTime);
            var dateEnd = dateStart.AddDays(1);



            var data = pointRepository
                .GetPointsByRoute(id)
                .Where(d => d.DateTime > dateStart)
                .Where(d=> d.DateTime < dateEnd);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPointsJson(int id)
        {
            var data = pointRepository
                .GetPointsByRoute(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoutePointViewModel routePointVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    routeRepository.CreateRoute(routePointVM.Route);
                    routeRepository.Save();

                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {
                routeRepository.__Log(dex, dex.Message);

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(routePointVM);
        }

        public ActionResult Edit(int id)
        {
            Route route = routeRepository.GetRoute(id);
            var description = route.Description;
            ViewBag.RouteDescription = description;

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Route route)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    routeRepository.EditRoute(route);
                    routeRepository.Save();

                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {
                routeRepository.__Log(dex, dex.Message);

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(route);
        }

        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Route route = routeRepository.GetRoute(id);
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                routeRepository.DeleteRoute(id);
                routeRepository.Save();
            }
            catch (DataException dex)
            {
                routeRepository.__Log(dex, dex.Message);

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            routeRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}