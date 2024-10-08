using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EVENTMGT.Models;

namespace EVENTMGT.Controllers
{
    public class EventRegistrationsController : Controller
    {
        private EVENTEntities1 db = new EVENTEntities1();

        // GET: EventRegistrations
        public ActionResult Index()
        {
            var eventRegistrations = db.EventRegistrations.Include(e => e.Attendee1).Include(e => e.EventDetail);
            return View(eventRegistrations.ToList());
        }

        // GET: EventRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Create
        public ActionResult Create()
        {
            ViewBag.attendee = new SelectList(db.Attendees, "id", "fullName");
            ViewBag.eventName = new SelectList(db.EventDetails, "id", "eventName");
            return View();
        }

        // POST: EventRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,eventName,attendee")] EventRegistration eventRegistration)
        {
            if (ModelState.IsValid)
            {
                db.EventRegistrations.Add(eventRegistration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.attendee = new SelectList(db.Attendees, "id", "fullName", eventRegistration.attendee);
            ViewBag.eventName = new SelectList(db.EventDetails, "id", "eventName", eventRegistration.eventName);
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.attendee = new SelectList(db.Attendees, "id", "fullName", eventRegistration.attendee);
            ViewBag.eventName = new SelectList(db.EventDetails, "id", "eventName", eventRegistration.eventName);
            return View(eventRegistration);
        }

        // POST: EventRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,eventName,attendee")] EventRegistration eventRegistration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventRegistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.attendee = new SelectList(db.Attendees, "id", "fullName", eventRegistration.attendee);
            ViewBag.eventName = new SelectList(db.EventDetails, "id", "eventName", eventRegistration.eventName);
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            return View(eventRegistration);
        }

        // POST: EventRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            db.EventRegistrations.Remove(eventRegistration);
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
