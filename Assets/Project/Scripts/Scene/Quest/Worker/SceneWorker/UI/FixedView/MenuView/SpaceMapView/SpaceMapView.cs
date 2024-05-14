using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace.UI
{
    public class SpaceMapView : MonoBehaviour
    {
        [FormerlySerializedAs("areaViewCellPrefab")] [SerializeField] SpaceMapViewCell spaceMapViewCellPrefab;
        [SerializeField] RectTransform cellParent;

        [SerializeField] LineRenderer lineTemplate;
        [SerializeField] Transform lineParent;
        
        bool isDirty;

        SpaceMapInputLayer spaceMapInputLayer;

        List<LineRenderer> axisLines = new List<LineRenderer>();
        
        List<SpaceMapViewCell> actorCells = new List<SpaceMapViewCell>();
        List<SpaceMapViewCell> interactCells = new List<SpaceMapViewCell>();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            spaceMapInputLayer = new SpaceMapInputLayer(questData.UserData);
            
            MessageBus.Instance.User.SetObserveArea.AddListener(SetUserObserveArea);
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.AddListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.AddListener(UserInputCloseSpaceMapView);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetObserveArea.RemoveListener(SetUserObserveArea);
            MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.RemoveListener(UserInputOpenSpaceMapView);
            MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.RemoveListener(UserInputCloseSpaceMapView);
        }

        public void OnUpdate()
        {
            if (!isDirty || questData == null)
            {
                return;
            }

            isDirty = false;

            var currentAreaActors = questData.ActorData.Values.Where(actor => actor.AreaId == questData.UserData.ObserveAreaData?.AreaId).ToArray();
            for (var i = 0; i < Mathf.Max(actorCells.Count, currentAreaActors.Length); i++)
            {
                if (actorCells.Count < i + 1)
                {
                    actorCells.Add(Instantiate(spaceMapViewCellPrefab, cellParent));
                }

                actorCells[i].gameObject.SetActive(i < currentAreaActors.Length);

                if (i < currentAreaActors.Length)
                {
                    var position = currentAreaActors[i].Position;
                    actorCells[i].Apply(currentAreaActors[i], position, OnClickActorCell);
                }
            }
            
            var currentAreaInteracts = questData.InteractData.Values.Where(actor => actor.AreaId == questData.UserData.ObserveAreaData?.AreaId).ToArray();
            for (var i = 0; i < Mathf.Max(interactCells.Count, currentAreaInteracts.Length); i++)
            {
                if (interactCells.Count < i + 1)
                {
                    interactCells.Add(Instantiate(spaceMapViewCellPrefab, cellParent));
                }

                interactCells[i].gameObject.SetActive(i < currentAreaInteracts.Length);

                if (i < currentAreaInteracts.Length)
                {
                    var position = currentAreaInteracts[i].Position;
                    interactCells[i].Apply(currentAreaInteracts[i], position, OnClickInteractCell);
                }
            }

            UpdateAxisLine();
        }
        
        void SetDirty()
        {
            isDirty = true;
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
                    axisLines.Add(Instantiate(lineTemplate, lineParent));
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
                    axisLines.Add(Instantiate(lineTemplate, lineParent));
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
            SetDirty();

            InputLayerController.Instance.PushLayer(spaceMapInputLayer);
        }

        void UserInputCloseSpaceMapView()
        {
            gameObject.SetActive(false);

            InputLayerController.Instance.PopLayer(spaceMapInputLayer);
        }

        void SetUserObserveArea(AreaData areaData)
        {
            SetDirty();
        }

        void OnClickActorCell(ActorData actorData)
        {
        }

        void OnClickInteractCell(IInteractData interactData)
        {
        }
    }
}
