using System;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class AreaCellTest : MonoBehaviour
    {
	    [SerializeField] LineRenderer lineRenderer;
	    
	    Vector3[] target = {};
	    bool showDirection;

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

	    void OnDrawGizmos()
	    {
		    if (showDirection)
		    {
			    foreach (AreaDirection areaDirection in Enum.GetValues(typeof(AreaDirection)))
			    {
				    Gizmos.DrawLine(transform.position, transform.position + AreaCellVertex.GetDirection(areaDirection) * 10);
			    }
		    }
	    }

	    [ContextMenu("OnClickDirection")]
	    void OnClickDirection()
	    {
		    showDirection = !showDirection;
	    }
        
        [ContextMenu("OnClickTop")]
        void OnClickTop()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Top).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottom")]
        void OnClickBottom()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Bottom).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickFront")]
        void OnClickFront()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Front).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBack")]
        void OnClickBack()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Back).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickRight")]
        void OnClickRight()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Right).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickLeft")]
        void OnClickLeft()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.Left).Select(x => x + transform.position).ToArray();
        }
        
        [ContextMenu("OnClickTopFrontLeft")]
        void OnClickTopFrontLeft()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.TopFrontLeft).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopFrontRight")]
        void OnClickTopFrontRight()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.TopFrontRight).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopBackLeft")]
        void OnClickTopBackLeft()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.TopBackLeft).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickTopBackRight")]
        void OnClickTopBackRight()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.TopBackRight).Select(x => x + transform.position).ToArray();
        }
        
        [ContextMenu("OnClickBottomFrontLeft")]
        void OnClickBottomFrontLeft()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.BottomFrontLeft).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomFrontRight")]
        void OnClickBottomFrontRight()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.BottomFrontRight).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomBackLeft")]
        void OnClickBottomBackLeft()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.BottomBackLeft).Select(x => x + transform.position).ToArray();
        }

        [ContextMenu("OnClickBottomBackRight")]
        void OnClickBottomBackRight()
        {
	        target = AreaCellVertex.GetPrimitives(AreaDirection.BottomBackRight).Select(x => x + transform.position).ToArray();
        }
    }
}