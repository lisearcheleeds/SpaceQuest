using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class SpaceMapView : MonoBehaviour
    {
        [SerializeField] SpaceMapViewCell spaceMapViewCellPrefab;

        [SerializeField] Transform parent;
        [SerializeField] LineRenderer lineTemplate;
        
        SpaceMapInputLayer spaceMapInputLayer;

        List<LineRenderer> axisLines = new List<LineRenderer>();
        
        List<SpaceMapViewCell> cells = new List<SpaceMapViewCell>();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            spaceMapInputLayer = new SpaceMapInputLayer(questData.UserData);
            
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.AddListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.AddListener(UserInputCloseSpaceMapView);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.RemoveListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.RemoveListener(UserInputCloseSpaceMapView);
        }

        public void OnUpdate()
        {
            if (questData == null || !gameObject.activeSelf)
            {
                return;
            }

            // Actorのセルを生成
            var currentAreaActors = questData.ActorData.Values.Where(actor => actor.AreaId == questData.UserData.ObserveAreaData?.AreaId).ToArray();
            var needCellCount = Mathf.Max(cells.Count, currentAreaActors.Length);
            for (var i = 0; i < needCellCount; i++)
            {
                if (cells.Count < i + 1)
                {
                    cells.Add(Instantiate(spaceMapViewCellPrefab, parent));
                }

                cells[i].gameObject.SetActive(i < currentAreaActors.Length);

                if (i < currentAreaActors.Length)
                {
                    cells[i].Apply(currentAreaActors[i], 
                        questData.UserData.ControlActorData.InstanceId == currentAreaActors[i].InstanceId,
                        questData.UserData.PlayerData.InstanceId == currentAreaActors[i].PlayerInstanceId);
                }
            }
            
            // Interactのセルを生成
            var currentAreaInteracts = questData.InteractData.Values.Where(actor => actor.AreaId == questData.UserData.ObserveAreaData?.AreaId).ToArray();
            var allNeedCellCount = Mathf.Max(cells.Count, currentAreaActors.Length + currentAreaInteracts.Length);
            for (var i = needCellCount; i < allNeedCellCount; i++)
            {
                if (cells.Count < i + 1)
                {
                    cells.Add(Instantiate(spaceMapViewCellPrefab, parent));
                }

                cells[i].gameObject.SetActive(i < currentAreaInteracts.Length);

                if (i < currentAreaInteracts.Length)
                {
                    cells[i].Apply(currentAreaInteracts[i]);
                }
            }

            UpdateAxisLine();
        }

        void UpdateAxisLine()
        {
            var scale = 100.0f;
            var gridCount = 10;
            var lineCount = gridCount * 2 + 1;
            for (var x = 0; x < lineCount; x++)
            {
                if (axisLines.Count <= x)
                {
                    axisLines.Add(Instantiate(lineTemplate, parent));
                }

                axisLines[x].positionCount = 2;
                axisLines[x].SetPositions(new[] { new Vector3((gridCount - x) * scale, 0, scale * gridCount), new Vector3((gridCount - x) * scale, 0, -scale * gridCount) });
                axisLines[x].startColor = x == gridCount ? new Color(0.5f, 0.5f, 0.5f, 1.0f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
                axisLines[x].endColor = x == gridCount ? new Color(0.5f, 0.5f, 0.5f, 1.0f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            
            for (var z = 0; z < lineCount; z++)
            {
                if (axisLines.Count <= z + lineCount)
                {
                    axisLines.Add(Instantiate(lineTemplate, parent));
                }

                axisLines[z + lineCount].positionCount = 2;
                axisLines[z + lineCount].SetPositions(new[] { new Vector3(scale * gridCount, 0, (gridCount - z) * scale), new Vector3(-scale * gridCount, 0, (gridCount - z) * scale) });
                axisLines[z + lineCount].startColor = z == gridCount ? new Color(0.5f, 0.5f, 0.5f, 1.0f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
                axisLines[z + lineCount].endColor = z == gridCount ? new Color(0.5f, 0.5f, 0.5f, 1.0f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
        
        void UserInputOpenSpaceMapView()
        {
            gameObject.SetActive(true);

            InputLayerController.Instance.PushLayer(spaceMapInputLayer);
            MessageBus.Instance.UserInput.UserCommandSetCameraGroupType.Broadcast(CameraGroupType.SpaceMap);
        }

        void UserInputCloseSpaceMapView()
        {
            gameObject.SetActive(false);

            InputLayerController.Instance.PopLayer(spaceMapInputLayer);
            MessageBus.Instance.UserInput.UserCommandSetCameraGroupType.Broadcast(CameraGroupType.Space);
        }
    }
}
