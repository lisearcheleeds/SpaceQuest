using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class GraphicEffectObjectUpdater
    {
        AreaData observeArea;
        bool isReset;

        List<GraphicEffect> graphicEffectList = new List<GraphicEffect>();

        public void Initialize()
        {
            MessageBus.Instance.SpawnGraphicEffect.AddListener(SpawnGraphicEffect);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SpawnGraphicEffect.RemoveListener(SpawnGraphicEffect);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
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

        void SetUserObserveArea(AreaData areaData)
        {
            this.observeArea = areaData;
            ResetGraphicEffectObjectList();
        }

        void ResetGraphicEffectObjectList()
        {
            isReset = true;
        }

        void SpawnGraphicEffect(GraphicEffectSpecVO graphicEffectSpecVO, IGraphicEffectHandler graphicEffectHandler)
        {
            if (graphicEffectHandler.PositionData.AreaId != observeArea?.AreaId)
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
