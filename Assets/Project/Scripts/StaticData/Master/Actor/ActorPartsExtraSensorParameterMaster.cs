using System.Linq;

namespace AloneSpace
{
    public class ActorPartsExtraSensorParameterMaster
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
        static ActorPartsExtraSensorParameterMaster instance;

        public static ActorPartsExtraSensorParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExtraSensorParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExtraSensorParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 100.0f, 60.0f, 300.0f)
            };
        }
    }
}
