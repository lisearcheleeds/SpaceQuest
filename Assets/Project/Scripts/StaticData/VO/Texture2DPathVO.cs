namespace RoboQuest
{
    public class Texture2DPathVO : IAssetPath
    {
        public int Id { get; }
        public string Path { get; }

        public Texture2DPathVO(int id, string path)
        {
            Id = id;
            Path = path;
        }
    }
}