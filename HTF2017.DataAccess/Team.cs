using System;
using System.Collections.Generic;

namespace HTF2017.DataAccess
{
    public class Team
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public String Name { get; set; }

        public Int64 Score { get; set; }

        public Int32 TotalNumberOfAndroids { get; set; }

        public String FeedbackEndpoint { get; set; }

        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Android> Androids { get; set; }
    }
}