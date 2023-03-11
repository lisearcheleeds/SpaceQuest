namespace AloneSpace
{
    public class ActorPartsExtraSensorParameterVO
    {
        public int Id => row.Id;

        // 視認距離
        public float? VisionSensorDistance => row.VisionSensorDistance;
            
        // 音距離
        public float? SoundSensorDistance => row.SoundSensorDistance;
            
        // レーダー
        public float? RadarSensorPerformance => row.RadarSensorPerformance;

        ActorPartsExtraSensorParameterMaster.Row row;
        
        public ActorPartsExtraSensorParameterVO(int id)
        {
            row = ActorPartsExtraSensorParameterMaster.Instance.Get(id);
        }
    }
}