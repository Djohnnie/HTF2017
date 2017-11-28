using System;

namespace HTF2017.DataTransferObjects
{
    public class AndroidRequestDto
    {
        public String Password { get; set; }

        public Boolean Location { get; set; }

        public Boolean Crowd { get; set; }

        public Boolean Mood { get; set; }

        public Boolean Relationship { get; set; }
    }
}