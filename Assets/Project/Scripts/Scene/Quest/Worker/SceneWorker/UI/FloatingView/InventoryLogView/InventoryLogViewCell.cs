using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class InventoryLogViewCell : MonoBehaviour
    {
        [SerializeField] Text logTypeText;
        [SerializeField] RawImage itemIcon;
        [SerializeField] Text itemText;
        [SerializeField] Text amountText;
        
        [SerializeField] Animator animator;
        
        public LogData CurrentLogData { get; private set; }

        public bool IsUsing { get; private set; }

        public enum LogType
        {
            Add,
            Remove,
        }

        public class LogData
        {
            public LogType LogType { get; }

            public ItemVO ItemVO { get; }
            
            public int Amount { get; }

            public LogData(LogType logType, ItemVO itemVO, int amount)
            {
                LogType = logType;
                ItemVO = itemVO;
                Amount = amount;
            }
            
            public LogData(LogData prev, int addAmount)
            {
                LogType = prev.LogType;
                ItemVO = prev.ItemVO;
                Amount = prev.Amount + addAmount;
            }
        }

        public void Apply(LogData logData)
        {
            if (logData == null)
            {
                Hide();
                return;
            }

            Show();
            
            animator.SetTrigger(AnimatorKey.Show);
            
            CurrentLogData = logData;

            logTypeText.text = $"{logData.LogType}:";
            itemText.text = logData.ItemVO.Name;
            amountText.text = $"x{logData.Amount}";
            
            itemIcon.gameObject.SetActive(false);
            AssetLoader.Instance.StartLoadAsyncTextureCache(
                logData.ItemVO.ImageAsset,
                target =>
                {
                    itemIcon.texture = target;
                    itemIcon.gameObject.SetActive(true);
                });
        }

        public void OnHideFromAnimator()
        {
            Hide();
        }

        void Show()
        {
            IsUsing = true;
            gameObject.SetActive(true);
        }

        void Hide()
        {
            IsUsing = false;
            gameObject.SetActive(false);
        }
    }
}