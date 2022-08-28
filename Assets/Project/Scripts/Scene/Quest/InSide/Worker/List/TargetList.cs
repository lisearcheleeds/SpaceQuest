using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class TargetList
    {
        bool isDirty = false;
        List<ITarget> targetList = new List<ITarget>();

        public void Initialize()
        {
            MessageBus.Instance.SendTarget.AddListener(OnReceiveTarget);
            MessageBus.Instance.SubscribeUpdateAll.AddListener(SubscribeUpdateAll);
        }

        public void LateUpdate()
        {
            if (isDirty)
            {
                MessageBus.Instance.SubscribeUpdateTargetList.Broadcast(targetList.ToArray());
                isDirty = false;
            }
        }

        public void Finalize()
        {
            MessageBus.Instance.SendTarget.RemoveListener(OnReceiveTarget);
            MessageBus.Instance.SubscribeUpdateAll.RemoveListener(SubscribeUpdateAll);
        }

        public void OnLoadedArea()
        {
            isDirty = true;
        }

        void OnReceiveTarget(ITarget entryTarget, bool isEntry)
        {
            if (isEntry)
            {
                targetList.Add(entryTarget);
            }
            else
            {
                targetList.Remove(entryTarget);
            }
            
            isDirty = true;
        }

        void SubscribeUpdateAll()
        {
            isDirty = true;
        }
    }
}
