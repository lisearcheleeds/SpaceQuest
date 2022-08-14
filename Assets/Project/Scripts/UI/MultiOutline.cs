using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MultiOutline : BaseMeshEffect
{
    [SerializeField, Range(0, 100)] int amount;
    [SerializeField] Color color;
    [SerializeField] float offset;
 
    List<UIVertex> outlineVertexList = new List<UIVertex>();
    List<UIVertex> vertexList = new List<UIVertex>();
 
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
 
        vertexList.Clear();
        outlineVertexList.Clear();
        vh.GetUIVertexStream(vertexList);
 
        var splitAngle = 360f / amount;

 
        var count = vertexList.Count;
        for (var i = 0; i < amount; i++)
        {
            var angle = splitAngle * i;
            for (var j = 0; j < count; j++)
            {
                var v = vertexList[j];
                var pos = v.position;
                pos.x += Mathf.Cos(angle * Mathf.Deg2Rad) * offset;
                pos.y += Mathf.Sin(angle * Mathf.Deg2Rad) * offset;
                v.position = pos;
                v.color = color;
                outlineVertexList.Add(v);
            }
        }
 
        outlineVertexList.AddRange(vertexList);
 
        vh.Clear();
        vh.AddUIVertexTriangleStream(outlineVertexList);
    }
}