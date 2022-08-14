using System;
using System.Collections.Generic;
using System.Linq;
using RoboQuest.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class GlobalMapPanelCell : MonoBehaviour
    {
        const float CellDefaultBaseSize = 100.0f;
        public float CellBaseSize => cellBaseSize;

        [SerializeField] float cellBaseSize = CellDefaultBaseSize;
        [SerializeField] Button button;

        [SerializeField] Color defaultBaseColor;
        [SerializeField] Color disableBaseColor;

        [SerializeField] Color defaultFrameColor;
        [SerializeField] Color currentFrameColor;
        [SerializeField] Color destinateFrameColor;
        [SerializeField] Color disableFrameColor;

        [SerializeField] Image[] baseImages;
        [SerializeField] Image[] frameImages;
        [SerializeField] GameObject[] directions;

        [SerializeField] Text text;
        
        [SerializeField] GameObject actorMarkerTemplate;
        [SerializeField] RectTransform actorMarkerParent;

        QuestData questData;
        int index;
        Action<int> onClick;

        List<RectTransform> actorMarkers = new List<RectTransform>();
        
        public void Initialize(QuestData questData, int index, Action<int> onClick)
        {
            this.questData = questData;
            this.onClick = onClick;
            this.index = index;

            foreach (var direction in directions)
            {
                direction.SetActive(false);
            }
        }

        public void UpdateView()
        {
            text.text = $"{index}";

            foreach (var baseImage in baseImages)
            {
                if (questData.MapData.AreaData[index] != null)
                {
                    baseImage.color = defaultBaseColor;
                }
                else
                {
                    baseImage.color = disableBaseColor;
                }
            }

            foreach (var frameImage in frameImages)
            {
                if (questData.MapData.AreaData[index] != null && questData.ObservePlayer != null)
                {
                    if (questData.ObserveActor.CurrentAreaIndex == index)
                    {
                        frameImage.color = currentFrameColor;
                    }
                    else if (questData.ObservePlayer.DestinateAreaIndex == index)
                    {
                        frameImage.color = destinateFrameColor;
                    }
                    else
                    {
                        frameImage.color = defaultFrameColor;
                    }
                }
                else
                {
                    frameImage.color = disableFrameColor;
                }
            }

            var wayData = questData.ObserveActor.GetRouteAreaData()?.FirstOrDefault(x => x.AreaIndex == index);
            for (var i = 0; i < directions.Length; i++)
            {
                if (wayData != null && wayData.Direction.HasValue)
                {
                    directions[i].SetActive(i == (int)wayData.Direction.Value);
                }
                else
                {
                    directions[i].SetActive(false);
                }
            }
        }

        void Awake()
        {
            button.onClick.AddListener(() => onClick?.Invoke(index));
        }

        /// <summary>
        /// FIXME 重い
        /// </summary>
        void Update()
        {
            if (questData == null)
            {
                return;
            }

            var actors = questData.ActorData.Where(x => x.CurrentAreaIndex == index && x.ActorState == ActorState.Running).ToArray();
            
            for (var i = actorMarkers.Count; i < actors.Length; i++)
            {
                actorMarkers.Add(Instantiate(actorMarkerTemplate, actorMarkerParent, false).transform as RectTransform);
            }

            var cellScale = cellBaseSize / CellDefaultBaseSize;
            
            for (var i = 0; i < actorMarkers.Count; i++)
            {
                actorMarkers[i].gameObject.SetActive(i < actors.Length);
                
                if (i >= actors.Length)
                {
                    continue;
                }

                actorMarkers[i].localPosition = new Vector3(actors[i].Position.x, actors[i].Position.z, 0) * 0.5f * cellScale;
            }
        }
    }
}
