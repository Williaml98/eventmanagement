using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EVENTMGT.Models;

namespace EVENTMGT.Controllers
{
    public class RegistrationsController : Controller
    {
        private EVENTEntities2 db = new EVENTEntities2();

        // GET: Registrations
        public ActionResult Index()
        {
            return View(db.Registrations.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        // GET: Registrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registrations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,names,email,pwd")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                // Hash the password
                registration.pwd = encrytPass(registration.pwd);

                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registration);
        }


        // POST: Registrations/Login
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Login(string email, string password)
         {
             // Hash the password
             string hashedPassword = encrytPass(password);

             // Check if the email and hashed password exist in the database
             var user = db.Registrations.FirstOrDefault(u => u.email == email && u.pwd == hashedPassword);

             if (user != null)
             {
                 // Login successful
                 // Create a session and store the user's information
                 Session["UserID"] = user.id;
                 Session["UserName"] = user.names;
                 Session["UserEmail"] = user.email;

                 // Redirect to a desired page or action
                 return RedirectToAction("Index", "Home");
             }
             else
             {
                 // Login failed
                 ModelState.AddModelError("", "Invalid email or password");
                 return View();
             }
         }*/

        // POST: Registrations/Login
        // POST: Registrations/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string pwd)
        {

            System.Diagnostics.Debug.WriteLine($"Email: {email}");
            System.Diagnostics.Debug.WriteLine($"Password: {pwd}");
        
            // Hash the provided password
                    string hashedPassword = encrytPass(pwd);

            // Check if the email and hashed password exist in the database
            var user = db.Registrations.FirstOrDefault(u => u.email == email && u.pwd == hashedPassword);

            if (user != null)
            {
                // Login successful
                // Create a session and store the user's information
                Session["UserID"] = user.id;
                Session["UserName"] = user.names;
                Session["UserEmail"] = user.email;

                // Redirect to a desired page or action
                // return RedirectToAction("Index", "Home");
                return PartialView("_LoginHandler");
            }
            else
            {
                // Login failed
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }
        }

         /*private string encrytPass(string password)
         {

             // Hash computation
             using (SHA256 sha256Hash = SHA256.Create())
             {
                 byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                 StringBuilder sb = new StringBuilder();
                 for (int i = 0; i < bytes.Length; i++)
                 {
                     sb.Append(bytes[i].ToString("x2"));
                 }
                 return sb.ToString();
             }
         }*/

        private string encrytPass(string password)
        {
            // Check if the password is not null
            if (!string.IsNullOrEmpty(password))
            {
                // Hash computation
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            else
            {
                // Return null or handle the null case as per your requirement
                return null;
            }
        }


        // GET: Registrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,names,email,pwd")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
            db.SaveChanges();
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
