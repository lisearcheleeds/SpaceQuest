using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ItemObject : InteractionObject
    {
        [SerializeField] ParticleSystem particleSystem;

        public override IInteractData InteractData => ItemInteractData;
        public override InteractionType InteractionType => InteractionType.Item;

        public ItemInteractData ItemInteractData { get; private set; }
        public Rarity Rarity => ItemInteractData.ItemData.ItemVO.Rarity;

        public void Apply(ItemInteractData itemInteractData)
        {
            ItemInteractData = itemInteractData;
            var particleSetting = particleSystem.main;
            particleSetting.startColor = Rarity.GetRarityColor();
            transform.position = itemInteractData.Position;
        }

        protected override void OnRelease()
        {
            InteractData.SetPosition(transform.position);
        }
    }
}
