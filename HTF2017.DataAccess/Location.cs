using System;
using System.Collections.Generic;

namespace HTF2017.DataAccess
{
    public class Location
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public String Name { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}