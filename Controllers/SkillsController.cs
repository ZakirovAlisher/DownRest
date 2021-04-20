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

namespace DownRest.Controllers
{
    public class SkillsController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Skills
        public async Task<ActionResult> Index()
        {
            return View(await db.Skills.ToListAsync());
        }

        // GET: Skills/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // GET: Skills/Create
        public ActionResult Create()
        {
            Skill s = new Skill();
            return View("Create",s);
        }

        // POST: Skills/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Skills.Add(skill);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        // GET: Skills/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            ViewBag.Freelancers = db.Freelancers.ToList();
            return View(skill);
        }

        // POST: Skills/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Skill skill, string[] selectedCourses)
        {


            Skill newSkill = db.Skills.Find(skill.Id);
            newSkill.Name = skill.Name;



            newSkill.Users.Clear();
            if (selectedCourses != null)
            {

                //получаем выбранные курсы
                foreach (var c in db.Users.Where(co => selectedCourses.Contains(co.Id)))
                {
                    newSkill.Users.Add(c);
                }

            }


            if (ModelState.IsValid)
            {
                db.Entry(newSkill).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        // GET: Skills/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = await db.Skills.FindAsync(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Skill skill = await db.Skills.FindAsync(id);
            db.Skills.Remove(skill);
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
