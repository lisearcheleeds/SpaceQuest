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
        AreaData observeAreaData;

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
                    mapPanelCells[i].Apply(areaData, areaData.AreaId == observeAreaData?.AreaId, OnClickCell);

                    mapPanelCells[i].UpdatePosition(MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Unicast(
                        CameraController.CameraType.CameraAmbient,
                        areaData.StarSystemPosition,
                        cellParent));
                }
            }
        }

        public void SetObserveAreaData(AreaData observeAreaData)
        {
            this.observeAreaData = observeAreaData;
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
                    CameraController.CameraType.CameraAmbient,
                    questData.StarSystemData.AreaData[index].StarSystemPosition,
                    cellParent));
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
        }
    }
}
