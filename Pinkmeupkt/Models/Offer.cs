using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pinkmeupkt.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public double Duration { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}