using DownRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
using System.Data.Entity;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Net;

namespace DownRest.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        UserContext db = new UserContext();
 
        [HttpGet]
        public ActionResult Admin()
        {
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (user.role == "admin")

            return View("Admin");
            else
                return   RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Profile()
        {
           
            var store = new UserStore<User>(new UserContext());
            var userManager = new UserManager<User>(store);
            User user = userManager.FindByNameAsync(User.Identity.Name).Result;
            ViewBag.Projects = db.Projects.Include(p=>p.User).ToList();
            ViewBag.Responses = db.Responses.ToList();
            ViewBag.Skills = db.Skills.ToList();
            return View("Profile",user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile([Bind(Include = "Id,balance")] User user, int[] selectedCourses)
        {

            User newStudent = db.Users.Find(user.Id);
           
            newStudent.balance = user.balance;
             

            newStudent.Skills.Clear();

            if (selectedCourses != null)
            {

          
                foreach (var c in db.Skills.Where(co => selectedCourses.Contains(co.Id)))
                {
                    newStudent.Skills.Add(c);
                }

            }

          

            db.Entry(newStudent).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Profile");



        }

        public async Task<ActionResult> DetailsProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = null;
            var projects =  db.Projects.Include(p => p.Cat).Include(p => p.User).Include(i => i.Responses.Select(it => it.User));
            foreach (Project p in projects) {
                if (p.Id == id) {
                    project = p; 

                }
            }


            if (project != null)
            {
                ViewBag.responses = project.Responses;
                return View("DetailsP", project);
            }

            return View("Index");
        }
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {

            if (User.Identity.IsAuthenticated)
            {
                if (upload != null)
                {
                    var store = new UserStore<User>(new UserContext());
                    var userManager = new UserManager<User>(store);
                    User user = userManager.FindByNameAsync(User.Identity.Name).Result;
                    User user2 = db.Users.Find(user.Id);
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    string h = GetStringSha256Hash(fileName);
                    upload.SaveAs(Server.MapPath("~/Files/" + h +".jpg"));

                    
                  

                    user2.avatar = h + ".jpg";

                    db.Entry(user2).State = EntityState.Modified;

                    db.SaveChanges();

                }
                return RedirectToAction("Profile");
            }
            else {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult code404()
        {

            return new HttpStatusCodeResult(404);
        }
         

        [HttpGet]
        public JsonResult CheckReward(string name)
        {
            var result = !(Convert.ToInt32(name) >= 0);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //CREATE

        [HttpGet]
        public ActionResult Create()
        {
            SelectList teams = new SelectList(db.Resumes, "Id", "Text");
            ViewBag.Resumes = teams;
            return View( );
        }
        [HttpPost]
        public ActionResult Create(Freelancer book)
        {

            db.Freelancers.Add(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

       
        public ActionResult AcceptResponse(int id, string userId, int projId)
        {



            Project p = db.Projects.Find(projId);
            p.AcceptedBy = id;

            
            User user2 = db.Users.Find(userId);
            // получаем имя файла




            user2.balance = user2.balance + p.Reward;

            db.Entry(user2).State = EntityState.Modified;
 
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("DetailsProject", "Home", new { id = projId });
        }

        //READ
        public ActionResult Index()
        {

            var cats = db.Categories.Include(p => p.Projects.Select(po => po.User));
            

            return View(cats);
             
    
        }

        //DELETE

        public ActionResult Delete(int id)
        {
            Freelancer b = db.Freelancers.Find(id);
            if (b != null)
            {
                db.Freelancers.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //UPDATE
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
             
            Freelancer f = db.Freelancers.Find(id);
            if (f != null)
            {
               
                return View(f);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Freelancer f)
        {
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
    }
}