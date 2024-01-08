using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class ActorStatusView : MonoBehaviour
    {
        [SerializeField] Text enduranceText;
        [SerializeField] Text shieldText;
        [SerializeField] Text currentStateText;

        [SerializeField] Text interactOrdersText;

        ActorData actorData;

        bool isDirty;

        float? prevEnduranceValue;
        float? prevEnduranceValueMax;
        float? prevShieldValue;
        float? prevShieldValueMax;
        Guid? prevMainTargetId;
        int? prevInteractOrderCount;
        Guid? prevMoveTargetId;

        public void Initialize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.AddListener(UIMenuStatusViewSelectActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.RemoveListener(UIMenuStatusViewSelectActorData);
        }

        public void SetDirty()
        {
            isDirty = true;
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

                interactOrdersText.text = "";
            }
            else if (actorData.ActorStateData.InteractOrderStateList.Count != 0)
            {
                currentStateText.text = "インタラクト中";
                interactOrdersText.text = string.Join("\n", actorData.ActorStateData.InteractOrderStateList.Select(
                    state => $"・{state.InteractData.Text}\n -> {state.ProgressRatio * 100.0f:F1}%").ToArray());
            }
            else if (actorData.ActorStateData.MoveTarget != null)
            {
                currentStateText.text = $"{actorData.ActorStateData.MoveTarget}に移動中";
                interactOrdersText.text = "";
            }
            else
            {
                currentStateText.text = "";
                interactOrdersText.text = "";
            }
        }

        void UIMenuStatusViewSelectActorData(ActorData actorData)
        {
            this.actorData = actorData;
            SetDirty();
        }

        void SetPrev()
        {
            prevEnduranceValue = actorData?.ActorStateData.EnduranceValue;
            prevEnduranceValueMax = actorData?.ActorStateData.EnduranceValueMax;
            prevShieldValue = actorData?.ActorStateData.ShieldValue;
            prevShieldValueMax = actorData?.ActorStateData.ShieldValueMax;

            prevMainTargetId = actorData?.ActorStateData.MainTarget?.InstanceId;
            prevInteractOrderCount = actorData?.ActorStateData.InteractOrderStateList.Count;
            prevMoveTargetId = actorData?.ActorStateData.MoveTarget?.InstanceId;
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

            if (prevInteractOrderCount != actorData?.ActorStateData.InteractOrderStateList.Count)
            {
                return true;
            }

            if (prevMoveTargetId != actorData?.ActorStateData.MoveTarget?.InstanceId)
            {
                return true;
            }

            return false;
        }
    }
}
