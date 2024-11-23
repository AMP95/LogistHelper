﻿using System.ComponentModel;

namespace Models
{
   
    public class Carrier : Company
    {
        public VAT Vat { get; set; }

        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Truck> Trucks { get; set; }
        public virtual ICollection<Trailer> Trailers { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
