using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryView : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;
        
        [SerializeField] StandardCore inventoryCore;
        [SerializeField] InventoryDataView inventoryDataViewPrefab;
        [SerializeField] Button tabButtonPrefab;
        [SerializeField] Button closeMapButton;

        [SerializeField] GameObject rightInventoryObject;
        [SerializeField] RectTransform rightInventoryTabButtonParent;
        [SerializeField] RectTransform rightInventoryParent;
        List<InventoryData[]> rightData = new List<InventoryData[]>();
        List<Button> rightTabButtons = new List<Button>();
        List<InventoryDataView> rightStashView = new List<InventoryDataView>();
        int? rightTabIndex;
        
        [SerializeField] GameObject leftInventoryObject;
        [SerializeField] RectTransform leftInventoryTabButtonParent;
        [SerializeField] RectTransform leftInventoryParent;
        List<InventoryData[]> leftData = new List<InventoryData[]>();
        List<Button> leftTabButtons = new List<Button>();
        List<InventoryDataView> leftStashView = new List<InventoryDataView>();
        int? leftTabIndex;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandUpdateInventory.AddListener(UserCommandUpdateInventory);
            inventoryCore.Initialize();
            Close();
            
            closeMapButton.onClick.AddListener(OnClickClose);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void ApplyInventoryData(InventoryData[] data, bool isRight)
        {
            if (isRight)
            {
                rightData.Clear();
            }
            else
            {
                leftData.Clear();
            }

            AddInventoryData(data, isRight);
        }

        public void AddInventoryData(InventoryData[] data, bool isRight)
        {
            if (isRight)
            {
                rightData.Add(data);
            }
            else
            {
                leftData.Add(data);
            }

            UpdateView();
        }

        void UserCommandUpdateInventory(Guid[] inventoryInstanceIds)
        {
            UpdateView();
        }

        void UpdateView()
        {
            if (!IsOpen)
            {
                return;
            }

            UpdateSideView(rightInventoryObject, rightInventoryTabButtonParent, rightInventoryParent, rightData, rightTabButtons, rightStashView, ref rightTabIndex);
            UpdateSideView(leftInventoryObject, leftInventoryTabButtonParent, leftInventoryParent, leftData, leftTabButtons, leftStashView, ref leftTabIndex);
            
            void UpdateSideView(
                GameObject inventoryObject,
                RectTransform inventoryTabButtonParent,
                RectTransform inventoryParent,
                List<InventoryData[]> data,
                List<Button> tabButtons,
                List<InventoryDataView> stashView,
                ref int? tabIndex)
            {
                var dataCount = data.Count;
                for (var i = 0; i < Math.Max(dataCount, tabButtons.Count); i++)
                {
                    if (i >= tabButtons.Count)
                    {
                        var index = i;
                        var newTabButton = Instantiate(tabButtonPrefab, inventoryTabButtonParent);
                        newTabButton.onClick.AddListener(() => OnClickTab(index, true));
                        tabButtons.Add(newTabButton);
                    }

                    tabButtons[i].gameObject.SetActive(i < dataCount);
                    tabButtons[i].enabled = tabIndex != i;
                }

                tabIndex = data.Count == 0 ? null : (int?)Math.Min(tabIndex.HasValue ? tabIndex.Value : 0, data.Count - 1);
                var showData = tabIndex.HasValue ? data[tabIndex.Value] : null;
                var showDataLength = showData?.Length ?? 0;
                inventoryObject.SetActive(showData != null);
                for (var i = 0; i < Math.Max(showDataLength, stashView.Count); i++)
                {
                    if (i >= stashView.Count)
                    {
                        var newInventory = Instantiate(inventoryDataViewPrefab, inventoryParent);
                        inventoryCore.AddInventoryView(newInventory.StandardStashView);
                        stashView.Add(newInventory);
                    }

                    stashView[i].gameObject.SetActive(i < showDataLength);

                    if (i < Math.Min(showDataLength, stashView.Count))
                    {
                        stashView[i].Apply(showData[i]);
                    }
                }
            }
        }

        void OnClickTab(int index, bool isRight)
        {
            if (isRight)
            {
                rightTabIndex = index;
            }
            else
            {
                leftTabIndex = index;
            }

            UpdateView();
        }

        void OnClickClose()
        {
            Close();
        }
    }
}