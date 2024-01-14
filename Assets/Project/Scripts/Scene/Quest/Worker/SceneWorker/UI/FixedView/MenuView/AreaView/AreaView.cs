using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class AreaView : MonoBehaviour
    {
        [SerializeField] AreaViewCell areaViewCellPrefab;
        [SerializeField] RectTransform cellParent;

        bool isDirty;

        List<AreaViewCell> actorCells = new List<AreaViewCell>();
        List<AreaViewCell> interactCells = new List<AreaViewCell>();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.User.SetObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void SetDirty()
        {
            isDirty = true;
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
                    actorCells.Add(Instantiate(areaViewCellPrefab, cellParent));
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
                    interactCells.Add(Instantiate(areaViewCellPrefab, cellParent));
                }

                interactCells[i].gameObject.SetActive(i < currentAreaInteracts.Length);

                if (i < currentAreaInteracts.Length)
                {
                    var position = currentAreaInteracts[i].Position;
                    interactCells[i].Apply(currentAreaInteracts[i], position, OnClickInteractCell);
                }
            }
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
