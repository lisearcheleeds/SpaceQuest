using System;
using System.Linq;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class InteractionItemObjectListViewCell : FancyScrollRectCell<InteractionItemObjectListViewCell.CellData, InteractionItemObjectListViewCell.CellContext>
    {
        [SerializeField] Image backColor;
        [SerializeField] Image frontColor;
        [SerializeField] Text text;
        [SerializeField] Button button;
        
        [SerializeField] GameObject isSelect;
        [SerializeField] GameObject isInteractTarget;
        [SerializeField] Text InteractTargetText;

        CellData cellData;
        
        public class CellData
        {
            public Rarity Rarity;
            public string Text;
            public ItemData ItemData;
            
            public CellData(ItemData itemData)
            {
                Rarity = itemData.ItemVO.Rarity;
                Text = itemData.ItemVO.Text;
                ItemData = itemData;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
            public Action<CellData> OnClick { get; set; }
            
            public ItemInteractData[][] TakeOrderItems { get; set; }
            
            public CellData SelectCellData { get; set; }
        }

        public override void Initialize()
        {
            base.Initialize();
            button.onClick.AddListener(() => Context.OnClick(cellData));
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;
            text.text = cellData.Text;
            backColor.color = cellData.Rarity.GetRarityColor();
            frontColor.color = cellData.Rarity.GetRaritySubColor();

            // 選択中
            isSelect.SetActive(Context.SelectCellData == cellData);
            
            // Interact中
            isInteractTarget.SetActive(false);
                
            for (var i = 0; i < Context.TakeOrderItems.Length; i++)
            {
                var index = Context.TakeOrderItems[i].FirstIndex(x => x.ItemData == cellData.ItemData);
                if (index != -1)
                {
                    isInteractTarget.SetActive(true);
                    InteractTargetText.text = $"Actor{i}[{index}]";
                }
            }
        }
    }
}