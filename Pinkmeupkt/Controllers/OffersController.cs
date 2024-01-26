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
using System.IO;
using System.EnterpriseServices;

namespace Pinkmeupkt.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offers
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole(Roles.Admin))
            {
                return View(await db.Offers.ToListAsync());
            }

            return View("OurOffers", await db.Offers.ToListAsync());
        }

        // GET: Offers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(offer.ImageFile.FileName);
                string extension = Path.GetExtension(offer.ImageFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                
                offer.ImagePath = "~/Pictures/OffersPictures/" + fileName;

                fileName = Path.Combine(Server.MapPath("~/Pictures/OffersPictures/"), fileName);
                offer.ImageFile.SaveAs(fileName);

                db.Offers.Add(offer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        // GET: Offers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Offer offer)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(offer.ImageFile.FileName);
                string extension = Path.GetExtension(offer.ImageFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                offer.ImagePath = "~/Pictures/OffersPictures/" + fileName;

                fileName = Path.Combine(Server.MapPath("~/Pictures/OffersPictures/"), fileName);
                offer.ImageFile.SaveAs(fileName);

                db.Entry(offer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Offer offer = await db.Offers.FindAsync(id);
            db.Offers.Remove(offer);
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
