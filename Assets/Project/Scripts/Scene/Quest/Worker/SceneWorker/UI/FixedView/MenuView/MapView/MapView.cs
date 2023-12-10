using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace.UI
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] AreaDataCell areaDataCellPrefab;
        [SerializeField] RectTransform cellParent;

        bool isDirty;

        List<AreaDataCell> areaDataCells = new List<AreaDataCell>();

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
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

            isDirty = true;

            for (var i = 0; i < Mathf.Max(areaDataCells.Count, questData.StarSystemData.AreaData.Length); i++)
            {
                if (areaDataCells.Count < i + 1)
                {
                    areaDataCells.Add(Instantiate(areaDataCellPrefab, cellParent));
                }

                areaDataCells[i].gameObject.SetActive(i < questData.StarSystemData.AreaData.Length);

                if (i < questData.StarSystemData.AreaData.Length)
                {
                    var areaData = questData.StarSystemData.AreaData[i];
                    areaDataCells[i].Apply(
                        areaData,
                        areaData.AreaId == questData.UserData.ObserveAreaData?.AreaId,
                        GetScreenPositionFromStarSystemPosition(areaData.StarSystemPosition),
                        OnClickCell);
                }
            }
        }

        void SetUserObserveArea(AreaData areaData)
        {
            SetDirty();
        }

        void OnClickCell(AreaData areaData)
        {
        }

        Vector3 GetScreenPositionFromStarSystemPosition(Vector3 starSystemPosition)
        {
            return new Vector3(starSystemPosition.x, starSystemPosition.z, 0);
        }
    }
}
