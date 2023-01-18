using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace SpaceShooter
{    public class CircleArea : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;
        
        public Vector2 RandomInsideZone
        {
            get
            {
                return (Vector2)transform.position + (Vector2)UnityEngine.Random.insideUnitSphere * m_Radius;
            }
        }

        public bool IsInside(Vector2 p)
        {
            return ((Vector2)transform.position - p).sqrMagnitude < m_Radius * m_Radius;
        }

#if UNITY_EDITOR
        private static readonly Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}