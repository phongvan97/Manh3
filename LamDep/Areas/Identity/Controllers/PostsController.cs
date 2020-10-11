using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LamDep.Models;
using Microsoft.AspNet.Identity;

namespace LamDep.Areas.Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Identity/Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author).Include(p=>p.Category).Where(p => !p.IsDeleted);
            return View(posts.ToList());
        }

        // GET: Identity/Posts/Details/5
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

        // GET: Identity/Posts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email");
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive && !c.IsDeleted), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Identity/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase myfile)
        {
            if (myfile != null && myfile.ContentLength > 0)
            {
                string imgName = Path.GetFileName(myfile.FileName);
                string imgExt = Path.GetExtension(imgName);
                if (imgExt.Equals(".jpg") || imgExt.Equals(".jpeg") || imgExt.Equals(".png"))
                {
                    string imgPath = Path.Combine(Server.MapPath("~/Assets/userImage"), imgName);
                    myfile.SaveAs(imgPath);
                    post.Image = "/Assets/userImage/" + imgName;
                }
                else
                {
                    ModelState.AddModelError("", "Sai loại tệp");
                }
            }
            if (ModelState.IsValid)
            {
                var id = this.User.Identity.GetUserId();
                post.AuthorId = id;
                post.CreateDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive && !c.IsDeleted), "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Identity/Posts/Edit/5
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
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive && !c.IsDeleted), "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Identity/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase myfile)
        {
            if (myfile != null && myfile.ContentLength > 0)
            {
                string imgName = Path.GetFileName(myfile.FileName);
                string imgExt = Path.GetExtension(imgName);
                if (imgExt.Equals(".jpg") || imgExt.Equals(".jpeg") || imgExt.Equals(".png"))
                {
                    string imgPath = Path.Combine(Server.MapPath("~/Assets/userImage"), imgName);
                    myfile.SaveAs(imgPath);
                    post.Image = "/Assets/userImage/" + imgName;
                }
                else
                {
                    ModelState.AddModelError("", "Wrong file type");
                }
            }
            if (ModelState.IsValid)
            {
                var old = db.Posts.Find(post.PostId);
                old.Image = post.Image;
                old.Title = post.Title;
                old.Content = post.Content;
                old.UpdatedDate = DateTime.Now;
                old.IsActive = post.IsActive;
                old.IsApproved = post.IsApproved;
                old.CategoryId = post.CategoryId;
                db.Entry(old).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", post.AuthorId);
            return View(post);
        }

        // GET: Identity/Posts/Delete/5
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

        // POST: Identity/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            post.IsDeleted = true;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
