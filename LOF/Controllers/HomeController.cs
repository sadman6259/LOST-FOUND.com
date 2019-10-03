using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LOF.ViewModel;
using PagedList;

namespace LOF.Controllers
{
    public class HomeController : Controller
    {
        private LOFDbEntities5 db = new LOFDbEntities5();

        public ActionResult Index()
        {
            var mymodel = new Home();

            mymodel.Topfound = db.Topfoundtbls.ToList();
            mymodel.Toplost = db.TopLosttbls.ToList();

            return View(mymodel);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Mytemplate()
        {
            return View();
        }
    }
}