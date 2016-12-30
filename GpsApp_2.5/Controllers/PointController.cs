using GpsApp_2._5.DAL;
using GpsApp_2._5.Models;
using System.Data;
using System.Web.Mvc;

namespace GpsApp_2._5.Controllers
{
    public class PointController : Controller
    {
        private IPointRepository pointRepository;
        
        public PointController()
        {
            this.pointRepository = new PointRepository(new GpsContext());
        }

        public PointController(IPointRepository pointRepository)
        {
            this.pointRepository = pointRepository;
        }

        public ActionResult Index()
        {
            var points = pointRepository
                .GetPoints();

            return View(points);
        }

        public ActionResult Create(int routeId)
        {
            ViewBag.RouteId = routeId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GpsPoint point)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pointRepository.Create(point);
                    pointRepository.Save();

                    return RedirectToAction("ShowPoints", "Route", new { @id = point.RouteId });
                }
            }
            catch (DataException dex)
            {
                pointRepository.__Log(dex, dex.Message);
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(point);
        }

        public ActionResult Edit(int id)
        {
            GpsPoint point = pointRepository.GetPoint(id);

            return View(point);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GpsPoint point)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pointRepository.Edit(point);
                    pointRepository.Save();

                    return RedirectToAction("ShowPoints", "Route", new { id = point.RouteId });
                }
            }
            catch (DataException dex)
            {
                pointRepository.__Log(dex, "exception: "+ dex.Message); 

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(point);
        }

        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var point = pointRepository.GetPoint(id);

            return View(point);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            GpsPoint point;
            try
            {
                point = pointRepository.GetPoint(id);
                pointRepository.Delete(id);
                pointRepository.Save();
            }
            catch (DataException dex)
            {
                pointRepository.__Log(dex, "exception: " + dex.Message);

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("ShowPoints", "Route", new { id = point.RouteId });
        }
    }
}