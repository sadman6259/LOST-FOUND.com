using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using PagedList;
namespace LOF.Controllers
{
    public class AllproductsController : Controller
    {
        private LOFDbEntities6 db = new LOFDbEntities6();

        // GET: Allproducts
        public ActionResult Index(string searchString, string subcategorysrch,string divisionsrch, string locationsrch, string movieC, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var GenreLst = new List<string>();
            var LocationLst = new List<string>();

            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var GenreQry = from d in db.SubLocations
                           orderby d.SubLocationName
                           select d.SubLocationName;
            var GenreC = from d in db.Categories
                         orderby d.CategoryName
                         select d.CategoryName;
            GenreLst.AddRange(GenreC.Distinct());
            LocationLst.AddRange(GenreQry.Distinct());


            ViewBag.locationsrch = new SelectList(LocationLst);
            ViewBag.movieC = new SelectList(GenreLst);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var movies = from m in db.AllProductsTbls
                         select m;
            switch (sortOrder)
            {

                case "Date":
                    movies = movies.OrderBy(s => s.DateOfFound);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.DateOfFound);
                    break;
                default:
                    movies = movies.OrderByDescending(s => s.DateOfFound);
                    break;
            }
            if (!String.IsNullOrEmpty(subcategorysrch))
            {
                movies = movies.Where(m => m.SubCategory1.SubCategoryName.Contains(subcategorysrch));
                //   return View(movies.ToList());

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
                //   return View(movies.ToList());

            }
            if (!string.IsNullOrEmpty(divisionsrch))
            {
                movies = movies.Where(x => x.Location1.LocationName == divisionsrch);
            }
            if (!string.IsNullOrEmpty(locationsrch))
            {
                movies = movies.Where(x => x.SubLocation1.SubLocationName == locationsrch);
            }
            if (!string.IsNullOrEmpty(movieC))
            {
                movies = movies.Where(x => x.Category1.CategoryName == movieC);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          AllProductsTbl allproductstbl = db.AllProductsTbls.Find(id);
            if (allproductstbl == null)
            {
                return HttpNotFound();
            }
            TempData["Title"] = db.AllProductsTbls
                  .Where(x => x.Id == id)
                  .Select(x => x.Title)
                  .FirstOrDefault();
            TempData["Location"] = db.AllProductsTbls
                 .Where(x => x.Id == id)
                 .Select(x => x.Location1.LocationName)
                 .FirstOrDefault();
            TempData["SubLocation"] = db.AllProductsTbls
              .Where(x => x.Id == id)
              .Select(x => x.SubLocation1.SubLocationName)
              .FirstOrDefault();
            TempData["Details"] = db.AllProductsTbls
             .Where(x => x.Id == id)
             .Select(x => x.Details)
             .FirstOrDefault();
            TempData["SubCategory"] = db.AllProductsTbls
                .Where(x => x.Id == id)
               .Select(x => x.SubCategory1.SubCategoryName)
                .FirstOrDefault();
            if (allproductstbl.UniqueKey != null) {
                TempData["Uniquekey"] = db.AllProductsTbls
                 .Where(x => x.Id == id)
                .Select(x => x.UniqueKey)
                 .FirstOrDefault();
            }
            return View(allproductstbl);
        }
        public ActionResult Matchedproducts(string sortOrder, string title, int? Id)
        {
            
            string Title = TempData["Title"].ToString();
            string Location = TempData["Location"].ToString();
            string SubLocation = TempData["SubLocation"].ToString();
            string Details = TempData["Details"].ToString();
            string SubCategory = TempData["SubCategory"].ToString();


            if (TempData["Uniquekey"] == null)
            {
                var products = from m in db.AllProductsTbls
                               where ((m.SubCategory1.SubCategoryName.Contains(SubCategory)) && (m.Title.Contains(Title) || m.Location1.LocationName.Contains(Location) || m.SubLocation1.SubLocationName.Contains(SubLocation) || m.Details.Contains(Details)))
                               select m;
                return View(products.ToList());

                //   return RedirectToAction("NotFound");
            }
            int uniquekey = (int)TempData["Uniquekey"];
            

            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            /*     var movies = from m in db.AllProductsTbls
                              where  ((m.SubCategory1.SubCategoryName.Contains(SubCategory))&&(m.Title.Contains(Title)||m.Location1.LocationName.Contains(Location) || m.SubLocation1.SubLocationName.Contains(SubLocation)|| m.Details.Contains(Details)))
                              select m;
                              */


            var movies = from m in db.AllProductsTbls
                             where (m.UniqueKey == uniquekey)
                             select m;
            

            switch (sortOrder)
            {

                case "Date":
                    movies = movies.OrderBy(s => s.DateOfFound);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.DateOfFound);
                    break;
                default:
                    movies = movies.OrderByDescending(s => s.DateOfFound);
                    break;
            }
            return View(movies.ToList());
        }
        public ActionResult NotFound()
        {
            return View();
        }


    }

}