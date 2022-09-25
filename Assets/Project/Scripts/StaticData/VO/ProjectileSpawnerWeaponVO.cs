namespace AloneSpace
{
    public class ProjectileSpawnerWeaponVO : IWeaponVO
    {
        public int Id { get; }
        public string Name { get; }
        public WeaponType WeaponType => WeaponType.ProjectileSpawner;

        public int ProjectileId { get; }
        public float PrimaryReloadTime { get; }
        public float SecondaryReloadTime { get; }
        public int MagazineCapacity { get; }
        public int AmmoCount { get; }

        public ProjectileSpawnerWeaponVO(int id)
        {
            Id = id;
            Name = id == 0 ? "Vulcan" : "Missile";
            ProjectileId = id;
            PrimaryReloadTime = id == 0 ? 0.05f : 0.1f;
            SecondaryReloadTime = id == 0 ? 0.5f : 6.0f;
            MagazineCapacity = id == 0 ? 8 : 1;
            AmmoCount = id == 0 ? 600 : 60;
        }
    }
}
