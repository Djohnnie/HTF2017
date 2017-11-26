using System;

namespace HTF2017.DataTransferObjects
{
    public class LocationDto
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public double Lattitude { get; set; }
        
        public double Longitude { get; set; }
    }
}