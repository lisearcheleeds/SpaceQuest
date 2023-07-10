namespace AloneSpace
{
    public class ParticleBulletMakerWeaponStateData : WeaponStateData
    {
        public override bool IsReloadable => false;
        public override bool IsExecutable => true;

        public override float FireTime => 0;
        public override float ReloadRemainTime => 0;
        public override int ResourceIndex => 0;
    }
}
