﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOF;
using System.IO;
using System.Data.Entity.Validation;
using PagedList;
using LOF.Models;
using LOF.ViewModel;

namespace LOF.Controllers
{
    // [AuthenticationSampleFilter]


    public class FoundProductsController : Controller
    {
        private LOFDbEntities6 db = new LOFDbEntities6();
        // GET: FoundProducts
        public ActionResult Index(string searchString,string subcategorysrch,string divisionsrch, string locationsrch, string movieC, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var LocationLst = new List<string>();
            var GenreLst = new List<string>();
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

            var movies = from m in db.Foundtbls
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
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));

        }
        // GET: FoundProducts/Details/5
        public ActionResult Details(int? id)
        {
            var foundproducts = db.Foundtbls.Include(m => m.Category1).Include(m => m.SubCategory1).SingleOrDefault(x => x.Id == id);
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Foundtbl foundtbl = db.Foundtbls.Find(id);
            if (foundproducts == null)
            {
                return HttpNotFound();
            }
            if (foundtbl.UniqueKey != null)
            {
                TempData["Uniquekey"] = db.Foundtbls
                 .Where(x => x.Id == id)
                .Select(x => x.UniqueKey)
                 .FirstOrDefault();
            }

            return View(foundproducts);
        }
        public ActionResult Matchedproducts(string sortOrder, string title, int? Id)
        {
            if (TempData["Uniquekey"] == null)
            {
                return RedirectToAction("NotFound");
            }
        

            int uniquekey = (int)TempData["Uniquekey"];


            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            /*     var movies = from m in db.AllProductsTbls
                              where  ((m.SubCategory1.SubCategoryName.Contains(SubCategory))&&(m.Title.Contains(Title)||m.Location1.LocationName.Contains(Location) || m.SubLocation1.SubLocationName.Contains(SubLocation)|| m.Details.Contains(Details)))
                              select m;
                              */


            var movies = from m in db.Losttbls
                         where (m.UniqueKey == uniquekey)
                         select m;


            switch (sortOrder)
            {

                case "Date":
                    movies = movies.OrderBy(s => s.DateOfLost);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.DateOfLost);
                    break;
                default:
                    movies = movies.OrderByDescending(s => s.DateOfLost);
                    break;
            }
            return View(movies.ToList());
        }
        public ActionResult NotFound()
        {
            return View();
        }
        [Authorize]

        // GET: FoundProducts/Create
        public ActionResult Create()
        {
            ViewBag.username = User.Identity.Name;
          //  ViewBag.username = (string)Session["username"];
          //  ViewBag.username = (from u in db.LoginTbls
                   //           select u.RegisterTbl.UserName).Max();
      //   ViewBag.cellno = (from u in db.LoginTbls
                           //   select u.RegisterTbl.ContactNo).Max();
                   var lastrecord = (from c in db.LoginTbls orderby c.Id descending select c).First();
            ViewBag.cellno = lastrecord.RegisterTbl.ContactNo;
            ViewBag.address = lastrecord.RegisterTbl.Address;
            /*      ViewBag.cellno =
          (from D in db.RegisterTbls
           join E in db.LoginTbls on D.Id equals E.RegisterId
           orderby E.Id, D.Id descending
           select new
           {
               D.ContactNo
           }).FirstOrDefault();
           */
            bindLocation();
            bindCategory();
            return View();

        }
        public void bindLocation()
        {
            var location = db.Locations.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Location", Value = "0" });
            foreach (var m in location)
            {



                li.Add(new SelectListItem { Text = m.LocationName, Value = m.LocationId.ToString() });
                ViewBag.location = li;
                // ViewBag.Category = new SelectList(li, m.CategoryName,m.CategoryName);
            }
        }
        public void bindCategory()
        {
            var category = db.Categories.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Category", Value = "0" });
            foreach (var m in category)
            {



                 li.Add(new SelectListItem { Text = m.CategoryName, Value = m.CategoryId.ToString() });
                 ViewBag.category = li;
               // ViewBag.Category = new SelectList(li, m.CategoryName,m.CategoryName);
            }
        }

        public JsonResult getSubCategory(int id)
        {
            var ddlSubCategory = db.SubCategories.Where(x => x.CategoryId == id).ToList();
            List<SelectListItem> lisub = new List<SelectListItem>();

            lisub.Add(new SelectListItem { Text = "Select SubCategory", Value = "0" });
            if (ddlSubCategory != null)
            {
                foreach (var x in ddlSubCategory)
                {
                    lisub.Add(new SelectListItem { Text = x.SubCategoryName, Value = x.SubCategoryId.ToString() });
                }
            }
            return Json(new SelectList(lisub, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getSubLocation(int id)
        {
            var ddlSubLocation = db.SubLocations.Where(x => x.LocationId == id).ToList();
            List<SelectListItem> lisub = new List<SelectListItem>();

            lisub.Add(new SelectListItem { Text = "Select SubLocation", Value = "0" });
            if (ddlSubLocation != null)
            {
                foreach (var x in ddlSubLocation)
                {
                    lisub.Add(new SelectListItem { Text = x.SubLocationName, Value = x.SubLocationId.ToString() });
                }
            }
            return Json(new SelectList(lisub, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        // POST: Foundstbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(AdminApprovalTbl approve,Foundtbl found)
        {
            

             string fileName = Path.GetFileNameWithoutExtension(approve.ImageFile.FileName);

            //  string fileName = Path.GetFileNameWithoutExtension(loststbl.ImageFile.FileName);
            // string fileNames = Path.GetFileNameWithoutExtension(losttbl.ImageFile.FileName);

             string extension = Path.GetExtension(approve.ImageFile.FileName);

            //  string extension = Path.GetExtension(loststbl.ImageFile.FileName);
            //   string extensions = Path.GetExtension(losttbl.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;


            // fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //   fileNames = fileNames + DateTime.Now.ToString("yymmssfff") + extensions;

            approve.Image = "~/Image/" + fileName;

            //  loststbl.Image = "~/Image/" + fileName;
            //  losttbl.Image = "~/Image/" + fileNames;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

            //  fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            //   fileNames = Path.Combine(Server.MapPath("~/Image/"), fileNames);
            approve.ImageFile.SaveAs(fileName);

            //   loststbl.ImageFile.SaveAs(fileName);
            //   losttbl.ImageFile.SaveAs(fileNames);


            if (ModelState.IsValid)
            {
                approve.CategoryId = Int32.Parse (approve.Category);
                approve.SubCategoryId = Int32.Parse ( approve.SubCategory);

                approve.LocationId = Int32.Parse(approve.Location);
               approve.SubLocationId = Int32.Parse(approve.SubLocation);


                //     loststbl.CategoryId = Int32.Parse(loststbl.Category);
                //    loststbl.SubCategoryId = Int32.Parse(loststbl.SubCategory);

                //    loststbl.LocationId = Int32.Parse(loststbl.Location);
                //      loststbl.SubLocationId = Int32.Parse(loststbl.SubLocation);

                //   losttbl.CategoryId = Int32.Parse(losttbl.Category);
                //   losttbl.SubCategoryId = Int32.Parse(losttbl.SubCategory);

                //     losttbl.LocationId = Int32.Parse(loststbl.Location);
                //     losttbl.SubLocationId = Int32.Parse(losttbl.Sublocation);
          

                db.AdminApprovalTbls.Add(approve);
                //     ViewBag.cellno = db.LoginTbls.Max(item => item.RegisterTbl.ContactNo);
                //  ViewBag.cellno = found.CellNo;
              
                //   db.Foundtbls.Add(loststbl);
                //   db.AllProductsTbls.Add(losttbl);
                try
                {
                    db.SaveChanges();
              

                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
              return RedirectToAction("Success");
            }

          return View(approve);
        }
        // GET: FoundProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foundtbl foundtbl = db.Foundtbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            return View(foundtbl);
        }

        // POST: FoundProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Image,Category,Location,Details,SubLocation,DateOfFound")] Foundtbl foundtbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foundtbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foundtbl);
        }
       

        // GET: FoundProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foundtbl foundtbl = db.Foundtbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            return View(foundtbl);
        }

        // POST: FoundProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Foundtbl foundtbl = db.Foundtbls.Find(id);
            db.Foundtbls.Remove(foundtbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult searchpartial()
        {
            List<Foundtbl> getProduct = new List<Foundtbl>();
            getProduct = db.Foundtbls.ToList();
            if (getProduct != null)
            {
                return View("searchpartial", getProduct);
            }
            else
            {
                return View("searchpartial"); // return without passing model
            }
        }
        public ActionResult Success()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
