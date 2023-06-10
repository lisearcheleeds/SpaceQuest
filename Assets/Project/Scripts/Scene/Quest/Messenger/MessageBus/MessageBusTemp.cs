﻿namespace AloneSpace
{
    public partial class MessageBus
    {
        // 登録系メッセージ
        public MessageBusDefine.RegisterCollision RegisterCollision { get; } = new MessageBusDefine.RegisterCollision();
        public MessageBusDefine.UnRegisterCollision UnRegisterCollision { get; } = new MessageBusDefine.UnRegisterCollision();

        // プレイヤー設定
        public MessageBusDefine.SetOrderUserPlayer SetOrderUserPlayer { get; } = new MessageBusDefine.SetOrderUserPlayer();
        public MessageBusDefine.SetUserPlayer SetUserPlayer { get; } = new MessageBusDefine.SetUserPlayer();
        public MessageBusDefine.SetOrderUserArea SetOrderUserArea { get; } = new MessageBusDefine.SetOrderUserArea();
        public MessageBusDefine.SetUserArea SetUserArea { get; } = new MessageBusDefine.SetUserArea();

        // イベント通知
        public MessageBusDefine.NoticeCollisionEventData NoticeCollisionEventData { get; } = new MessageBusDefine.NoticeCollisionEventData();
        public MessageBusDefine.NoticeCollisionEventEffectData NoticeCollisionEventEffectData { get; } = new MessageBusDefine.NoticeCollisionEventEffectData();

        // Playerによるゲームコマンド
        public MessageBusDefine.PlayerCommandSetAreaId PlayerCommandSetAreaId { get; } = new MessageBusDefine.PlayerCommandSetAreaId();
        public MessageBusDefine.PlayerCommandSetMoveTarget PlayerCommandSetMoveTarget { get; } = new MessageBusDefine.PlayerCommandSetMoveTarget();
        public MessageBusDefine.PlayerCommandSetInteractOrder PlayerCommandSetInteractOrder { get; } = new MessageBusDefine.PlayerCommandSetInteractOrder();
        public MessageBusDefine.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefine.PlayerCommandSetTacticsType();

        // ActorのItem収集
        public MessageBusDefine.ManagerCommandPickItem ManagerCommandPickItem { get; } = new MessageBusDefine.ManagerCommandPickItem();
        public MessageBusDefine.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefine.ManagerCommandTransferItem();

        // Weapon
        public MessageBusDefine.CreateWeaponEffectData CreateWeaponEffectData { get; } = new MessageBusDefine.CreateWeaponEffectData();
        public MessageBusDefine.ReleaseWeaponEffectData ReleaseWeaponEffectData { get; } = new MessageBusDefine.ReleaseWeaponEffectData();
        public MessageBusDefine.AddWeaponEffectData AddWeaponEffectData { get; } = new MessageBusDefine.AddWeaponEffectData();
        public MessageBusDefine.RemoveWeaponEffectData RemoveWeaponEffectData { get; } = new MessageBusDefine.RemoveWeaponEffectData();
    }
}