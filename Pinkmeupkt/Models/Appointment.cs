using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pinkmeupkt.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Time of appointment")]
        public DateTime startTime { get; set; }
        public string startTimeString { get; set; }
        [DisplayName("Time of booking")]
        public DateTime bookTime { get; set; }
        public string bookTimeString { get; set; }
        [DisplayName("Booked")]
        public bool isBooked { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int OfferId { get; set; }
        public Offer offer { get; set; }
    }
}