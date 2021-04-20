using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DownRest.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace DownRest.Controllers
{
    public class CategoriesController : Controller
    {
        private Context db = new Context();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            
            if (user.role == "admin")

                return View(await db.Categories.ToListAsync());
            else
                return RedirectToAction("Index");
           
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (user.role == "admin")

              return View(category);
            else
                return RedirectToAction("Index");
           
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            Category ct = new Category();
            return View("Create", ct);
        }

        // POST: Categories/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (user.role == "admin")

                return View(category);
            else
                return RedirectToAction("Index");
            
        }

        // POST: Categories/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (user.role == "admin")

                 return View(category);
            else
                return RedirectToAction("Index");
           
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
