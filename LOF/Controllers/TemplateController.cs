using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LOF;
using LOF.ViewModel;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using PagedList;

namespace LOF.Controllers
{
    public class TemplateController : Controller
    {
        LOFDbEntities6 obj = new LOFDbEntities6();
        public ActionResult CommonIndex(string subcategorysrch,string locationsrch)
        {
            var commonmodel = new CommonIndex();
            var movies = from m in obj.Foundtbls
                         select m;
            var cinema = from n in obj.Losttbls
                         select n;

            if (!string.IsNullOrEmpty(subcategorysrch))
            {
              movies = movies.Where(m => m.SubCategory.Contains(subcategorysrch));
               cinema = cinema.Where(n => n.SubCategory.Contains(subcategorysrch));
                //   return View(movies.ToList());

            }
            if (!string.IsNullOrEmpty(locationsrch))
            {
           movies = movies.Where(x => x.Location == locationsrch);
            cinema = cinema.Where(x => x.Location == locationsrch);

            }
            commonmodel.found = obj.Foundtbls.ToList();
            commonmodel.lost = obj.Losttbls.ToList();

            return View(commonmodel);
        }
        // GET: Template
        public ActionResult Index()
        {
            
                var mymodel = new Home();
          
            mymodel.  Topfound = obj.Topfoundtbls.ToList();
          mymodel.  Toplost = obj.TopLosttbls.ToList();

        
            return View(mymodel);
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
            var lastrecord = (from c in obj.LoginTbls orderby c.Id descending select c).First();
            ViewBag.cellno = lastrecord.RegisterTbl.ContactNo;
            ViewBag.address = lastrecord.RegisterTbl.Address;
            bindLocation();
            bindCategory();
            return View();
        }
        public void bindLocation()
        {
            var location = obj.Locations.ToList();
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
            var category = obj.Categories.ToList();
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
            var ddlSubCategory = obj.SubCategories.Where(x => x.CategoryId == id).ToList();
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
            var ddlSubLocation = obj.SubLocations.Where(x => x.LocationId == id).ToList();
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

        public ActionResult Create(AdminApprovalTbl approve)
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
                approve.CategoryId = Int32.Parse(approve.Category);
                approve.SubCategoryId = Int32.Parse(approve.SubCategory);

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
                obj.AdminApprovalTbls.Add(approve);

                //   db.Foundtbls.Add(loststbl);
                //   db.AllProductsTbls.Add(losttbl);
                try
                {
                    obj.SaveChanges();
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

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topfoundtbl foundtbl = obj.Topfoundtbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            TempData["Title"] = obj.Topfoundtbls
              .Where(x => x.Id == id)
              .Select(x => x.Title)
              .FirstOrDefault();
            TempData["Location"] = obj.Topfoundtbls
                 .Where(x => x.Id == id)
                 .Select(x => x.Location1.LocationName)
                 .FirstOrDefault();
            TempData["SubLocation"] = obj.Topfoundtbls
              .Where(x => x.Id == id)
              .Select(x => x.SubLocation1.SubLocationName)
              .FirstOrDefault();
            TempData["Details"] = obj.Topfoundtbls
             .Where(x => x.Id == id)
             .Select(x => x.Details)
             .FirstOrDefault();
            TempData["SubCategory"] = obj.Topfoundtbls
                .Where(x => x.Id == id)
               .Select(x => x.SubCategory1.SubCategoryName)
                .FirstOrDefault();
            if (foundtbl.UniqueKey != null)
            {
                TempData["Uniquekey"] = obj.Topfoundtbls
                 .Where(x => x.Id == id)
                .Select(x => x.UniqueKey)
                 .FirstOrDefault();
            }
            return View(foundtbl);
        }
        public ActionResult DetailsToplost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopLosttbl foundtbl = obj.TopLosttbls.Find(id);
            if (foundtbl == null)
            {
                return HttpNotFound();
            }
            TempData["Titletl"] = obj.TopLosttbls
             .Where(x => x.Id == id)
             .Select(x => x.Title)
             .FirstOrDefault();
            TempData["Locationtl"] = obj.TopLosttbls
                 .Where(x => x.Id == id)
                 .Select(x => x.Location1.LocationName)
                 .FirstOrDefault();
            TempData["SubLocationtl"] = obj.TopLosttbls
              .Where(x => x.Id == id)
              .Select(x => x.SubLocation1.SubLocationName)
              .FirstOrDefault();
            TempData["Detailstl"] = obj.TopLosttbls
             .Where(x => x.Id == id)
             .Select(x => x.Details)
             .FirstOrDefault();
            TempData["SubCategorytl"] = obj.TopLosttbls
                .Where(x => x.Id == id)
               .Select(x => x.SubCategory1.SubCategoryName)
                .FirstOrDefault();
            if (foundtbl.UniueKey != null)
            {
                TempData["Uniuekey"] = obj.TopLosttbls
                 .Where(x => x.Id == id)
                .Select(x => x.UniueKey)
                 .FirstOrDefault();
            }
            return View(foundtbl);
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
                var products = from m in obj.Losttbls
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


            var movies = from m in obj.Losttbls
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
        public ActionResult MatchedproductsToplost(string sortOrder, string title, int? Id)
        {
            string Title = TempData["Titletl"].ToString();
            string Location = TempData["Locationtl"].ToString();
            string SubLocation = TempData["SubLocationtl"].ToString();
            string Details = TempData["Detailstl"].ToString();
            string SubCategory = TempData["SubCategorytl"].ToString();


            if (TempData["Uniuekey"] == null)
            {
                var products = from m in obj.Foundtbls
                               where ((m.SubCategory1.SubCategoryName.Contains(SubCategory)) && (m.Title.Contains(Title) || m.Location1.LocationName.Contains(Location) || m.SubLocation1.SubLocationName.Contains(SubLocation) || m.Details.Contains(Details)))
                               select m;
                return View(products.ToList());

                //   return RedirectToAction("NotFound");
            }


            int uniuekey = (int)TempData["Uniuekey"];


            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            /*     var movies = from m in db.AllProductsTbls
                              where  ((m.SubCategory1.SubCategoryName.Contains(SubCategory))&&(m.Title.Contains(Title)||m.Location1.LocationName.Contains(Location) || m.SubLocation1.SubLocationName.Contains(SubLocation)|| m.Details.Contains(Details)))
                              select m;
                              */


            var movies = from m in obj.Foundtbls
                         where (m.UniqueKey == uniuekey)
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
        [Authorize]

        // GET: FoundProducts/Create
        public ActionResult CreateTopLost()
        {
            ViewBag.username = User.Identity.Name;
            //  ViewBag.username = (string)Session["username"];
            //  ViewBag.username = (from u in db.LoginTbls
            //           select u.RegisterTbl.UserName).Max();
            //   ViewBag.cellno = (from u in db.LoginTbls
            //   select u.RegisterTbl.ContactNo).Max();
            var lastrecord = (from c in obj.LoginTbls orderby c.Id descending select c).First();
            ViewBag.cellno = lastrecord.RegisterTbl.ContactNo;
            ViewBag.address = lastrecord.RegisterTbl.Address;
            bindCategory();
            bindLocation();
            return View();
        }


        // POST: Foundstbls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateTopLost(AdminApprovalTbl approve)
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
                approve.CategoryId = Int32.Parse(approve.Category);
                approve.SubCategoryId = Int32.Parse(approve.SubCategory);

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
                obj.AdminApprovalTbls.Add(approve);

                //   db.Foundtbls.Add(loststbl);
                //   db.AllProductsTbls.Add(losttbl);
                try
                {
                    obj.SaveChanges();
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
        public ActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AjaxMethod()
        {
            string query = "SELECT (Location.LocationName) TotalLo, COUNT(AllProductsTbl.Id) TotalId";
            query += " FROM AllProductsTbl,Location WHERE Location.LocationId = AllProductsTbl.LocationId GROUP BY Location.LocationName";
             
                string constr = "Data Source=lofdbserver.database.windows.net;Initial Catalog=Lofdb1;Integrated Security=True";
                List<object> chartData = new List<object>();
                chartData.Add(new object[]
                                {
                            "Location.LocationName", "TotalId"
                                });
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                chartData.Add(new object[]
                                {
                            sdr["TotalLo"], sdr["TotalId"]
                                });
                            }
                        }

                        con.Close();
                    }
                }

                return Json(chartData);
            } 
    }
}
