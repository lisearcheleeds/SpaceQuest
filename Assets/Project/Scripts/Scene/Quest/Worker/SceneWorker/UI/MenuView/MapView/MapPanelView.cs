﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class MapPanelView : MonoBehaviour
    {
        [SerializeField] AreaDataCell cellTemplate;
        [SerializeField] RectTransform cellParent;

        QuestData questData;
        AreaData observeArea;

        List<AreaDataCell> mapPanelCells = new List<AreaDataCell>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        void UpdateView()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            for (var i = 0; i < Mathf.Max(mapPanelCells.Count, questData.StarSystemData.AreaData.Length); i++)
            {
                if (mapPanelCells.Count < i + 1)
                {
                    mapPanelCells.Add(Instantiate(cellTemplate, cellParent));
                }

                mapPanelCells[i].gameObject.SetActive(i < questData.StarSystemData.AreaData.Length);

                if (i < questData.StarSystemData.AreaData.Length)
                {
                    var areaData = questData.StarSystemData.AreaData[i];
                    mapPanelCells[i].Apply(areaData, areaData.AreaId == observeArea?.AreaId, OnClickCell);

                    mapPanelCells[i].UpdatePosition(MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Unicast(
                        CameraType.CameraAmbient,
                        areaData.StarSystemPosition,
                        cellParent));
                }
            }
        }

        void SetUserObserveArea(AreaData areaData)
        {
            observeArea = areaData;
        }

        void UpdatePosition()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            for (var i = 0; i < questData.StarSystemData.AreaData.Length; i++)
            {
                var index = i;
                mapPanelCells[index].UpdatePosition(MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Unicast(
                    CameraType.CameraAmbient,
                    questData.StarSystemData.AreaData[index].StarSystemPosition,
                    cellParent));
            }
        }

        void OnClickCell(AreaData areaData)
        {
        }
    }
}