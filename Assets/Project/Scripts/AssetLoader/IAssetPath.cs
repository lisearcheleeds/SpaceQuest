using VariableInventorySystem;

namespace AloneSpace
{
    public interface IAssetPath : IVariableInventoryAsset
    {
        string Path { get; }
    }
}
