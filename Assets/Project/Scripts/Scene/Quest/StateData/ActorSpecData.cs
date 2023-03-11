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

        public Dictionary<int, ActorPartsVO[]> ActorPartsVOHierarchy { get; private set; }

        public float Endurance { get; private set; }
        public float KineticResistant { get; private set; }
        public float HeatResistant { get; private set; }
        public float BlastResistant { get; private set; }

        public float ForwardBoosterPower { get; private set; }
        public float BackBoosterPower { get; private set; }
        public float HorizonBoosterPower { get; private set; }
        public float VerticalBoosterPower { get; private set; }
        
        public float PitchBoosterPower { get; private set; }
        public float RollBoosterPower { get; private set; }
        public float YawBoosterPower { get; private set; }
        
        public float VisionSensorDistance { get; private set; }
        public float SoundSensorDistance { get; private set; }
        public float RadarSensorPerformance { get; private set; }

        public ActorPartsExtraInventoryParameterVO[] ActorPartsExclusiveInventoryParameterVOs { get; private set; }

        public IActorPartsWeaponParameterVO[] ActorPartsWeaponParameterVOs { get; private set; }

        public void Setup(ActorBluePrint actorBluePrint)
        {
            ActorBluePrint = actorBluePrint;

            ActorPartsVOHierarchy = actorBluePrint.PartsHierarchy
                .ToDictionary(kv => kv.Key,
                    kv => kv.Value.Select(x => new ActorPartsVO(ActorPartsMaster.Instance.Get(x))).ToArray());
            Refresh();
        }

        void Refresh()
        {
            Endurance = ActorPartsVOHierarchy.Values.Sum(x => x.Sum(y => y.ActorPartsParameterVO.Endurance));
            KineticResistant = ActorPartsVOHierarchy.Values.Sum(x => x.Sum(y => y.ActorPartsParameterVO.KineticResistant));
            HeatResistant = ActorPartsVOHierarchy.Values.Sum(x => x.Sum(y => y.ActorPartsParameterVO.HeatResistant));
            BlastResistant = ActorPartsVOHierarchy.Values.Sum(x => x.Sum(y => y.ActorPartsParameterVO.BlastResistant));

            var externalMovingParameterVOs = ActorPartsVOHierarchy.Values.SelectMany(x => x.Select(y => y.ActorPartsExtraBoosterParameterVO)).Where(x => x != null).ToArray();
            ForwardBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.MainDirectionRatio);
            BackBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubDirectionRatio);
            HorizonBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubDirectionRatio);
            VerticalBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubDirectionRatio);
            PitchBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubRotateRatio);
            RollBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubRotateRatio);
            YawBoosterPower = externalMovingParameterVOs.Sum(x => x.BoosterPower * x.SubRotateRatio);

            var externalSensorParameterVOs = ActorPartsVOHierarchy.Values.SelectMany(x => x.Select(y => y.ActorPartsExtraSensorParameterVO)).Where(x => x != null).ToArray();
            VisionSensorDistance = externalSensorParameterVOs.Length != 0 ? externalSensorParameterVOs.Max(x => x.VisionSensorDistance ?? 0) : 0;
            SoundSensorDistance = externalSensorParameterVOs.Length != 0 ? externalSensorParameterVOs.Max(x => x.SoundSensorDistance ?? 0) : 0;
            RadarSensorPerformance = externalSensorParameterVOs.Length != 0 ? externalSensorParameterVOs.Max(x => x.RadarSensorPerformance ?? 0) : 0;
            
            ActorPartsExclusiveInventoryParameterVOs = ActorPartsVOHierarchy.SelectMany(kv => kv.Value.Select(x => x.ActorPartsExtraInventoryParameterVO)).Where(x => x != null).ToArray();
            
            ActorPartsWeaponParameterVOs = ActorPartsVOHierarchy.SelectMany(kv => kv.Value.Select(x => x.ActorPartsWeaponParameterVO)).Where(x => x != null).ToArray();
        }
    }
}
