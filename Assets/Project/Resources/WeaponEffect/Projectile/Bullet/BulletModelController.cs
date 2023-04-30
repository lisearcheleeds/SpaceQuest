using UnityEngine;

namespace AloneSpace
{
    public class BulletModelController : MonoBehaviour
    {
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] float height;
        [SerializeField] float radius;

        void Awake()
        {
            UpdateMesh();
        }

        [ContextMenu("UpdateMesh")]
        void UpdateMesh()
        {
            var mesh = new Mesh();
            mesh.name = "GenerateMesh";
            mesh.vertices = new Vector3[]
            {
                new (radius * -2, -height, 0),  // 0
                new (radius * -2, -height, 0),  // 1
                new (radius * 2 , -height, 0),  // 2
                new (radius * 2 , -height, 0),  // 3
                new (radius * -2,  height, 0),  // 4
                new (radius * 2,  height, 0),  // 5
                new (radius * -2,  height, 0),  // 6 
                new (radius * 2 ,  height, 0),  // 7
            };

            mesh.uv = new Vector2[]
            {
                new (0, 0),
                new (0, 0.5f),
                new (1, 0),
                new (1, 0.5f),
                new (0, 0.5f),
                new (1, 0.5f),
                new (0, 1),
                new (1, 1),
            };

            mesh.uv2 = new Vector2[]
            {
                new (radius * -2.0f, 0),
                new (0, 0),
                new (radius * -2.0f, 0),
                new (0, 0),
                new (0, 0),
                new (0, 0),
                new (radius * 2.0f, 0),
                new (radius * 2.0f, 0),
            };

            mesh.triangles = new[]
            {
                0, 1, 2, 
                1, 3, 2,
                1, 4, 3, 
                4, 5, 3,
                4, 6, 5, 
                6, 7, 5,
            };

            meshFilter.sharedMesh = mesh;
        }
    }
}
