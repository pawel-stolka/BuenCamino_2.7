using GpsApp_2._5.DAL;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace GpsApp_2._5.Controllers
{
    public class FileController : Controller
    {
        private FileRepository fileRepository;
        //private IPointRepository fileRepository;
        private IPsLog<DataException> logger;

        public FileController()
        {
            //var path = @"D:\CODE\Libraries\tcx";
            //var filename = "Pawe_Stolka_2016-01-27_14-06-18.tcx";
            this.fileRepository = new FileRepository();//path);//, filename);
        }

        public FileController(IPointRepository fileRepository)
        {
            //this.fileRepository = fileRepository;
        }

        public ActionResult Index()
        {
            var points = from f in fileRepository
                         .GetEveryNPoints(fileRepository.FullPath, 10)
                         select f;
            //var points = from f in fileRepository.GetPoints()
            //             select f;
            return View(points);
        }

        // GET: File/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: File/Create
        public ActionResult Create()
        {
            FileRepository fileRepo = new FileRepository();

            var points = fileRepo.GetPointsCount().ToList();

            return View(points);
        }

        // POST: File/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: File/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: File/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: File/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: File/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
