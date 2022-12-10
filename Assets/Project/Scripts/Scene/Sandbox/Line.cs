using System;
using AloneSpace;
using UnityEngine;

namespace SandBox
{
    public class Line : MonoBehaviour, IPosition
    {
        [SerializeField] Transform arrow;
        [SerializeField] Vector3 direction;
    
        CollisionShapeLine line;

        public CollisionShape LineShape => line;
        public int AreaId { get; } = 0;
        public Vector3 Position => transform.position;

        void Awake()
        {
            line = new CollisionShapeLine(this, Vector3.up, 1);
        }

        public void Update()
        {
            line.Directon = direction;
            
            arrow.LookAt(transform.position + direction);
        }
    }
}
