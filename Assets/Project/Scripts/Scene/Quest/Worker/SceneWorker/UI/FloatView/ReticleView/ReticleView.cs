using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ReticleView : MonoBehaviour
    {
        [SerializeField] RectTransform baseReticle;
        
        [SerializeField] RectTransform bulletReticle;
        [SerializeField] RectTransform rocketReticle;
        [SerializeField] RectTransform missileReticle;
        [SerializeField] RectTransform dotReticle;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
        }
    }
}
