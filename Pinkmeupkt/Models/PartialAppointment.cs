﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pinkmeupkt.Models
{
    public class PartialAppointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime startTime { get; set; }
        public string startTimeString { get; set; }
        public DateTime bookTime { get; set; }
        public string bookTimeString { get; set; }
        public int OfferId { get; set; }
    }
}