using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class ActorStatusView : MonoBehaviour
    {
        [SerializeField] Text enduranceText;
        [SerializeField] Text shieldText;
        [SerializeField] Text currentStateText;
        [SerializeField] GameObject progressGaugeObject;
        [SerializeField] RectTransform progressGauge;

        ActorData actorData;

        bool isDirty;

        float? prevEnduranceValue;
        float? prevEnduranceValueMax;
        float? prevShieldValue;
        float? prevShieldValueMax;
        Guid? prevMainTargetId;
        Guid? prevInteractOrderId;
        Guid? prevMoveTargetId;
        float? prevInteractingTime;

        public void Initialize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.AddListener(UIMenuStatusViewSelectActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.RemoveListener(UIMenuStatusViewSelectActorData);
        }

        public void OnUpdate()
        {
            isDirty |= CheckDirty();

            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }
        }

        void Refresh()
        {
            SetPrev();

            if (actorData == null)
            {
                enduranceText.text = "";
                shieldText.text = "";
                currentStateText.text = "";

                progressGaugeObject.SetActive(false);
                progressGauge.localScale = Vector3.one;
                return;
            }

            enduranceText.text = $"耐久値: {actorData.ActorStateData.EnduranceValue:#,0} / {actorData.ActorStateData.EnduranceValueMax:#,0}";
            shieldText.text = $"シールド耐久値: {actorData.ActorStateData.ShieldValue:#,0} / {actorData.ActorStateData.ShieldValueMax:#,0}";

            if (actorData.ActorStateData.MainTarget != null)
            {
                if (actorData.ActorStateData.MainTarget is ActorData mainTargetActorData)
                {
                    currentStateText.text = $"戦闘中:{mainTargetActorData.ActorSpecVO.Name}をターゲット中";
                }
                else
                {
                    currentStateText.text = $"戦闘中";
                }

                progressGaugeObject.SetActive(false);
                progressGauge.localScale = Vector3.one;
            }
            else if (actorData.ActorStateData.InteractOrder != null)
            {
                currentStateText.text = $"{actorData.ActorStateData.InteractOrder.Text}にインタラクト中";

                progressGaugeObject.SetActive(true);
                var interactProgress = Mathf.Clamp01(actorData.ActorStateData.CurrentInteractingTime / actorData.ActorStateData.InteractOrder.InteractTime);
                progressGauge.localScale = new Vector3(interactProgress, 1.0f, 1.0f);
            }
            else if (actorData.ActorStateData.MoveTarget != null)
            {
                currentStateText.text = $"{actorData.ActorStateData.MoveTarget}に移動中";

                progressGaugeObject.SetActive(false);
                progressGauge.localScale = Vector3.one;
            }
            else
            {
                currentStateText.text = "";

                progressGaugeObject.SetActive(false);
                progressGauge.localScale = Vector3.one;
            }
        }

        void UIMenuStatusViewSelectActorData(ActorData actorData)
        {
            this.actorData = actorData;
            isDirty = true;
        }

        void SetPrev()
        {
            prevEnduranceValue = actorData?.ActorStateData.EnduranceValue;
            prevEnduranceValueMax = actorData?.ActorStateData.EnduranceValueMax;
            prevShieldValue = actorData?.ActorStateData.ShieldValue;
            prevShieldValueMax = actorData?.ActorStateData.ShieldValueMax;

            prevMainTargetId = actorData?.ActorStateData.MainTarget?.InstanceId;
            prevInteractOrderId = actorData?.ActorStateData.InteractOrder?.InstanceId;
            prevMoveTargetId = actorData?.ActorStateData.MoveTarget?.InstanceId;
            prevInteractingTime = actorData?.ActorStateData.CurrentInteractingTime;
        }

        bool CheckDirty()
        {
            if (prevEnduranceValue != actorData?.ActorStateData.EnduranceValue ||
                prevEnduranceValueMax != actorData?.ActorStateData.EnduranceValueMax ||
                prevShieldValue != actorData?.ActorStateData.ShieldValue ||
                prevShieldValueMax != actorData?.ActorStateData.ShieldValueMax)
            {
                return true;
            }

            if (prevMainTargetId != actorData?.ActorStateData.MainTarget?.InstanceId)
            {
                return true;
            }

            if (prevInteractOrderId != actorData?.ActorStateData.InteractOrder?.InstanceId)
            {
                return true;
            }

            if (prevMoveTargetId != actorData?.ActorStateData.MoveTarget?.InstanceId)
            {
                return true;
            }

            if (prevInteractingTime != actorData?.ActorStateData.CurrentInteractingTime)
            {
                return true;
            }

            return false;
        }
    }
}
