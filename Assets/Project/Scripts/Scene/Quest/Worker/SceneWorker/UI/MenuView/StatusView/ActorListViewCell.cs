using System;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class ActorListViewCell : FancyScrollRectCell<ActorListViewCell.CellData, ActorListViewCell.CellContext>
    {
        [SerializeField] Image icon;
        [SerializeField] Text text;
        [SerializeField] Text distanceText;
        [SerializeField] Button button;
        [SerializeField] Animator animator;

        CellData cellData;
        float lastUpdateTime;

        public class CellData
        {
            public ActorData ActorData { get; }
            public bool IsSelected { get; }
            public Func<IPositionData, string> GetDistanceText { get; private set; }

            public CellData(ActorData actorData, bool isSelected, Func<IPositionData, string> getDistanceText)
            {
                ActorData = actorData;
                IsSelected = isSelected;
                GetDistanceText = getDistanceText;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
            public Action<CellData> OnSelect { get; set; }
        }

        public override void Initialize()
        {
            base.Initialize();
            button.onClick.AddListener(OnClick);
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;
            text.text = cellData.ActorData.ActorSpecVO.Name;
            distanceText.text = cellData.GetDistanceText(cellData.ActorData);

            animator.SetBool(AnimatorKey.IsSelect, cellData.IsSelected);
        }

        void Update()
        {
            if (Time.time - lastUpdateTime > 1.0f)
            {
                lastUpdateTime = Time.time;
                distanceText.text = cellData.GetDistanceText(cellData.ActorData);
            }
        }

        void OnClick()
        {
            Context.OnSelect(cellData);
        }
    }
}
