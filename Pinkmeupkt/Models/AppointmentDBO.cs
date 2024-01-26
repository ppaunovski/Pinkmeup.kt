using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pinkmeupkt.Models
{
    public class AppointmentDBO
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string bookTime { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public bool isBooked { get; set; }
        public int Id { get; set; }
    }
}