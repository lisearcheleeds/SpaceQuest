using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class MapPanelView : MonoBehaviour
    {
        [SerializeField] AreaDataCell cellTemplate;
        [SerializeField] RectTransform cellParent;

        QuestData questData;

        List<AreaDataCell> mapPanelCells = new List<AreaDataCell>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserInputSwitchMap.AddListener(UserInputSwitchMap);
            MessageBus.Instance.UserInputOpenMap.AddListener(UserInputOpenMap);
            MessageBus.Instance.UserInputCloseMap.AddListener(UserInputCloseMap);

            MessageBus.Instance.UserCommandSetCameraAngle.AddListener(SetCameraAngle);

            UserInputCloseMap();
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
                var index = i;
                MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Broadcast(
                    CameraController.CameraType.CameraAmbient,
                    questData.StarSystemData.AreaData[index].Position,
                    cellParent,
                    screenPos => mapPanelCells[index].UpdatePosition(screenPos));
            }
        }

        void OnClickCell(AreaData areaData)
        {
        }

        void SetCameraAngle(Quaternion quaternion)
        {
            UpdatePosition();
        }

        void UserInputSwitchMap()
        {
            if (gameObject.activeSelf)
            {
                MessageBus.Instance.UserInputCloseMap.Broadcast();
            }
            else
            {
                MessageBus.Instance.UserInputOpenMap.Broadcast();
            }
        }

        void UserInputOpenMap()
        {
            MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Map);
            
            gameObject.SetActive(true);
            UpdateView();
        }
        
        void UserInputCloseMap()
        {
            MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraController.CameraMode.Default);
            
            gameObject.SetActive(false);
            UpdateView();
        }
    }
}
