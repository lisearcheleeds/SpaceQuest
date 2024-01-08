using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class ActorSpecView : MonoBehaviour
    {
        [SerializeField] Text nameText;
        [SerializeField] Text enduranceValueText;

        [SerializeField] Text shieldValueText;
        [SerializeField] Text shieldTruncateValueText;
        [SerializeField] Text shieldAutoRecoveryResilienceTimeText;
        [SerializeField] Text shieldAutoRecoveryValueText;

        [SerializeField] Text electronicProtectionValueText;
        [SerializeField] Text electronicProtectionTruncateValueText;
        [SerializeField] Text electronicProtectionAutoRecoveryResilienceTimeText;
        [SerializeField] Text electronicProtectionAutoRecoveryValueText;

        [SerializeField] Text weaponSlotCountText;

        [SerializeField] Text mainBoosterPowerText;
        [SerializeField] Text subBoosterPowerText;
        [SerializeField] Text maxSpeedText;
        [SerializeField] Text pitchRotatePowerText;
        [SerializeField] Text yawRotatePowerText;
        [SerializeField] Text rollRotatePowerText;

        [SerializeField] Text capacityText;

        [SerializeField] Text visionSensorDistanceText;
        [SerializeField] Text radarSensorPerformanceText;

        ActorData actorData;

        bool isDirty;

        public void Initialize()
        {
            MessageBus.Instance.UserInput.UIMenuStatusViewSelectActorData.AddListener(UIMenuStatusViewSelectActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UIMenuStatusViewSelectActorData.RemoveListener(UIMenuStatusViewSelectActorData);
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }
        }

        void Refresh()
        {
            if (actorData == null)
            {
                nameText.text = "";
                enduranceValueText.text = "";

                shieldValueText.text = "";
                shieldTruncateValueText.text = "";
                shieldAutoRecoveryResilienceTimeText.text = "";
                shieldAutoRecoveryValueText.text = "";

                electronicProtectionValueText.text = "";
                electronicProtectionTruncateValueText.text = "";
                electronicProtectionAutoRecoveryResilienceTimeText.text = "";
                electronicProtectionAutoRecoveryValueText.text = "";

                weaponSlotCountText.text = "";

                mainBoosterPowerText.text = "";
                subBoosterPowerText.text = "";
                maxSpeedText.text = "";
                pitchRotatePowerText.text = "";
                yawRotatePowerText.text = "";
                rollRotatePowerText.text = "";

                capacityText.text = "";

                visionSensorDistanceText.text = "";
                radarSensorPerformanceText.text = "";
                return;
            }

            nameText.text = actorData.ActorSpecVO.Name;
            enduranceValueText.text = $"最大耐久値: {actorData.ActorSpecVO.EnduranceValue}";

            shieldValueText.text = $"最大シールド量: {actorData.ActorSpecVO.ShieldValue}";
            shieldTruncateValueText.text = $"シールド固定減衰量: {actorData.ActorSpecVO.ShieldTruncateValue}";
            shieldAutoRecoveryResilienceTimeText.text = $"シールド自動回復 復旧時間: {actorData.ActorSpecVO.ShieldAutoRecoveryResilienceTime}s";
            shieldAutoRecoveryValueText.text = $"シールド自動回復量: {actorData.ActorSpecVO.ShieldAutoRecoveryValue}/s";

            electronicProtectionValueText.text = $"電子シールド量: {actorData.ActorSpecVO.ElectronicProtectionValue}";
            electronicProtectionTruncateValueText.text = $"電子シールド固定減衰量: {actorData.ActorSpecVO.ElectronicProtectionTruncateValue}";
            electronicProtectionAutoRecoveryResilienceTimeText.text = $"電子シールド自動回復 復旧時間: {actorData.ActorSpecVO.ElectronicProtectionAutoRecoveryResilienceTime}";
            electronicProtectionAutoRecoveryValueText.text = $"電子シールド自動回復量: {actorData.ActorSpecVO.ElectronicProtectionAutoRecoveryValue}";

            weaponSlotCountText.text = $"兵装スロット: {actorData.ActorSpecVO.WeaponSlotCount}";

            mainBoosterPowerText.text = $"メインブースター性能: {actorData.ActorSpecVO.MainBoosterPower}";
            subBoosterPowerText.text = $"サブブースター量: {actorData.ActorSpecVO.SubBoosterPower}";
            maxSpeedText.text = $"最大スピード: {actorData.ActorSpecVO.MaxSpeed}";
            pitchRotatePowerText.text = $"ピッチ回転性能: {actorData.ActorSpecVO.PitchRotatePower}";
            yawRotatePowerText.text = $"ヨー回転性能: {actorData.ActorSpecVO.YawRotatePower}";
            rollRotatePowerText.text = $"ロール回転性能: {actorData.ActorSpecVO.RollRotatePower}";

            capacityText.text = $"カーゴ容量: 横{actorData.ActorSpecVO.CapacityWidth}スロット : 縦{actorData.ActorSpecVO.CapacityHeight}スロット";

            visionSensorDistanceText.text = $"視界距離{actorData.ActorSpecVO.VisionSensorDistance}";
            radarSensorPerformanceText.text = $"レーダー距離{actorData.ActorSpecVO.RadarSensorPerformance}";
        }

        void UIMenuStatusViewSelectActorData(ActorData actorData)
        {
            this.actorData = actorData;
            SetDirty();
        }
    }
}
