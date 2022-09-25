using System.Collections;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InventoryObject : InteractionObject
    {
        [SerializeField] ParticleSystem particleSystem;

        public override IInteractData InteractData => InventoryInteractData;
        public override InteractionType InteractionType => InteractionType.Inventory;

        public InventoryInteractData InventoryInteractData { get; private set; }
        public Rarity Rarity { get; private set; }

        public void Apply(InventoryInteractData inventoryInteractData)
        {
            InventoryInteractData = inventoryInteractData;

            Rarity = inventoryInteractData.InventoryData.Max(x => x.VariableInventoryViewData.CellData.Max(y => (y as ItemData)?.ItemVO.Rarity ?? Rarity.Common));

            var particleSetting = particleSystem.main;
            particleSetting.startColor = Rarity.GetRarityColor();
            
            transform.position = inventoryInteractData.Position + GetPlaceOffsetHeight();
        }

        protected override void OnRelease()
        {
            InteractData.SetPosition(transform.position);
        }
    }
}
