using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class ItemDataMenu : MonoBehaviour
    {
        [SerializeField] ItemDetail itemDetail;
        [SerializeField] Button button;
        [SerializeField] Text buttonText;
        [SerializeField] Text optionText;

        ItemData itemData;
        Action onClick;
        
        public void Initialize()
        {
            Close();
            button.onClick.AddListener(() => onClick());
            
            MessageBus.Instance.UserCommandOpenItemDataMenu.AddListener(Open);
            MessageBus.Instance.UserCommandCloseItemDataMenu.AddListener(Close);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandOpenItemDataMenu.RemoveListener(Open);
            MessageBus.Instance.UserCommandCloseItemDataMenu.RemoveListener(Close);
        }

        void Open(ItemData itemData, Action onClick, string buttonText, string optionText)
        {
            this.itemData = itemData;
            this.onClick = onClick;
            this.buttonText.text = buttonText;
            this.optionText.text = optionText;
            itemDetail.Apply(itemData);

            button.interactable = onClick != null;
            
            gameObject.SetActive(true);
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}