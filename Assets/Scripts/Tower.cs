using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius = 2f;
        private Turret[] m_Turrets;
        private Destructible m_Target = null;

        private void Start()
        {
            m_Turrets = GetComponentsInChildren<Turret>();

            var radius = Upgrades.Instance.Radius;
            SetRadius(radius / 1.5f);
        }

        private void Update()
        {
            if (m_Target)
            {
                Vector2 targetVector = m_Target.transform.position - transform.position;

                if (targetVector.magnitude <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = targetVector;
                        turret.Fire();
                    }
                }
                else
                {
                    m_Target = null;
                }
            }            
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enter)
                {
                    m_Target = enter.transform.root.GetComponent<Destructible>();
                }
            }
        }

        private void SetRadius(float radius)
        {
            m_Radius += radius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}
