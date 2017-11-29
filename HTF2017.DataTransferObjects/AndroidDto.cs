using System;

namespace HTF2017.DataTransferObjects
{
    public class AndroidDto
    {
        public Guid Id { get; set; }

        public AutoPilotDto AutoPilot { get; set; }

        public SensorAccuracyDto LocationSensorAccuracy { get; set; }

        public SensorAccuracyDto CrowdSensorAccuracy { get; set; }

        public SensorAccuracyDto MoodSensorAccuracy { get; set; }

        public SensorAccuracyDto RelationshipSensorAccuracy { get; set; }

        public Boolean Compromised { get; set; }
    }
}