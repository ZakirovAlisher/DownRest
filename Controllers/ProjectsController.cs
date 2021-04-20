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
using Telegram.Bot;
using Telegram.Bot.Args;

namespace DownRest.Controllers
{
    public class ProjectsController : Controller
    {
       
        private UserContext db = new UserContext();
        
        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Cat).Include(p => p.User);
            return View("Index", projects.ToList());
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project != null)
            {
                return PartialView("Details", project);
            }
            return View("Index");
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            Project p = new Project();
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            return PartialView("Create",p);
           // return View(p);
        }

        // POST: Projects/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Reward,CatId")] Project project)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var store = new UserStore<User>(new UserContext());
                    var userManager = new UserManager<User>(store);
                    User user = userManager.FindByNameAsync(User.Identity.Name).Result;
                    User user2 = db.Users.Find(user.Id);

                    if (user2.balance >= project.Reward)
                    {
                        user2.balance = user2.balance - project.Reward;
                        project.UserId = user.Id;


                        db.Projects.Add(project);
                        db.Entry(user2).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Home", new { area = "" });



                    }
                    else {
                         
                        return RedirectToAction("Profile", "Home", new { error = "Not enough balance" }); }






                   
                }
                else {
                    return new HttpUnauthorizedResult();
                }


            }

            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", project.CatId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", project.UserId);
            return View(project);
        }
      
        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project != null)
            {
                ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", project.CatId);
                ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", project.UserId);
                return PartialView("Edit", project);
            }
           
            return View("Index");
        }

        // POST: Projects/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Reward,ClientId,CatId")] Project project)
        {
            ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", project.CatId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", project.UserId);
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project != null)
            {
                 
                return PartialView("Delete", project);
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
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
