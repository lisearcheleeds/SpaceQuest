﻿namespace RoboQuest.Quest.InSide
{
    public interface IThreat
    {
        WeaponData WeaponData { get; }
        ITarget Target { get; }
        
        CollisionShape HitCollidePrediction { get; }
    }
}