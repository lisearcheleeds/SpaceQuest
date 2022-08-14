using System.Linq;

namespace RoboQuest
{
    public class ActorPartsExclusiveSensorParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // 視認距離
            public float? VisionSensorDistance { get; }
            
            // 音距離
            public float? SoundSensorDistance { get; }
            
            // レーダー
            public float? RadarSensorPerformance { get; }

            public Row(
                int id,
                float visionSensorDistance,
                float soundSensorDistance,
                float radarSensorPerformance)
            {
                Id = id;
                VisionSensorDistance = visionSensorDistance;
                SoundSensorDistance = soundSensorDistance;
                RadarSensorPerformance = radarSensorPerformance;
            }
        }

        Row[] rows;
        static ActorPartsExclusiveSensorParameterMaster instance;

        public static ActorPartsExclusiveSensorParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExclusiveSensorParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExclusiveSensorParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 100.0f, 60.0f, 300.0f)
            };
        }
    }
}
