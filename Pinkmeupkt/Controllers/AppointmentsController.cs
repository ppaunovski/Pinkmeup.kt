using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pinkmeupkt.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace Pinkmeupkt.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index()
        {
            var appointments = db.Appointments.Include(a => a.offer);
            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        //[Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            var apps = db.Appointments.Include(a => a.ApplicationUser).Include(a => a.offer);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            foreach(var ap in apps)
            {
                if (ap.Id == id)
                    return View(ap);
            }
            return View(appointment);
        }

        [AllowAnonymous]
        public ActionResult Calendar()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetOfferInJSON(int id)
        {
            var offer = db.Offers.Where(o => o.Id == id);
            return new JsonResult { Data = offer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [AllowAnonymous]
        public JsonResult GetAppointments()
        {
            var appointments = db.Appointments.Include(a => a.offer).Where(a => a.startTime.CompareTo(DateTime.Now) >= 0).ToList();
            List<AppointmentDBO> list = new List<AppointmentDBO>();

            foreach (var a in appointments)
            {
                AppointmentDBO appointmentDBO = new AppointmentDBO();
                appointmentDBO.startTime = a.startTime.ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                appointmentDBO.endTime = a.startTime.AddHours(a.offer.Duration).ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                appointmentDBO.bookTime = a.bookTime != null ? a.bookTime.ToString("yyyy-MM-dd HH:mm:ss") + ".000" : null;
                appointmentDBO.Price = a.offer.Price;
                appointmentDBO.Description = a.offer.Description;
                appointmentDBO.Title = a.offer.Title;
                appointmentDBO.isBooked = a.isBooked;
                appointmentDBO.Id = a.Id;

                list.Add(appointmentDBO);
            }

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,Customer")]
        public JsonResult CreateAppointment (string startTime)
        {
            DateTime start = DateTime.Parse(startTime);
            PartialAppointment partial = new PartialAppointment();
            partial.startTime = start;
            partial.startTimeString = start.ToString("yyyy-MM-dd HH:mm:ss");
            partial.bookTime = DateTime.Now;
            partial.bookTimeString = partial.bookTime.ToString("yyyy-MM-dd HH:mm:ss");
            db.PartialAppointments.Add(partial);
            db.SaveChanges();

            return Json(partial.Id);
        }

        [HttpGet]
        public JsonResult RequestForUser (string user)
        {
            string email = user.Replace("#", "@");
            List<List<string>> apps = new List<List<string>> ();
            var app = db.Appointments.Where(a => a.ApplicationUser.Email == email).Include(a => a.offer).ToList();
            foreach(var ap in app)
            {
                List<string> list = new List<string> ();
                list.Add(ap.offer.Title);
                list.Add(ap.startTimeString);
                list.Add(ap.Id.ToString());
                apps.Add(list);
            }

            return Json(apps, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,Moderator,Customer")]
        public ActionResult BookAppointment (int id)
        {
            //2023-08-03T12:00:00+02:00
            var partial = db.PartialAppointments.Find(id);

            ViewBag.OfferId = new SelectList(db.Offers.Where(o => o.Title != "Слободен термин"), "Id", "Title");

            return View(partial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator,Customer")]
        public ActionResult BookAppointment(PartialAppointment PartialAppointment)
        {
            Appointment app = new Appointment();
            app.startTime = PartialAppointment.startTime;
            app.startTimeString = PartialAppointment.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            app.bookTime = DateTime.Now;
            app.bookTimeString = PartialAppointment.bookTime.ToString("yyyy-MM-dd HH:mm:ss");
            app.offer = db.Offers.Find(PartialAppointment.OfferId);
            app.OfferId = PartialAppointment.OfferId;
            app.isBooked = true;
            app.ApplicationUser = db.Users.Find(User.Identity.GetUserId());

            db.Appointments.Add(app);
            db.SaveChanges();
            return RedirectToAction("Calendar");
        }

        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Book(int id)
        {
            var model = db.Appointments.Find(id);
            ViewBag.OfferId = new SelectList(db.Offers.Where(o => o.Description != "Слободен термин"), "Id", "Title", model.OfferId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Book(Appointment appointment)
        {
            Appointment app = new Appointment();
            app.Id = appointment.Id;
            app.startTime = appointment.startTime;
            app.startTimeString = appointment.startTime.ToString("yyyy-MM-dd HH:mm:ss");
            app.offer = appointment.offer;
            app.OfferId = appointment.OfferId;
            app.isBooked = false;

            db.Appointments.Add(app);

            db.SaveChanges();
            return RedirectToAction("Calendar");
        }


        // GET: Appointments/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title");
            return View();
        }



        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,startTime,startTimeString,bookTime,bookTimeString,isBooked,OfferId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                Appointment app = new Appointment();
                app.Id = appointment.Id;
                app.startTime = appointment.startTime;
                app.startTimeString = appointment.startTime.ToString("yyyy-MM-dd HH:mm:ss");
                app.offer = appointment.offer;
                app.OfferId = appointment.OfferId;
                app.isBooked = false;

                db.Appointments.Add(app);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", appointment.OfferId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", appointment.OfferId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,startTime,startTimeString,bookTime,bookTimeString,isBooked,OfferId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", appointment.OfferId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        //[Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            db.Appointments.Remove(appointment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Giftcard()
        {

            return View();
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
