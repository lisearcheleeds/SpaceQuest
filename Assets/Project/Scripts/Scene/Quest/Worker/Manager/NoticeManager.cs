using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class NoticeManager : MonoBehaviour
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.NoticeDamage.AddListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
        }

        public void Finalize()
        {
            MessageBus.Instance.NoticeDamage.RemoveListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
        }

        void NoticeDamage(WeaponData weaponData, ItemVO resourceItemVO, IDamageableData damageableData)
        {
            var prevIsBroken = damageableData.IsBroken;
            
            damageableData.OnDamage(new DamageData(weaponData, resourceItemVO, damageableData));

            if (!prevIsBroken && damageableData.IsBroken)
            {
                MessageBus.Instance.NoticeBroken.Broadcast(damageableData);
            }
        }
        
        void NoticeBroken(IDamageableData damageableData)
        {
            if (!(damageableData is ActorData actorData))
            {
                return;
            }

            var areaIndex = actorData.CurrentAreaIndex;
            
            // 一覧から削除
            questData.ActorData.Remove(actorData);
            
            // 残骸を設置
            var interactBrokenActorData = new BrokenActorInteractData(actorData, actorData.Position);
            questData.MapData.AreaData[areaIndex].AddInteractData(interactBrokenActorData);
            
            // 適当なアイテムを設置
            var inventoryData = ItemDataVOHelper.GetActorDropInventoryData(actorData);
            questData.MapData.AreaData[areaIndex].AddInteractData(new InventoryInteractData(inventoryData, areaIndex, actorData.Position));
        }
    }
}
