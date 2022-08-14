using System;
using System.Collections;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class GlobalMapPanel : MonoBehaviour
    {
        // セルの上辺から底辺までの長さ
        // セルの辺の長さ
        // セルのバウンディングボックスサイズ
        // セルを並べる時のオフセット

        public float CellBaseSize => cellTemplate.CellBaseSize;
        public float CellLineSize => CellBaseSize * (1.0f / Mathf.Sqrt(3.0f));
        public Vector2 CellSize => new Vector2(CellLineSize * 2.0f, CellBaseSize);
        public Vector2 CellOffset => new Vector2(CellLineSize * 3.0f, CellBaseSize * 0.5f);

        [SerializeField] float CellMargin = 10.0f;
        [SerializeField] GlobalMapPanelCell cellTemplate;
        [SerializeField] RectTransform cellParent;

        QuestData questData;

        GlobalMapPanelCell[] globalMapPanelCells;

        Coroutine focusCoroutine;
        Vector3 targetFocusPosition;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandGlobalMapFocusCell.AddListener(SetGlobalMapFocusCell);
            
            globalMapPanelCells = new GlobalMapPanelCell[questData.MapData.MapSize];

            for (var i = 0; i < globalMapPanelCells.Length; i++)
            {
                var index = i;
                globalMapPanelCells[index] = Instantiate(cellTemplate, cellParent);
                globalMapPanelCells[index].Initialize(questData, index, clickedIndex => OnClickCell(clickedIndex));
            }
            
            RefreshLayout();
            UpdateView();
        }

        void SetGlobalMapFocusCell(int index, bool immediate)
        {
            targetFocusPosition = -globalMapPanelCells[index].transform.localPosition;

            if (immediate)
            {
                if (focusCoroutine != null)
                {
                    StopCoroutine(focusCoroutine);
                    focusCoroutine = null;
                }
                
                cellParent.localPosition = targetFocusPosition;
            }
            else
            {
                if (focusCoroutine == null && gameObject.activeInHierarchy)
                {
                    focusCoroutine = StartCoroutine(FocusCoroutine());
                }
            }
            
            UpdateView();
        }

        void UpdateView()
        {
            for (var i = 0; i < globalMapPanelCells.Length; i++)
            {
                globalMapPanelCells[i].UpdateView();
            }
        }

        void RefreshLayout()
        {
            var CellMarginX = CellOffset.x + CellMargin * Mathf.Sqrt(3.0f);
            var CellMarginY = CellOffset.y + CellMargin * 0.5f;

            cellParent.sizeDelta = new Vector2(CellMarginX * questData.MapData.MapSizeX + CellMarginX * 0.5f, CellMarginY * questData.MapData.MapSizeY);
            var areaOffset = -cellParent.sizeDelta * 0.5f + new Vector2(CellMarginX, CellMarginY) * 0.5f;

            for (var y = 0; y < questData.MapData.MapSizeY; y++)
            {
                for (var x = 0; x < questData.MapData.MapSizeX; x++)
                {
                    var index = y * questData.MapData.MapSizeX + x;
                    var OddYOffsetX = y % 2 == 1 ? CellMarginX * 0.5f : 0.0f;

                    (globalMapPanelCells[index].transform as RectTransform).localPosition =
                        new Vector3(
                            CellMarginX * x + areaOffset.x + OddYOffsetX,
                            CellMarginY * y + areaOffset.y,
                            0);
                }
            }
        }

        void OnClickCell(int index)
        {
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.Broadcast(questData.ObservePlayer.InstanceId, index);
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(index, false);
        }

        IEnumerator FocusCoroutine()
        {
            while ((cellParent.localPosition - targetFocusPosition).sqrMagnitude > 1.0f)
            {
                cellParent.localPosition += (targetFocusPosition - cellParent.localPosition) * 0.1f;
                yield return null;
            }

            cellParent.localPosition = targetFocusPosition;
            focusCoroutine = null;
        }
    }
}
