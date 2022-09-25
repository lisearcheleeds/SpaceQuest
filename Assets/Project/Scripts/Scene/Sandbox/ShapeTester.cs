using AloneSpace;
using UnityEngine;

namespace SandBox
{
    public class ShapeTester : MonoBehaviour
    {
        [SerializeField] Transform arrow;
        [SerializeField] Line line;

        public void Update()
        {
            /*
            var outwardVector = line.LineShape.GetOutwardVector(transform.position);
            arrow.LookAt(transform.position + outwardVector);
            */
        }
    }
}
