using System.Linq;

namespace RoboQuest
{
    public class MapPresetVO
    {
        public int MapId => mapMaster.Id;
        public int MapSizeX => mapMaster.MapSizeX;
        public int MapSizeY => mapMaster.MapSizeY;
        public int MapSizeZ => mapMaster.MapSizeZ;
        public int MapSize => MapSizeX * MapSizeY * MapSizeZ;

        public AreaAssetVO[] AreaAssetVOs { get; }

        MapMaster.Row mapMaster;
        
        public MapPresetVO(int id)
        {
            mapMaster = MapMaster.Instance.Get(id);
            
            AreaAssetVOs = Enumerable.Range(0, MapSize)
                .Select(i =>
                {
                    var row = AreaMaster.Instance.Get(MapId, i);
                    return new AreaAssetVO(row.AreaId, row.AmbientObjectAssetId, row.PlacedObjectAssetId, row.AreaSize);
                })
                .ToArray();
        }
    }
}