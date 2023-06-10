using System;
using UnityEngine;

namespace AloneSpace
{
    public class PlanetCircle : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(Vector3.forward, 0.02f);
        }
    }
}