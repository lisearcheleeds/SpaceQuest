namespace AloneSpace
{
    public class Texture2DPathVO : AssetPath
    {
        public int Id { get; }

        public Texture2DPathVO(int id, string path) : base(path)
        {
            Id = id;
        }
    }
}