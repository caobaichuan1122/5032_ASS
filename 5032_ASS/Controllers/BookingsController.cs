using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _5032_ASS.Email;
using _5032_ASS.AutoEmail;
using _5032_ASS.Models;
using Microsoft.AspNet.Identity;
using System.Text;

namespace _5032_ASS.Controllers
{
    public class BookingsController : Controller
    {
        private BookModel db = new BookModel();

        // GET: Bookings
        [Authorize]
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Clinic);
            return View(bookings.ToList());
        }

        // GET: Bookings/BookingTables
        public ActionResult BookingTables()
        {
            if (User.IsInRole("admin"))
            {
                var booking = db.Bookings.Include(b => b.Clinic);
                return View(booking.ToList());
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var bookings = db.Bookings.Where(b => b.User_Id == userId);
                return View(bookings.ToList());
            }

        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create(String date)
        {
            Booking booking = new Booking();
            if (date == null)
            {
                return RedirectToAction("BookingTables");
            }
            DateTime bookingDate = Convert.ToDateTime(date);
            booking.Booking_Date = bookingDate;
            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name");
            return View(booking);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Booking_Name,Booking_Email,Booking_Phone,Booking_Date,Booking_Description,Clinic_Id")] Booking booking)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HttpUtility.HtmlEncode(booking.Booking_Description));

            sb.Replace("<b>", "<b>");
            sb.Replace("</b>", "</b>");
            booking.Booking_Description = sb.ToString();

            string strDes = HttpUtility.HtmlEncode(booking.Booking_Description);
            booking.Booking_Description = strDes;

            DateTime dateTime = DateTime.Now;
            if (DateTime.Compare(booking.Booking_Date,dateTime) < 0) 
            {
                return Content("<Script>alert('booking date cannot eariler now');window.location.href = 'Index'</Script>");
            }
            var bookinglist = db.Bookings.Where(b=>b.Clinic_Id == booking.Clinic_Id).ToList();
            foreach (var bk in bookinglist)
            {
                if (DateTime.Compare(booking.Booking_Date, bk.Booking_Date) < 0)
                {
                    return Content("<Script>alert('The time has been booked!');window.location.href = 'Index'</Script>");
                }
            }
            String date = booking.Booking_Date.ToShortDateString();
            EmailAsync(booking.Booking_Email, date);
            booking.User_Id = User.Identity.GetUserId();
            ModelState.Clear();
            TryValidateModel(booking);

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("BookingTables");
            }

            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name", booking.Clinic_Id);
            return View(booking);
        }

        public ActionResult Rating(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name", booking.Clinic_Id);
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rating([Bind(Include = "Id,Booking_Name,Booking_Email,Booking_Phone,Booking_Date,Booking_Description,Booking_Rating,User_Id,Clinic_Id")] Booking booking)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HttpUtility.HtmlEncode(booking.Booking_Description));

            sb.Replace("<b>", "<b>");
            sb.Replace("</b>", "</b>");
            booking.Booking_Description = sb.ToString();

            string strDes = HttpUtility.HtmlEncode(booking.Booking_Description);
            booking.Booking_Description = strDes;

            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookingTables");
            }
            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name", booking.Clinic_Id);
            return View(booking);
        }


        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name", booking.Clinic_Id);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Booking_Name,Booking_Email,Booking_Phone,Booking_Date,Booking_Description,Booking_Rating,User_Id,Clinic_Id")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookingTables");
            }
            ViewBag.Clinic_Id = new SelectList(db.Clinics, "Id", "Clinic_Name", booking.Clinic_Id);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("BookingTables");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> EmailAsync(String email,String date)
        {
            autoEmailSender aes = new autoEmailSender();
            await aes.SendAsync(email,date);
            return null;
        }
    }
}
