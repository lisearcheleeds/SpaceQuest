using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class MapPanelView : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        [SerializeField] Button closeMapButton;
        
        [SerializeField] AreaDataCell cellTemplate;
        [SerializeField] RectTransform cellParent;

        QuestData questData;

        List<AreaDataCell> mapPanelCells = new List<AreaDataCell>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            Close();
            closeMapButton.onClick.AddListener(OnClickCloseMap);
            
            MessageBus.Instance.UserCommandSetCameraAngle.AddListener(SetCameraAngle);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            UpdateView();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            UpdateView();
        }

        void OnClickCloseMap()
        {
            Close();
        }
        
        public void UpdateView()
        {
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
                    mapPanelCells[i].Apply(areaData, areaData.AreaId == questData.ObserveAreaData.AreaId, OnClickCell);
                    
                    MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Broadcast(
                        CameraController.CameraType.CameraAmbient,
                        areaData.Position,
                        cellParent,
                        screenPos => mapPanelCells[i].UpdatePosition(screenPos));
                }
            }
        }

        void UpdatePosition()
        {
            for (var i = 0; i < questData.StarSystemData.AreaData.Length; i++)
            {
                MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Broadcast(
                    CameraController.CameraType.CameraAmbient,
                    questData.StarSystemData.AreaData[i].Position,
                    cellParent,
                    screenPos => mapPanelCells[i].UpdatePosition(screenPos));
            }
        }

        void OnClickCell(AreaData areaData)
        {
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(areaData.AreaId, false);
        }

        void SetCameraAngle(Quaternion quaternion)
        {
            UpdatePosition();
        }
    }
}
