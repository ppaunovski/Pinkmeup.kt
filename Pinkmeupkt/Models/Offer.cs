using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public string Title { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Giftcard> Giftcards { get; set; }
    }
}