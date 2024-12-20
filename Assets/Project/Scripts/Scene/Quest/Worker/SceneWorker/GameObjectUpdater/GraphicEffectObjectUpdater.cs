﻿using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class GraphicEffectObjectUpdater
    {
        AreaData observeArea;
        bool isReset;

        LinkedList<GraphicEffect> graphicEffectList = new LinkedList<GraphicEffect>();
        LinkedList<GraphicEffect> removeList = new LinkedList<GraphicEffect>();

        public void Initialize()
        {
            MessageBus.Instance.Data.SpawnGraphicEffect.AddListener(SpawnGraphicEffect);
            MessageBus.Instance.User.SetObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.Data.SpawnGraphicEffect.RemoveListener(SpawnGraphicEffect);
            MessageBus.Instance.User.SetObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var graphicEffect in graphicEffectList)
            {
                if (graphicEffect.IsCompleted || isReset)
                {
                    removeList.AddLast(graphicEffect);
                    continue;
                }

                graphicEffect.OnLateUpdate(deltaTime);
            }

            foreach (var removeTarget in removeList)
            {
                removeTarget.Release();
                graphicEffectList.Remove(removeTarget);
            }

            removeList.Clear();

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

            MessageBus.Instance.Asset.GetCacheAsset.Broadcast(graphicEffectSpecVO.Path, c =>
            {
                var graphicEffect = (GraphicEffect)c;
                graphicEffect.Init(graphicEffectSpecVO, graphicEffectHandler);
                graphicEffectList.AddLast(graphicEffect);
            });
        }
    }
}
