using AloneSpace.InSide;
using UnityEngine;

namespace SandBox
{
    public class Line : MonoBehaviour
    {
        [SerializeField] Transform arrow;
        [SerializeField] Vector3 direction;
    
        CollisionShapeLine line = new CollisionShapeLine(Vector3.zero, Vector3.up, 1);

        public CollisionShape LineShape => line;
    
        public void Update()
        {
            line.Position = transform.position;
            line.Directon = direction;
            
            arrow.LookAt(transform.position + direction);
        }
    }
}
