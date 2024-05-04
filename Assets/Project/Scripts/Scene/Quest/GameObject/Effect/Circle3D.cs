using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.Common
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]  
    public class Circle3D : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField, Min(2)] int division = 2;
        [SerializeField, Range(0f,1f)] float fillOrigin;
        [SerializeField, Range(0f,1f)] float fillAmount = 1.0f;
        
        [SerializeField, Min(1)] int beltDivision = 1;
        [SerializeField, Range(-1.0f,1.0f)] float beltFillOrigin;
        [SerializeField, Range(0f,1f)] float beltFillAmount = 1.0f;
        
        [SerializeField] bool isFront = true;
        [SerializeField] bool isBack = true;

        [SerializeField] Color color = Color.white;

        [SerializeField, HideInInspector] Mesh mesh;
        
        bool isInit;
        MeshFilter meshFilter;

        public void Apply()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Init();
            
            mesh = MakeMesh();
            Refresh();
        }

        public void Refresh()
        {
            meshFilter.mesh = mesh;
        }

        void Init()
        {
            if (isInit)
            {
                return;
            }

            meshFilter = GetComponent<MeshFilter>();

            isInit = true;
        }

        void OnValidate()
        {
            Apply();
        }

        void OnEnable()
        {
            Apply();
        }

        Mesh MakeMesh()
        {
            var mesh = new Mesh();
            mesh.name = "Circle3D";
            
            // 1divごとの角度
            var divRad = Mathf.PI * 2f * fillAmount / (division + 1);
            var beltWidth = Mathf.PI * beltFillAmount;
            var beltDivRad = Mathf.PI * beltFillAmount / beltDivision;
            
            var vertices = new List<Vector3>();
            var uv = new List<Vector2>();
            var triangles = new List<int>();
            
            for (var b = 0; b < beltDivision + 1; b++)
            {
                for (var i = 0; i < division + 2; i++)
                {
                    var originOffset = Mathf.PI * 2f * fillOrigin;
                    var beltOriginOffset = Mathf.PI * 2f * beltFillOrigin * 0.5f * ((1.0f - beltFillAmount) / 2.0f);
                    var heightScale = Mathf.Cos(beltDivRad * b - beltWidth / 2.0f + beltOriginOffset);
                    vertices.Add(new Vector3(
                        radius * Mathf.Cos(divRad * i + originOffset) * heightScale,
                        radius * Mathf.Sin(divRad * i + originOffset) * heightScale,
                        radius * Mathf.Sin(beltDivRad * b - beltWidth / 2.0f + beltOriginOffset)));

                    uv.Add(new Vector2((b * beltFillAmount) / (beltDivision + 1) - beltFillAmount * 0.5f, (i * fillAmount) / (division + 2)));

                    if (b < beltDivision && i < division + 1)
                    {
                        var offset = (division + 2) * b;

                        if (isFront)
                        {
                            triangles.Add(offset + i);
                            triangles.Add(offset + i + 1);
                            triangles.Add(offset + i + division + 2);

                            triangles.Add(offset + i + division + 2);
                            triangles.Add(offset + i + 1);
                            triangles.Add(offset + i + division + 3);
                        }

                        if (isBack)
                        {
                            triangles.Add(offset + i);
                            triangles.Add(offset + i + division + 2);
                            triangles.Add(offset + i + 1);

                            triangles.Add(offset + i + division + 2);
                            triangles.Add(offset + i + division + 3);
                            triangles.Add(offset + i + 1);
                        }
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.colors = uv.Select(_ => color).ToArray();
            mesh.triangles = triangles.ToArray();
            
            return mesh;
        }
    }
}