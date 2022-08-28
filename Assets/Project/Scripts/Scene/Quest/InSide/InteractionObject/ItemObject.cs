using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ItemObject : InteractionObject
    {
        [SerializeField] ParticleSystem particleSystem;

        public override IInteractData InteractData => ItemInteractData;
        public override InteractionType InteractionType => InteractionType.Item;

        public ItemInteractData ItemInteractData { get; private set; }
        public Rarity Rarity { get; private set; }

        public void Apply(ItemInteractData itemInteractData)
        {
            ItemInteractData = itemInteractData;

            Rarity = itemInteractData.ItemData.ItemVO.Rarity;

            var particleSetting = particleSystem.main;
            particleSetting.startColor = Rarity.GetRarityColor();
            
            transform.position = itemInteractData.Position + GetPlaceOffsetHeight();
            MessageBus.Instance.SendInteractionObject.Broadcast(this, true);
        }

        protected override void OnRelease()
        {
            InteractData.Position = transform.position;
            MessageBus.Instance.SendInteractionObject.Broadcast(this, false);
        }
    }
}
