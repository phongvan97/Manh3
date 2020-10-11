using LamDep.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LamDep.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var randomPost = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved).OrderBy(p => Guid.NewGuid()).Take(9).ToList();
            var categories = db.Categories.Where(c => !c.IsDeleted && c.IsActive).ToList();
            var listPostInCategory = new List<Post>();
            foreach (var cate in categories)
            {
                listPostInCategory=listPostInCategory.Concat(db.Posts.Where(p => p.CategoryId == cate.CategoryId
                                                        && !p.IsDeleted && p.IsActive && p.IsApproved)
                                                        .OrderBy(p => Guid.NewGuid())
                                                        .Take(10)).ToList();
            }

            var mostView = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved).OrderByDescending(p => p.ViewCount).Take(10).ToList();
            var latest = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved).OrderByDescending(p => p.CreateDate).Take(10).ToList();
            ViewBag.latest = latest;
            ViewBag.mostView = mostView;
            ViewBag.categories = categories;
            ViewBag.randomPost = randomPost;
            ViewBag.listPostInCategory = listPostInCategory;
            return View();
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
        public ActionResult LoadCategories()
        {
            var cateList = db.Categories.Where(c => !c.IsDeleted && c.IsActive).ToList();

            return PartialView(cateList);
        }
    }
}