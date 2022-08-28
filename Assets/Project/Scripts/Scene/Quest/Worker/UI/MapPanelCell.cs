using UnityEngine;

namespace AloneSpace
{
    public class MapPanelCell : MonoBehaviour
    {
        [SerializeField] MeshRenderer mesh;

        Material material;
        
        public void Initialize()
        {
            material = mesh.material;
        }

        public void UpdateView(Color color)
        {
            material.color = color;
        }
    }
}
