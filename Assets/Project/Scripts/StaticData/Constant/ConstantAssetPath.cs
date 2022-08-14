﻿using RoboQuest.Quest;

namespace RoboQuest
{
    public static class ConstantAssetPath
    {
        public static ConstantAssetPathVO ActorPathVO = new ConstantAssetPathVO("Actor/Actor");
        
        public static ConstantAssetPathVO ItemObjectPathVO = new ConstantAssetPathVO("AreaObjects/ItemObject");
        public static ConstantAssetPathVO AreaTransitionObjectPathVO = new ConstantAssetPathVO("AreaObjects/AreaTransitionObject");
        public static ConstantAssetPathVO BrokenActorObjectPathVO = new ConstantAssetPathVO("AreaObjects/BrokenActorObject");
        public static ConstantAssetPathVO InventoryObjectPathVO = new ConstantAssetPathVO("AreaObjects/InventoryObject");
    }
}