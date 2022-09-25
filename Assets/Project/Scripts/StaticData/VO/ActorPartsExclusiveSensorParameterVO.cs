namespace AloneSpace
{
    public class ActorPartsExclusiveSensorParameterVO : IActorPartsExclusiveParameterVO
    {
        public int Id => row.Id;

        public ActorPartsExclusiveType ActorPartsExclusiveType => ActorPartsExclusiveType.Sensor;

        // 視認距離
        public float? VisionSensorDistance => row.VisionSensorDistance;
            
        // 音距離
        public float? SoundSensorDistance => row.SoundSensorDistance;
            
        // レーダー
        public float? RadarSensorPerformance => row.RadarSensorPerformance;

        ActorPartsExclusiveSensorParameterMaster.Row row;
        
        public ActorPartsExclusiveSensorParameterVO(int id)
        {
            row = ActorPartsExclusiveSensorParameterMaster.Instance.Get(id);
        }
    }
}