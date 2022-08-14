using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class ActorDataView : MonoBehaviour
    {
        public bool IsSelect
        {
            get => isSelect.activeSelf;
            set => isSelect.SetActive(value);
        }

        public ActorData ActorData { get; private set; }

        [SerializeField] Button button;
        [SerializeField] GameObject isSelect;
        
        [SerializeField] Text actorNameText;
        [SerializeField] Slider hitPointGauge;
        [SerializeField] Text hitPointText;
        
        Action onClick;
        float hitPoint;
        
        public void Apply(ActorData actorData, Action onClick)
        {
            ActorData = actorData;
            this.onClick = onClick;
            
            actorNameText.text = "Actor";
        }

        void Update()
        {
            if (ActorData == null)
            {
                return;
            }

            if (CheckDirty())
            {
                hitPointGauge.value = ActorData.HitPoint / ActorData.ActorSpecData.Endurance;
                hitPointText.text = $"{ActorData.HitPoint} / {ActorData.ActorSpecData.Endurance}";
            }
        }

        bool CheckDirty()
        {
            var isDirty = false;
            
            if (hitPoint != ActorData.HitPoint)
            {
                hitPoint = ActorData.HitPoint;
                isDirty = true;
            }

            return isDirty;
        }

        void Awake()
        {
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}
