using Pinkmeupkt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Pinkmeupkt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            string rectangle = Server.MapPath("~/Pictures/insta_pics/rectangle");
            string square = Server.MapPath("~/Pictures/insta_pics/square");
            string[] images_rec = Directory.GetFiles(rectangle);
            string[] images_square = Directory.GetFiles(square);

            ViewBag.Rectangle = images_rec;
            ViewBag.Square = images_square;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(EmailModel emailModel)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("contact.pinkmeup.mail@gmail.com");
            mail.From = new MailAddress("contact.pinkmeup.mail@gmail.com");
            mail.Subject = "Message sent from: " + emailModel.Email;
            mail.Body = emailModel.Message + '\n' + "Mail from: " + emailModel.Email;
            mail.IsBodyHtml = true;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("contact.pinkmeup.mail@gmail.com", "jvrypqhtjqhlnbeg");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            ViewBag.Message = "Thanks for contacting us!";

            return View();
        }
    }
}