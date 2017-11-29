using System;

namespace HTF2017.DataTransferObjects
{
    public class FeedbackRequestDto
    {
        public Guid AndroidId { get; set; }

        public Double? Lattitude { get; set; }

        public Double? Longitude { get; set; }

        public Int32? Crowd { get; set; }

        public Byte? Mood { get; set; }

        public Byte? Relationship { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}