using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class ActorAIFight : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Fight;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            // ターゲット更新
            var currentTarget = actorAIHandler.Targets.FirstOrDefault(x => x.TargetData.IsTargetable && actorAIHandler.Actor.ActorData.InstanceId != x.TargetData.InstanceId);
            if (currentTarget == null)
            {
                actorAIHandler.MainTarget = null;
                return ActorAIState.Check;
            }

            actorAIHandler.MainTarget = currentTarget;
            
            // 戦闘中の移動先
            var targetRelativePosition = currentTarget.TargetData.Position - actorAIHandler.ActorData.Position;
            var targetDirection = targetRelativePosition.normalized;
            var targetDistance = targetRelativePosition.magnitude;
            if (targetDistance > 30.0f)
            {
                actorAIHandler.RequestMove = targetDirection;
            }
            
            if (targetDistance < 20.0f && targetDistance != 0)
            {
                actorAIHandler.RequestMove = targetDirection * -1.0f;
            }

            foreach (var hitThreat in actorAIHandler.HitThreatList)
            {
                actorAIHandler.RequestQuickMove = hitThreat.HitCollidePrediction.GetOutwardVector(actorAIHandler.ActorData.Position);
            }

            // 武器
            foreach (var weaponData in actorAIHandler.ActorData.WeaponData)
            {
                var availability = weaponData.GetAvailability();
                if (availability == 0.0f)
                {
                    if (weaponData.IsReloadable())
                    {
                        var itemDataList = actorAIHandler.ActorData.InventoryDataList
                            .SelectMany(x => x.VariableInventoryViewData.CellData)
                            .OfType<ItemData>()
                            .Where(x => x.ItemVO.ItemTypes.Any(y => y == ItemType.Ammo));
                        var useAmmo = new List<ItemVO>();
            
                        foreach (var itemData in itemDataList)
                        {
                            if (useAmmo.Count >= weaponData.ActorPartsWeaponParameterVO.WeaponResourceMaxCount)
                            {
                                break;
                            }
                
                            if (itemData.ItemVO.ItemExclusiveVOs.OfType<ItemExclusiveAmmoVO>().First().AmmoType == weaponData.ActorPartsWeaponParameterVO.AmmoType)
                            {
                                var includeCount = Mathf.Min(weaponData.ActorPartsWeaponParameterVO.WeaponResourceMaxCount - useAmmo.Count, itemData.Amount.Value);
                                useAmmo.AddRange(Enumerable.Range(0, includeCount).Select(_ => itemData.ItemVO));
                                itemData.Amount = itemData.Amount - includeCount;
                            }
                        }

                        if (useAmmo.Any())
                        {
                            weaponData.Reload(useAmmo.ToArray());
                            MessageBus.Instance.UserCommandUpdateInventory.Broadcast(
                                actorAIHandler.ActorData.InventoryDataList.Select(x => x.InstanceId).ToArray());
                        }
                    }

                    continue;
                }

                if (weaponData.IsExecutable(currentTarget.TargetData))
                {
                    weaponData.Execute(currentTarget.TargetData);
                }
            }
            
            return ActorAIState.Fight;
        }
    }
}