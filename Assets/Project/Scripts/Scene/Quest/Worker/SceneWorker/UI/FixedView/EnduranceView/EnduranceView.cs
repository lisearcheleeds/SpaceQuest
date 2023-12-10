using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class EnduranceView : MonoBehaviour
    {
        [SerializeField] Text enduranceText;
        [SerializeField] RectTransform enduranceGaugeRect;
        [SerializeField] RectTransform enduranceGaugeBackEffectRect;
        [SerializeField] Animator enduranceShaker;

        [SerializeField] Text shieldText;
        [SerializeField] RectTransform shieldGaugeRect;
        [SerializeField] RectTransform shieldGaugeBackEffectRect;
        [SerializeField] Animator shieldShaker;

        ActorData userControlActor;

        float prevEnduranceValue;
        float prevEnduranceValueMax;
        float prevShieldValue;
        float prevShieldValueMax;

        public void Initialize()
        {
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
        }

        public void OnUpdate()
        {
            if (userControlActor == null)
            {
                return;
            }

            UpdateEndurance();
            UpdateEnduranceBackEffect();

            UpdateShield();
            UpdateShieldBackEffect();
        }

        void SetUserControlActor(ActorData userControlActor)
        {
            this.userControlActor = userControlActor;
        }

        void UpdateEndurance()
        {
            if (prevEnduranceValue == userControlActor.ActorStateData.EnduranceValue && prevEnduranceValueMax == userControlActor.ActorStateData.EnduranceValueMax)
            {
                return;
            }

            prevEnduranceValue = userControlActor.ActorStateData.EnduranceValue;
            prevEnduranceValueMax = userControlActor.ActorStateData.EnduranceValueMax;

            if (userControlActor.ActorStateData.EnduranceValueMax == 0)
            {
                enduranceGaugeRect.localScale = new Vector3(0, 1.0f, 1.0f);
                enduranceText.text = "";
                return;
            }

            enduranceShaker.SetTrigger(AnimatorKey.Large);

            enduranceGaugeRect.localScale = new Vector3(
                Mathf.Clamp01(userControlActor.ActorStateData.EnduranceValue / userControlActor.ActorStateData.EnduranceValueMax),
                1.0f,
                1.0f);

            enduranceText.text = $"{userControlActor.ActorStateData.EnduranceValue:#,0} / {userControlActor.ActorStateData.EnduranceValueMax:#,0}";
        }

        void UpdateEnduranceBackEffect()
        {
            enduranceGaugeBackEffectRect.localScale = new Vector3(
                Mathf.Lerp(enduranceGaugeBackEffectRect.localScale.x, enduranceGaugeRect.localScale.x, 0.05f),
                1.0f,
                1.0f);
        }

        void UpdateShield()
        {
            if (prevShieldValue == userControlActor.ActorStateData.ShieldValue && prevShieldValueMax == userControlActor.ActorStateData.ShieldValueMax)
            {
                return;
            }

            prevShieldValue = userControlActor.ActorStateData.ShieldValue;
            prevShieldValueMax = userControlActor.ActorStateData.ShieldValueMax;

            if (userControlActor.ActorStateData.ShieldValueMax == 0)
            {
                shieldGaugeRect.localScale = new Vector3(0, 1.0f, 1.0f);
                shieldText.text = "";
                return;
            }

            enduranceShaker.SetTrigger(AnimatorKey.Small);

            shieldGaugeRect.localScale = new Vector3(
                Mathf.Clamp01(userControlActor.ActorStateData.ShieldValue / userControlActor.ActorStateData.ShieldValueMax),
                1.0f,
                1.0f);

            shieldText.text = $"{userControlActor.ActorStateData.ShieldValue:#,0} / {userControlActor.ActorStateData.ShieldValueMax:#,0}";
        }

        void UpdateShieldBackEffect()
        {
            shieldGaugeBackEffectRect.localScale = new Vector3(
                Mathf.Lerp(shieldGaugeBackEffectRect.localScale.x, shieldGaugeRect.localScale.x, 0.05f),
                1.0f,
                1.0f);
        }
    }
}
