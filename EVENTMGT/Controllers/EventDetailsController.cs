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
    public class EventDetailsController : Controller
    {
        private EVENTEntities1 db = new EVENTEntities1();

        // GET: EventDetails
        public ActionResult Index()
        {
            var eventDetails = db.EventDetails.Include(e => e.EventType).Include(e => e.Venue1);
            return View(eventDetails.ToList());
        }

        // GET: EventDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDetail eventDetail = db.EventDetails.Find(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }

        // GET: EventDetails/Create
        public ActionResult Create()
        {
            ViewBag.type = new SelectList(db.EventTypes, "id", "typeEvent");
            ViewBag.venue = new SelectList(db.Venues, "id", "place");
            return View();
        }

        // POST: EventDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,eventName,eventDescription,venue,eventDate,eventTime,type")] EventDetail eventDetail)
        {
            if (ModelState.IsValid)
            {
                db.EventDetails.Add(eventDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.type = new SelectList(db.EventTypes, "id", "typeEvent", eventDetail.type);
            ViewBag.venue = new SelectList(db.Venues, "id", "place", eventDetail.venue);
            return View(eventDetail);
        }

        // GET: EventDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDetail eventDetail = db.EventDetails.Find(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.type = new SelectList(db.EventTypes, "id", "typeEvent", eventDetail.type);
            ViewBag.venue = new SelectList(db.Venues, "id", "place", eventDetail.venue);
            return View(eventDetail);
        }

        // POST: EventDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,eventName,eventDescription,venue,eventDate,eventTime,type")] EventDetail eventDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type = new SelectList(db.EventTypes, "id", "typeEvent", eventDetail.type);
            ViewBag.venue = new SelectList(db.Venues, "id", "place", eventDetail.venue);
            return View(eventDetail);
        }

        // GET: EventDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDetail eventDetail = db.EventDetails.Find(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }

        // POST: EventDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventDetail eventDetail = db.EventDetails.Find(id);
            db.EventDetails.Remove(eventDetail);
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
