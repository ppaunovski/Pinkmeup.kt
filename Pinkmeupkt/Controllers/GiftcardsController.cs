using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Pinkmeupkt.Models;

namespace Pinkmeupkt.Controllers
{
    [Authorize]
    public class GiftcardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Giftcards
        public ActionResult Index()
        {
            if(!User.IsInRole(Roles.Admin)) return RedirectToAction("Create");

            var gifts = db.Gifts.Include(g => g.Offer);
            return View(gifts.ToList());
        }

        // GET: Giftcards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giftcard giftcard = db.Gifts.Find(id);
            if (giftcard == null)
            {
                return HttpNotFound();
            }
            return View(giftcard);
        }

        // GET: Giftcards/Create
        public ActionResult Create()
        {
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title");
            return View();
        }


        public ActionResult Redeem(int id)
        {
            var giftcard = db.Gifts.Find(id);

            return View(giftcard);
        }

        // POST: Giftcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Giftcard giftcard)
        {
            //if (ModelState.IsValid)
            //{
                giftcard.TimeOfPurchase = DateTime.Now;
                giftcard.AppointmentTime = DateTime.Now;
                db.Gifts.Add(giftcard);
                db.SaveChanges();


            string Body = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Pictures/htmlEmail/index.html"));

            Body = Body.Replace("#Name#", giftcard.Name + giftcard.Surname);
            Body = Body.Replace("#RedeemURL#", "https://localhost:44333/Giftcards/Redeem/" + giftcard.Id);


            if (giftcard.SendRecipientEmail)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(giftcard.Email);
                mail.From = new MailAddress("contact.pinkmeup.mail@gmail.com");
                mail.Subject = "A Thoughtful Gift for You: Pinkmeup Beauty Experience 🎁";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("contact.pinkmeup.mail@gmail.com", "jvrypqhtjqhlnbeg");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }

            MailMessage mail1 = new MailMessage();
            mail1.To.Add(User.Identity.Name);
            mail1.From = new MailAddress("contact.pinkmeup.mail@gmail.com");
            mail1.Subject = "A Thoughtful Gift for You: Pinkmeup Beauty Experience 🎁";
            mail1.Body = Body;
            mail1.IsBodyHtml = true;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            SmtpClient smtp1 = new SmtpClient();
            smtp1.Host = "smtp.gmail.com";
            smtp1.Port = 587;
            smtp1.UseDefaultCredentials = false;
            smtp1.Credentials = new NetworkCredential("contact.pinkmeup.mail@gmail.com", "jvrypqhtjqhlnbeg");
            smtp1.EnableSsl = true;
            smtp1.Send(mail1);


            return RedirectToAction("Index");
            //}

            //ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", giftcard.OfferId);
            //return View(giftcard);
        }

        public JsonResult BookTime(int id, string startTime)
        {
            Giftcard gc = db.Gifts.Find(id);
            gc.AppointmentTime = DateTime.Parse(startTime);
            gc.AppointmentTimeString = gc.AppointmentTime.ToString("yyyy-MM-dd HH:mm:ss");

            db.Gifts.AddOrUpdate(gc);
            db.SaveChanges();

            return Json(gc.Id);
        }
        public ActionResult RedeemGift(Giftcard giftcard)
        {
            Appointment appointment = new Appointment();

            appointment.startTime = giftcard.AppointmentTime;
            appointment.bookTime = DateTime.Now;
            appointment.ApplicationUser = db.Users.Find(User.Identity.GetUserId());
            appointment.offer = giftcard.Offer;
            appointment.OfferId = giftcard.OfferId;
            appointment.bookTimeString = appointment.bookTime.ToString();
            appointment.isBooked = true;
            appointment.startTimeString = appointment.startTime.ToString();

            db.Appointments.Add(appointment);
            db.SaveChanges();

            ViewBag.Name = giftcard.Name;

            return View(appointment);
        }


        // GET: Giftcards/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giftcard giftcard = db.Gifts.Find(id);
            if (giftcard == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", giftcard.OfferId);
            return View(giftcard);
        }

        // POST: Giftcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Phone,Email,SendRecipientEmail,TimeOfPurchase,Message,OfferId")] Giftcard giftcard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giftcard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfferId = new SelectList(db.Offers, "Id", "Title", giftcard.OfferId);
            return View(giftcard);
        }

        // GET: Giftcards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giftcard giftcard = db.Gifts.Find(id);
            if (giftcard == null)
            {
                return HttpNotFound();
            }
            return View(giftcard);
        }

        // POST: Giftcards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Giftcard giftcard = db.Gifts.Find(id);
            db.Gifts.Remove(giftcard);
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
