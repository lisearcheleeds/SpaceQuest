namespace SimpleNavMesh
{
    public interface INavMeshTarget
    {
        NavMeshTargetData NavMeshTargetData { get; }

        void UpdateNavMeshTargetData();
    }
}