using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LamDep.Models;

namespace LamDep.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ViewDetail(int? postId)
        {
            var model = db.Posts.Find(postId);
            var sameCategory = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved && p.CategoryId == model.CategoryId).ToList();
            var mostView = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved).OrderByDescending(p => p.ViewCount).Take(10).ToList();
            var latest = db.Posts.Where(p => !p.IsDeleted && p.IsActive && p.IsApproved).OrderByDescending(p => p.CreateDate).Take(10).ToList();
            var categories = db.Categories.Include(c=>c.Posts).Where(c => !c.IsDeleted && c.IsActive).ToList();
            ViewBag.categories = categories;
            ViewBag.latest = latest;
            ViewBag.sameCategory = sameCategory;
            ViewBag.mostView = mostView;

            return View(model);
        }
        public ActionResult SearchByCategory(int? categoryId)
        {
            return View("ListPost");
        }
    }
}
