using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField] Material robotMaterial;
    [SerializeField] MeshRenderer[] meshRenderers;

    static List<Color> colors = new List<Color>();

    public void ApplyMaterial(int teamIndex)
    {
        if (colors.Count <= teamIndex)
        {
            colors.Add(Random.ColorHSV(0, 1, 1, 1));
        }

        foreach (var meshRenderer in meshRenderers)
        {
            var mat = new Material(robotMaterial);
            mat.SetColor("_Color", colors[teamIndex]);
            meshRenderer.material = mat;
        }
    }
}
