using AloneSpace;

namespace RoboQuest
{
    public class ConstantAssetPathVO : ICacheableGameObjectPath
    {
        public string Path { get; }

        public ConstantAssetPathVO(string path)
        {
            Path = path;
        }
    }
}