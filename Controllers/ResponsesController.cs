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
    public class ResponsesController : Controller
    {
        private Context db = new Context();

        // GET: Responses
        public async Task<ActionResult> Index()
        {
            var responses = db.Responses.Include(r => r.User).Include(r => r.Project);
            return View(await responses.ToListAsync());
        }

        // GET: Responses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = await db.Responses.FindAsync(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // GET: Responses/Create
        public ActionResult Create()
        {
            Response r = new Response();
            ViewBag.FreelancerId = new SelectList(db.Freelancers, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return PartialView("Create", r);
        }

        // POST: Responses/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Text")] Response response, int ProjectId)
        {
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            response.ProjectId = ProjectId;
            response.UserId = user.Id;

            if (ModelState.IsValid)
            {
                db.Responses.Add(response);
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsProject", "Home", new { id = ProjectId });
            }

           
            return View(response);
        }

        // GET: Responses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = await db.Responses.FindAsync(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            ViewBag.FreelancerId = new SelectList(db.Freelancers, "Id", "Name", response.UserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", response.ProjectId);
            return View(response);
        }

        // POST: Responses/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Text,ProjectId,FreelancerId")] Response response)
        {
            if (ModelState.IsValid)
            {
                db.Entry(response).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FreelancerId = new SelectList(db.Freelancers, "Id", "Name", response.UserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", response.ProjectId);
            return View(response);
        }

        // GET: Responses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = await db.Responses.FindAsync(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // POST: Responses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Response response = await db.Responses.FindAsync(id);
            db.Responses.Remove(response);
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
