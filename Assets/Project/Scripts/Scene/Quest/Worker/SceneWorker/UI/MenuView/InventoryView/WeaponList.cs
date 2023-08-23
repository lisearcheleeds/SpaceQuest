﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace
{
    public class WeaponList : MonoBehaviour, IVariableInventoryView
    {
        [SerializeField] Image actorImage;
        [SerializeField] RectTransform weaponHolderParent;

        [SerializeField] WeaponListGroup weaponListGroupPrefab;
        [SerializeField] WeaponListGroupCell weaponListGroupCellPrefab;

        List<WeaponListGroup> weaponListGroups = new List<WeaponListGroup>();
        List<WeaponListGroupCell> weaponListViewCells = new List<WeaponListGroupCell>();

        QuestData questData;

        bool isLayoutDirty;
        bool isWeaponDataDirty;
        ActorData currentControlActorData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
            if (questData.UserData.ControlActorData?.InstanceId != currentControlActorData?.InstanceId)
            {
                currentControlActorData = questData.UserData.ControlActorData;

                isLayoutDirty = true;
                isWeaponDataDirty = true;
            }

            if (isLayoutDirty && enabled)
            {
                RefreshLayout();
                isLayoutDirty = false;
            }

            if (isWeaponDataDirty && enabled)
            {
                RefreshWeaponData();
                isWeaponDataDirty = false;
            }
        }

        void RefreshLayout()
        {
            actorImage.SetNativeSize();

            var layouts = currentControlActorData.ActorSpecVO.WeaponSlotLayout
                .Select((xy, weaponIndex) => (new Vector2(xy.Item1, xy.Item2), weaponIndex))
                .GroupBy(layout => layout.Item1)
                .Select(x => x.Select(y => y))
                .ToArray();

            while (weaponListGroups.Count < layouts.Length)
            {
                weaponListGroups.Add(Instantiate(weaponListGroupPrefab, weaponHolderParent, false));
            }

            while (weaponListViewCells.Count < currentControlActorData?.ActorSpecVO.WeaponSlotCount)
            {
                weaponListViewCells.Add(Instantiate(weaponListGroupCellPrefab, weaponHolderParent, false));
            }

            foreach (var weaponListGroup in weaponListGroups)
            {
                weaponListGroup.gameObject.SetActive(false);
            }

            foreach (var cell in weaponListViewCells)
            {
                cell.gameObject.SetActive(false);
            }

            for (var i = 0; i < layouts.Length; i++)
            {
                foreach (var cell in layouts[i])
                {
                    weaponListViewCells[cell.weaponIndex].gameObject.SetActive(true);
                    weaponListViewCells[cell.weaponIndex].transform.SetParent(weaponListGroups[i].transform);
                }

                // GroupサイズをGridLayoutGroupからもらうので後ろに
                weaponListGroups[i].gameObject.SetActive(true);
                weaponListGroups[i].UpdateLayout(layouts[i].First().Item1, layouts[i].Count());
            }
        }

        void RefreshWeaponData()
        {
            var weaponDataList = currentControlActorData.WeaponData.Values.ToArray();
            foreach (var weaponData in weaponDataList)
            {
                weaponListViewCells[weaponData.WeaponIndex].UpdateWeaponData(weaponData);
            }
        }

        void IVariableInventoryView.Initialize(
            GameObject cellPrefab,
            Action<IVariableInventoryCell> onCellClick,
            Action<IVariableInventoryCell> onCellOptionClick,
            Action<IVariableInventoryCell> onCellEnter,
            Action<IVariableInventoryCell> onCellExit)
        {
        }

        void IVariableInventoryView.ReApply()
        {
        }

        void IVariableInventoryView.OnPrePick(IVariableInventoryCell stareCell)
        {
        }

        bool IVariableInventoryView.OnPick(IVariableInventoryCell stareCell)
        {
        }

        void IVariableInventoryView.OnDrag(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell, PointerEventData cursorPosition)
        {
        }

        bool IVariableInventoryView.OnDrop(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
        }

        void IVariableInventoryView.OnDroped(bool isDroped)
        {
        }

        void IVariableInventoryView.OnCellEnter(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
        }

        void IVariableInventoryView.OnCellExit(IVariableInventoryCell stareCell)
        {
        }

        void IVariableInventoryView.OnSwitchRotate(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
        }

    }
}
