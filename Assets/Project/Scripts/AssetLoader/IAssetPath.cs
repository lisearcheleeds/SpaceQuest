using VariableInventorySystem;

namespace RoboQuest
{
    public interface IAssetPath : IVariableInventoryAsset
    {
        string Path { get; }
    }
}
