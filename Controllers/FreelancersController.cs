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
    public class FreelancersController : Controller
    {
        private Context db = new Context();

     

        // GET: Freelancers
        public  ActionResult Index()
        {
            var freelancers = db.Freelancers.Include(f => f.Resume);
           
            return View(freelancers.ToList());
        }

        // GET: Freelancers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = await db.Freelancers.FindAsync(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }


            ViewBag.Skills = freelancer.Skills;

            return View(freelancer);
        }

        // GET: Freelancers/Create
        public ActionResult Create()
        {
            Freelancer freelancer =  new Freelancer();
            ViewBag.ResumeId = new SelectList(db.Resumes, "Id", "Text");
            return View("Create",freelancer);
        }

        // POST: Freelancers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Surname,Email,Password,ResumeId")] Freelancer freelancer)
        {
            if (ModelState.IsValid)
            {
                db.Freelancers.Add(freelancer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ResumeId = new SelectList(db.Resumes, "Id", "Text", freelancer.ResumeId);
            return View(freelancer);
        }

        // GET: Freelancers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = await db.Freelancers.FindAsync(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Skills = db.Skills.ToList();
            ViewBag.ResumeId = new SelectList(db.Resumes, "Id", "Text", freelancer.ResumeId);
            return View(freelancer);
        }





        // POST: Freelancers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Email,Password,ResumeId")] Freelancer freelancer, int[] selectedCourses)
        {

            Freelancer newStudent = db.Freelancers.Find(freelancer.Id);
            newStudent.Name = freelancer.Name;
            newStudent.Surname = freelancer.Surname;
            newStudent.Email = freelancer.Email;
            newStudent.Password = freelancer.Password;
            newStudent.Resume = freelancer.Resume;
            newStudent.ResumeId = freelancer.ResumeId;

            newStudent.Skills.Clear();
            if (selectedCourses != null)
            {
                
                //получаем выбранные курсы
                foreach (var c in db.Skills.Where(co => selectedCourses.Contains(co.Id)))
                {
                    newStudent.Skills.Add(c);
                }
               
            }

            ViewBag.ResumeId = new SelectList(db.Resumes, "Id", "Text", freelancer.ResumeId);
         


            db.Entry(newStudent).State = EntityState.Modified;
            db.SaveChanges();
                
                return RedirectToAction("Index");
           


        }
        
        // GET: Freelancers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = await db.Freelancers.FindAsync(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // POST: Freelancers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Freelancer freelancer = await db.Freelancers.FindAsync(id);
            db.Freelancers.Remove(freelancer);
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
