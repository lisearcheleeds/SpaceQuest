using System;
using System.Linq;

namespace AloneSpace
{
    public class PlayerPresetVO
    {
        public ActorPresetVO[] ActorPresetVOs { get; }

        public PlayerPresetVO(int playerPresetId)
        {
            var playerPresetMasters = PlayerPresetMaster.Instance.GetRange(playerPresetId);
            ActorPresetVOs = playerPresetMasters.OrderBy(x => x.Index).Select(x => new ActorPresetVO(x.ActorPresetId)).ToArray();
        }
    }
}
