using System;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class AreaCellTest : MonoBehaviour
    {
	    [SerializeField] LineRenderer lineRenderer;
	    
	    Vector3[] target = {};

	    void Awake()
	    {
		    lineRenderer.startWidth = 0.01f;
		    lineRenderer.endWidth = 0.01f;
	    }

	    void Update()
        {
	        if (target.Length == 0)
	        {
		        return;
	        }

	        // 頂点の数
	        lineRenderer.positionCount = target.Length;
	        for (var i = 0; i < target.Length; i++)
	        {
		        lineRenderer.SetPosition(i, target[i]);
	        }
        }
        
        [ContextMenu("OnClickTop")]
        void OnClickTop()
        {
	        target = AreaCellVertex.Top.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottom")]
        void OnClickBottom()
        {
	        target = AreaCellVertex.Bottom.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickFront")]
        void OnClickFront()
        {
	        target = AreaCellVertex.Front.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBack")]
        void OnClickBack()
        {
	        target = AreaCellVertex.Back.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickRight")]
        void OnClickRight()
        {
	        target = AreaCellVertex.Right.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickLeft")]
        void OnClickLeft()
        {
	        target = AreaCellVertex.Left.Select(x => x + transform.position).ToArray();
        }
        
        [ContextMenu("OnClickTopFrontLeft")]
        void OnClickTopFrontLeft()
        {
	        target = AreaCellVertex.TopFrontLeft.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopFrontRight")]
        void OnClickTopFrontRight()
        {
	        target = AreaCellVertex.TopFrontRight.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopBackLeft")]
        void OnClickTopBackLeft()
        {
	        target = AreaCellVertex.TopBackLeft.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopBackRight")]
        void OnClickTopBackRight()
        {
	        target = AreaCellVertex.TopBackRight.Select(x => x + transform.position).ToArray();
        }
        
        [ContextMenu("OnClickBottomFrontLeft")]
        void OnClickBottomFrontLeft()
        {
	        target = AreaCellVertex.BottomFrontLeft.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomFrontRight")]
        void OnClickBottomFrontRight()
        {
	        target = AreaCellVertex.BottomFrontRight.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomBackLeft")]
        void OnClickBottomBackLeft()
        {
	        target = AreaCellVertex.BottomBackLeft.Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomBackRight")]
        void OnClickBottomBackRight()
        {
	        target = AreaCellVertex.BottomBackRight.Select(x => x + transform.position).ToArray();
        }
    }
}