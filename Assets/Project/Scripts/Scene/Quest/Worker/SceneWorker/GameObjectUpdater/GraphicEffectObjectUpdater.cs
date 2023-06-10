using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class GraphicEffectObjectUpdater
    {
        QuestData questData;
        AreaData observeAreaData;
        bool isReset;

        List<GraphicEffect> graphicEffectList = new List<GraphicEffect>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SpawnGraphicEffect.AddListener(SpawnGraphicEffect);
            MessageBus.Instance.SetUserArea.AddListener(SetUserArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SpawnGraphicEffect.RemoveListener(SpawnGraphicEffect);
            MessageBus.Instance.SetUserArea.RemoveListener(SetUserArea);
        }

        public void OnLateUpdate(float deltaTime)
        {
            // TODO: ToArray辞めたいが
            foreach (var graphicEffect in graphicEffectList.ToArray())
            {
                if (graphicEffect.IsCompleted || isReset)
                {
                    graphicEffect.Release();
                    graphicEffectList.Remove(graphicEffect);
                    continue;
                }

                graphicEffect.OnLateUpdate(deltaTime);
            }

            isReset = false;
        }

        void SetUserArea(AreaData areaData)
        {
            this.observeAreaData = areaData;
            ResetGraphicEffectObjectList();
        }

        void ResetGraphicEffectObjectList()
        {
            isReset = true;
        }

        void SpawnGraphicEffect(GraphicEffectSpecVO graphicEffectSpecVO, IGraphicEffectHandler graphicEffectHandler)
        {
            if (graphicEffectHandler.PositionData.AreaId != observeAreaData?.AreaId)
            {
                return;
            }

            MessageBus.Instance.GetCacheAsset.Broadcast(graphicEffectSpecVO.Path, c =>
            {
                var graphicEffect = (GraphicEffect)c;
                graphicEffect.Init(graphicEffectSpecVO, graphicEffectHandler);
                graphicEffectList.Add(graphicEffect);
            });
        }
    }
}