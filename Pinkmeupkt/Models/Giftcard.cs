using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pinkmeupkt.Models
{
    public class Giftcard
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [DisplayName("Send email to recipient")]
        public bool SendRecipientEmail { get; set; }
        public DateTime TimeOfPurchase { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentTimeString { get; set; }
        public string Message { get; set; }
        public ApplicationUser Buyer { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}