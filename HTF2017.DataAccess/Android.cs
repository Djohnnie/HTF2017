using System;

namespace HTF2017.DataAccess
{
    public class Android
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public Level Level { get; set; }

        public Boolean Compomised { get; set; }

        public virtual Team Team { get; set; }
    }
}