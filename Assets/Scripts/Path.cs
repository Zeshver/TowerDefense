using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea m_StartArea;
        public CircleArea StartArea => m_StartArea;

        [SerializeField] private AIPointPatrol[] m_Points;
        public int Lenght { get => m_Points.Length; }
        public AIPointPatrol this[int i] { get => m_Points[i]; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            foreach (var point in m_Points)
                Gizmos.DrawSphere(point.transform.position, point.Radius);
        }
    }
}