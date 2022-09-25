namespace AloneSpace
{
    public interface IWeaponVO
    {
        int Id { get; }
        string Name { get; }
        WeaponType WeaponType { get; }
    }
}
