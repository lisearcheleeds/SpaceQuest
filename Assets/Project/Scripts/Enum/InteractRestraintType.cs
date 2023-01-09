namespace AloneSpace
{
    // インタラクト条件
    public enum InteractRestraintType
    {
        None, // 制限無し
        NearPosition, // 付近の位置
        Angle, // 角度
        NearPositionAndAngle, // 位置と角度
    }
}