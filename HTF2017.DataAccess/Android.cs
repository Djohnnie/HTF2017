using System;
using System.Collections.Generic;

namespace HTF2017.DataAccess
{
    public class Android
    {
        public Guid Id { get; set; }

        public Int32 SysId { get; set; }

        public AutoPilot AutoPilot { get; set; }

        public SensorAccuracy LocationSensorAccuracy { get; set; }

        public SensorAccuracy CrowdSensorAccuracy { get; set; }

        public SensorAccuracy MoodSensorAccuracy { get; set; }

        public SensorAccuracy RelationshipSensorAccuracy { get; set; }

        public Boolean Compromised { get; set; }

        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<SensoryData> SensoryData { get; set; }

        public virtual ICollection<SensoryDataRequest> SensoryDataRequests { get; set; }
    }
}