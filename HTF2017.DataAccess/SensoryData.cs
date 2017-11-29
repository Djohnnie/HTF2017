using System;

namespace HTF2017.DataAccess
{
    public class SensoryData
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public Double? Lattitude { get; set; }

        public Double? Longitude { get; set; }

        public Int32? Crowd { get; set; }

        public Byte? Mood { get; set; }

        public Byte? Relationship { get; set; }

        public DateTime TimeStamp { get; set; }

        public Boolean Sent { get; set; }

        public Boolean? Received { get; set; }

        public Boolean AutonomousRequested { get; set; }

        public Guid AndroidId { get; set; }

        public virtual Android Android { get; set; }
    }
}