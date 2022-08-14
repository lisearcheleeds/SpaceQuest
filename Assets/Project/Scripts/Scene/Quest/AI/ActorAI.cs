using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RoboQuest.Quest
{
    /// <summary>
    /// ・ActorはPlayerの目的地に向かって最短ルートでArea移動する
    /// ・Actorは目的のInteractionに向かってエリア内の障害物を無視して移動する
    /// ・ActorはArea内のアイテムをPlayerPolicyにそってInteractする
    /// ・ActorはArea内に敵が居た場合、装備の有効距離に沿って最も近い敵を基準に移動する
    /// 　　離れる場合は敵からの有効距離を保ち続ける、最もエリアの中心に近い地点に移動する
    /// ・ActorはArea内に敵が居た場合、装備ごとに攻撃判定を行う
    /// 　（ただし、ActorのArea内の敵に対する挙動はPlayerTacticsにそって分岐する）
    /// ・Interactにかかる時間はスキップし、瞬時にInteractする
    /// </summary>
    public static class ActorAI
    {
        public static void Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            if (actorData.ActorState == ActorState.Broken || actorData.IsInteracting)
            {
                return;
            }
                
            // 戦闘
            if (UpdateFight(questData, actorData, deltaTime))
            {
                return;
            }
                
            // interact
            if (UpdateInteract(actorData))
            {
                return;
            }

            // ルート更新
            UpdateRoute(questData, actorData);
                
            // 移動
            UpdateMove(actorData, deltaTime);
        }
        
        static bool UpdateFight(QuestData questData, ActorData actorData, float deltaTime)
        {
            var nearestOtherActor = questData.ActorData
                .Where(x => x.CurrentAreaIndex == actorData.CurrentAreaIndex && actorData.PlayerQuestDataInstanceId != x.PlayerQuestDataInstanceId)
                .DefaultIfEmpty()
                .OrderBy(x => (x?.Position - actorData.Position)?.sqrMagnitude)
                .FirstOrDefault();

            if (nearestOtherActor == null)
            {
                return false;
            }
            
            // 移動
            var areaSize = questData.MapData.AreaData[actorData.CurrentAreaIndex].AreaAssetVO.AreaSize;
            var optimalRange = actorData.ActorSpecData.VisionSensorDistance;
            var wayDirection = nearestOtherActor.Position - actorData.Position;
            var isMoveAway = wayDirection.sqrMagnitude < optimalRange * optimalRange;
            if (isMoveAway)
            {
                // 良い感じに中央に引き寄せられる角度
                // FIXME: ちゃんとしたのを考える
                var wayPoint = Vector3.Cross(wayDirection, Vector3.up);
                actorData.SetPosition(actorData.Position + wayPoint.normalized * actorData.ActorSpecData.MovingSpeed * deltaTime);
            }
            else
            {
                actorData.SetPosition(actorData.Position + wayDirection.normalized * actorData.ActorSpecData.MovingSpeed * deltaTime);
            }

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
                            MessageBus.Instance.UserCommandUpdateInventory.Broadcast(
                                actorData.InventoryDataList.Select(x => x.InstanceId).ToArray());
                        }
                    }

                    continue;
                }

                if (weaponData.IsExecutable(nearestOtherActor))
                {
                    weaponData.Execute(nearestOtherActor);
                }
            }

            return true;
        }
        
        static bool UpdateInteract(ActorData actorData)
        {
            var nextOrder = actorData.GetNextInteractOrder();
            if (!nextOrder?.IsInteractionRange(actorData.Position) ?? true)
            {
                return false;
            }

            actorData.SetInteract(nextOrder);
            return true;
        }
        
        static void UpdateMove(ActorData actorData, float deltaTime)
        {
            var nextOrder = actorData.GetNextInteractOrder();
            if (nextOrder == null)
            {
                return;
            }

            // 移動
            var wayDirection = nextOrder.Position - actorData.Position;
            if (wayDirection.magnitude > actorData.ActorSpecData.MovingSpeed * deltaTime)
            {
                actorData.SetPosition(actorData.Position + wayDirection.normalized * actorData.ActorSpecData.MovingSpeed * deltaTime);
                return;
            }
            
            actorData.SetPosition(nextOrder.Position);
        }

        static void UpdateRoute(QuestData questData, ActorData actorData)
        {
            var areaTransitionOrder = actorData.InteractOrder.FirstOrDefault(x => x is AreaTransitionInteractData);
            if (areaTransitionOrder != null)
            {
                // 既に設定されている場合はスルー
                return;
            }

            var routeAreaIndexes = actorData.GetRouteAreaData()?.Select(x => x.AreaIndex).ToArray();
            if (routeAreaIndexes == null || routeAreaIndexes?.Length == 0)
            {
                // 目的地が無い場合はスルー
                return;
            }
            
            // 移動したいエリア
            var areaData = questData.MapData.AreaData[actorData.CurrentAreaIndex];
            AreaTransitionInteractData areaTransitionInteractData = null;
            foreach (var routeAreaIndex in routeAreaIndexes)
            {
                areaTransitionInteractData = areaData.InteractData
                    .Where(x => x is AreaTransitionInteractData)
                    .Cast<AreaTransitionInteractData>()
                    .FirstOrDefault(x => x.TransitionAreaIndex == routeAreaIndex) ?? areaTransitionInteractData;
            }
            
            actorData.InteractOrder.Add(areaTransitionInteractData);
        }
    }
}