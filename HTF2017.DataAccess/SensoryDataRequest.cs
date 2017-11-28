using System;

namespace HTF2017.DataAccess
{
    public class SensoryDataRequest
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public Boolean Location { get; set; }

        public Boolean Crowd { get; set; }

        public Boolean Mood { get; set; }

        public Boolean Relationship { get; set; }

        public DateTime TimeStamp { get; set; }

        public Boolean Fulfilled { get; set; }

        public Guid AndroidId { get; set; }

        public virtual Android Android { get; set; }
    }
}