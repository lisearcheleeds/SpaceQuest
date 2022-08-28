using RoboQuest;

namespace AloneSpace
{
    public class RouteAreaData
    {
        public int AreaIndex { get; }
        public AreaDirection? Direction { get; }

        public RouteAreaData(int areaIndex, AreaDirection? direction)
        {
            AreaIndex = areaIndex;
            Direction = direction;
        }
    }
}
