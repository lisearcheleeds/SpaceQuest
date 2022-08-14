using VariableInventorySystem;

namespace RoboQuest
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
