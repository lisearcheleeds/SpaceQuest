using System;
using System.Collections;
using FancyScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class InteractionListViewCell : FancyScrollRectCell<InteractionListViewCell.CellData, InteractionListViewCell.CellContext>
    {
        [SerializeField] Image icon;
        [SerializeField] Text text;
        [SerializeField] Text distanceText;
        [SerializeField] Button button;
        
        [SerializeField] Animator animator;

        CellData cellData;
        float lastUpdateTime;
        Coroutine confirmCancelTimerCoroutine;
        
        public class CellData
        {
            public IInteractData InteractData { get; }
            public bool IsSelected { get; }
            public string NameText { get; private set; }
            public Func<IInteractData, string> GetDistanceText { get; private set; }

            public CellData(IInteractData interactData, bool isSelected, Func<IInteractData, string> getDistanceText)
            {
                InteractData = interactData;
                NameText = $"Area {interactData.AreaId}";
                IsSelected = isSelected;
                GetDistanceText = getDistanceText;
            }
        }

        public class CellContext : FancyScrollRectContext
        {
            public Action<CellData> OnSelect { get; set; }
            
            public Action<CellData> OnConfirm { get; set; }
        }

        public override void Initialize()
        {
            base.Initialize();
            button.onClick.AddListener(OnClick);
        }

        public override void UpdateContent(CellData cellData)
        {
            this.cellData = cellData;
            text.text = cellData.NameText;
            distanceText.text = cellData.GetDistanceText(cellData.InteractData);

            animator.SetBool(AnimatorKey.IsSelect, cellData.IsSelected);
        }

        void Update()
        {
            if (Time.time - lastUpdateTime > 1.0f)
            {
                lastUpdateTime = Time.time;
                distanceText.text = cellData.GetDistanceText(cellData.InteractData);
            }
        }

        void OnClick()
        {
            if (!cellData.IsSelected || confirmCancelTimerCoroutine == null)
            {
                Context.OnSelect(cellData);
                animator.Play(AnimatorKey.Confirming);
                confirmCancelTimerCoroutine = StartCoroutine(ConfirmCancelTimer());
                return;
            }

            animator.Play(AnimatorKey.Confirm);
            Context.OnConfirm(cellData);

            StopCoroutine(confirmCancelTimerCoroutine);
            confirmCancelTimerCoroutine = null;
        }

        IEnumerator ConfirmCancelTimer()
        {
            yield return new WaitForSeconds(2.0f);
            confirmCancelTimerCoroutine = null;
            animator.Play(AnimatorKey.Off);
        }
    }
}