using System;

namespace HTF2017.DataTransferObjects
{
    public class TeamDto
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public Int64 Score { get; set; }

        public Int32 TotalNumberOfAndroids { get; set; }

        public Int32 NumberOfAndroidsAvailable { get; set; }

        public Int32 NumberOfAndroidsActive { get; set; }

        public Int32 NumberOfAndroidsCompromised { get; set; }

        public String FeedbackEndpoint { get; set; }

        public Guid LocationId { get; set; }

        public String LocationName { get; set; }
    }
}