using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIFight : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Fight;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            // ターゲット確認
            if (!actorData.ActorAICache.MainTarget?.IsAlive ?? true)
            {
                var currentTarget = actorData.ActorAICache.AroundTargets.FirstOrDefault(target => target.IsAlive && actorData.PlayerInstanceId != (target as ActorData)?.PlayerInstanceId);
                if (currentTarget != null)
                {
                    // ターゲット更新
                    actorData.ActorAICache.MainTarget = currentTarget;
                }
                else
                {
                    // 戦闘終了
                    actorData.ActorAICache.MainTarget = null;
                    return ActorAIState.Check;
                }
            }

            // 戦闘中の移動先
            // 今はまだゆっくり向いて固定値進むだけ
            var direction = actorData.ActorAICache.MainTarget.Position - actorData.Position;
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;

            // 武器
            foreach (var weaponData in actorData.WeaponData)
            {
                var availability = weaponData.GetAvailability();
                if (availability == 0.0f)
                {
                    if (weaponData.IsReloadable())
                    {
                        var itemDataList = actorData.InventoryDataList
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
                            MessageBus.Instance.UserCommandUpdateInventory.Broadcast(actorData.InventoryDataList.Select(x => x.InstanceId).ToArray());
                        }
                    }

                    continue;
                }

                if (weaponData.IsExecutable(actorData.ActorAICache.MainTarget))
                {
                    weaponData.Execute(actorData.ActorAICache.MainTarget);
                }
            }
            
            return ActorAIState.Fight;
        }
    }
}