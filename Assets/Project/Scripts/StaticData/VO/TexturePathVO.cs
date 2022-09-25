using VariableInventorySystem;

namespace AloneSpace
{
    public class TexturePathVO : IVariableInventoryAsset
    {
        public string Path { get; private set; }

        public TexturePathVO(string path)
        {
            Path = path;
        }
    }
}
