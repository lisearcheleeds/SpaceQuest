using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorSpecData
    {
        public ActorBluePrint ActorBluePrint { get; private set; }

        public ActorPartsVO[] ActorPartsVOs { get; private set; }
        public Dictionary<ActorPartsParameterVO, IActorPartsParameterVO[]> ActorPartsParameterVOs { get; private set; }

        public float Endurance { get; private set; }
        public float KineticResistant { get; private set; }
        public float HeatResistant { get; private set; }
        public float BlastResistant { get; private set; }

        public float DirectionMovingModify { get; private set; }
        public float MovingSpeed { get; private set; }
        public float QuickMovingSpeed { get; private set; }
        public float RotateSpeed { get; private set; }
        public float QuickRotateSpeed { get; private set; }
        
        public float VisionSensorDistance { get; private set; }
        public float SoundSensorDistance { get; private set; }
        public float RadarSensorPerformance { get; private set; }

        public ActorPartsExclusiveInventoryParameterVO[] ActorPartsExclusiveInventoryParameterVOs { get; private set; }

        public IActorPartsWeaponParameterVO[] ActorPartsWeaponParameterVOs { get; private set; }

        public void Setup(ActorBluePrint actorBluePrint)
        {
            ActorBluePrint = actorBluePrint;

            ActorPartsVOs = actorBluePrint.PartsHierarchy
                .SelectMany(kv => kv.Value.Select(x => new ActorPartsVO(ActorPartsMaster.Instance.Get(x))).ToArray())
                .ToArray();
            
            ActorPartsParameterVOs = ActorPartsVOs
                .ToDictionary(
                    actorPartsVO => new ActorPartsParameterVO(ActorPartsParameterMaster.Instance.Get(actorPartsVO.ParameterId)),
                    actorPartsVO =>
                    {
                        var values = new List<IActorPartsParameterVO>();

                        if (actorPartsVO.ExclusiveParameterIds != null)
                        {
                            values.AddRange(actorPartsVO.ExclusiveParameterIds.Select(exclusiveParameterId =>
                            {
                                var master = ActorPartsExclusiveParameterMaster.Instance.Get(exclusiveParameterId);
                                switch (master.ActorPartsExclusiveType)
                                {
                                    case ActorPartsExclusiveType.Inventory:
                                        return (IActorPartsParameterVO)new ActorPartsExclusiveInventoryParameterVO(master.ActorPartsExclusiveId);
                                    case ActorPartsExclusiveType.Sensor:
                                        return new ActorPartsExclusiveSensorParameterVO(master.ActorPartsExclusiveId);
                                    case ActorPartsExclusiveType.Moving:
                                        return new ActorPartsExclusiveMovingParameterVO(master.ActorPartsExclusiveId);
                                    default:
                                        throw new NotImplementedException();
                                }
                            }));
                        }

                        if (actorPartsVO.WeaponParameterIds != null)
                        {
                            values.AddRange(actorPartsVO.WeaponParameterIds?.Select(weaponParameterIds =>
                            {
                                var master = ActorPartsWeaponParameterMaster.Instance.Get(weaponParameterIds);
                                switch (master.WeaponType)
                                {
                                    case WeaponType.Rifle:
                                        return (IActorPartsParameterVO)new ActorPartsWeaponRifleParameterVO(master.ActorPartsWeaponId);
                                    case WeaponType.MissileLauncher:
                                        return new ActorPartsWeaponMissileLauncherParameterVO(master.ActorPartsWeaponId);
                                    default:
                                        throw new NotImplementedException();
                                }
                            }).ToArray());
                        }

                        return values.ToArray();
                    });
            
            Refresh();
        }

        void Refresh()
        {
            Endurance = ActorPartsParameterVOs.Keys.Sum(x => x.Endurance);
            KineticResistant = ActorPartsParameterVOs.Keys.Sum(x => x.KineticResistant);
            HeatResistant = ActorPartsParameterVOs.Keys.Sum(x => x.HeatResistant);
            BlastResistant = ActorPartsParameterVOs.Keys.Sum(x => x.BlastResistant);

            var externalParameterVOs = ActorPartsParameterVOs.Values.SelectMany(x => x);
            DirectionMovingModify = 0.1f;
            MovingSpeed = externalParameterVOs.OfType<ActorPartsExclusiveMovingParameterVO>().Sum(x => x.MovingSpeed);
            QuickMovingSpeed = externalParameterVOs.OfType<ActorPartsExclusiveMovingParameterVO>().DefaultIfEmpty().Sum(x => x?.QuickMovingSpeed ?? 0);
            RotateSpeed = externalParameterVOs.OfType<ActorPartsExclusiveMovingParameterVO>().DefaultIfEmpty().Sum(x => x?.RotateSpeed ?? 0);
            QuickRotateSpeed = externalParameterVOs.OfType<ActorPartsExclusiveMovingParameterVO>().DefaultIfEmpty().Sum(x => x?.QuickRotateSpeed ?? 0);

            VisionSensorDistance = externalParameterVOs.OfType<ActorPartsExclusiveSensorParameterVO>().DefaultIfEmpty().Max(x => x?.VisionSensorDistance ?? 0);
            SoundSensorDistance = externalParameterVOs.OfType<ActorPartsExclusiveSensorParameterVO>().DefaultIfEmpty().Max(x => x?.SoundSensorDistance ?? 0);
            RadarSensorPerformance = externalParameterVOs.OfType<ActorPartsExclusiveSensorParameterVO>().DefaultIfEmpty().Max(x => x?.RadarSensorPerformance ?? 0);
            
            ActorPartsExclusiveInventoryParameterVOs = externalParameterVOs.OfType<ActorPartsExclusiveInventoryParameterVO>().ToArray();
            
            ActorPartsWeaponParameterVOs = externalParameterVOs.OfType<IActorPartsWeaponParameterVO>().ToArray();
        }
    }
}
