namespace RoboQuest
{
    /// <summary>
    /// パーツの基本データ
    /// </summary>
    public class ActorPartsParameterVO
    {
        // ID
        public int Id => row.Id;
        
        // 耐久力
        public float Endurance => row.Endurance;

        // 対物性
        public float KineticResistant => row.KineticResistant;

        // 耐熱性
        public float HeatResistant => row.HeatResistant;

        // 耐爆性
        public float BlastResistant => row.BlastResistant;

        ActorPartsParameterMaster.Row row;
        
        public ActorPartsParameterVO(ActorPartsParameterMaster.Row row)
        {
            this.row = row;
        }
    }
}
