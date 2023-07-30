using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.Common
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] bool isAutoRefresh;
        [SerializeField] Tab[] tabs;
        Action<int> onChangeIndexFromButton;

        public int Index { get; private set; }

        [Serializable]
        struct Tab
        {
            [SerializeField] Button tabButton;
            [SerializeField] GameObject onButtonObject;
            [SerializeField] GameObject offButtonObject;

            [SerializeField] GameObject paneObject;

            public Button TabButton => tabButton;
            public GameObject OnButtonObject => onButtonObject;
            public GameObject OffButtonObject => offButtonObject;

            public GameObject PaneObject => paneObject;
        }

        public void SetOnChangeIndexFromButton(Action<int> callback)
        {
            onChangeIndexFromButton = callback;
        }

        public void SetIndex(int index)
        {
            Index = index;
            RefreshView();
        }

        void Awake()
        {
            Index = 0;
            for (var i = 0; i < tabs.Length; i++)
            {
                var index = i;
                tabs[i].TabButton.onClick.AddListener(() => OnClickTab(index));
            }
        }

        void OnClickTab(int index)
        {
            Index = index;
            onChangeIndexFromButton?.Invoke(index);

            if (isAutoRefresh)
            {
                RefreshView();
            }
        }

        void RefreshView()
        {
            for (var i = 0; i < tabs.Length; i++)
            {
                tabs[i].OnButtonObject.SetActive(i == Index);
                tabs[i].OffButtonObject.SetActive(i != Index);

                tabs[i].PaneObject.SetActive(i == Index);
            }
        }
    }
}
