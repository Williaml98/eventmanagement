using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using EVENTMGT.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EVENTMGT.Controllers
{
    public class AttendeesController : Controller
    {
        private EVENTEntities1 db = new EVENTEntities1();

        // GET: Attendees
        // Display all data in the table without search
        /*public ActionResult Index()
        {
            return View(db.Attendees.ToList());
        }*/

        // GET: Attendees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // Display all data in the table with search
        public ActionResult Index(string searchString)
        {
            var attendees = from a in db.Attendees
                            select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Search by name and display in the db
                attendees = attendees.Where(a => a.fullName.Contains(searchString));
            }

            return View(attendees.ToList());
        }



        // GET: Attendees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attendees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fullName,phone,email,address")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Attendees.Add(attendee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attendee);
        }

        // GET: Attendees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fullName,phone,email,address")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attendee);
        }

        // GET: Attendees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendee attendee = db.Attendees.Find(id);
            db.Attendees.Remove(attendee);
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


        public ActionResult GeneratePDF(string searchString)
        {
            var attendees = from a in db.Attendees
                            select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                attendees = attendees.Where(a => a.fullName.Contains(searchString));
            }

            var filteredAttendees = attendees.ToList();



            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);

            // Open the document to enable writing
            document.Open();

            // Create a table with 4 columns
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            // Add table headers
            table.AddCell("Full Name");
            table.AddCell("Phone");
            table.AddCell("Email");
            table.AddCell("Address");

            // Add attendee data to the table
            foreach (var attendee in filteredAttendees)
            {
                table.AddCell(attendee.fullName);
                table.AddCell(attendee.phone);
                table.AddCell(attendee.email);
                table.AddCell(attendee.address);
            }

            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();

            // Create a new MemoryStream from the buffer
            byte[] pdfData = workStream.ToArray();
            MemoryStream newStream = new MemoryStream(pdfData);

            // Return the PDF as a file stream
            return new FileStreamResult(newStream, "application/pdf");
        }



        /*public ActionResult GeneratePDF()
        {
            var attendees = db.Attendees.ToList();
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);

            // Open the document to enable writing
            document.Open();

            // Create a table with 5 columns
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            // Add table headers
            table.AddCell("Full Name");
            table.AddCell("Phone");
            table.AddCell("Email");
            table.AddCell("Address");
            //table.AddCell("Actions");

            // Add attendee data to the table
            foreach (var attendee in attendees)
            {
                table.AddCell(attendee.fullName);
                table.AddCell(attendee.phone);
                table.AddCell(attendee.email);
                table.AddCell(attendee.address);
               // table.AddCell("Edit, Details, Delete");
            }

            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();

            // Create a new MemoryStream from the buffer
            byte[] pdfData = workStream.ToArray();
            MemoryStream newStream = new MemoryStream(pdfData);

            // Return the PDF as a file stream
            return new FileStreamResult(newStream, "application/pdf");
        }*/

    }
}
