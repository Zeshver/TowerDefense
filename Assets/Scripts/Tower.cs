using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius = 2f;
        private float m_Lead = 0.2f;
        private Turret[] m_Turrets;
        private Rigidbody2D m_Target = null;

        private void Start()
        {
            m_Turrets = GetComponentsInChildren<Turret>();

            var radius = Upgrades.Instance.Radius;
            SetRadius(radius / 1.5f);
        }

        public void Use(TowerAsset asset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = asset.sprite;
            m_Turrets = GetComponentsInChildren<Turret>();

            foreach (var turret in m_Turrets)
            {
                turret.AssignLoadout(asset.turretProperties);
            }

            var radius = Upgrades.Instance.Radius;
            SetRadius(radius / 1.5f);

            GetComponentInChildren<BuildSite>().SetBuildableTowers(asset.m_UpgradesTo);
        }

        private void Update()
        {
            if (m_Target)
            {
                if (Vector3.Distance(m_Target.transform.position, transform.position) <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = m_Target.transform.position - turret.transform.position + (Vector3)m_Target.velocity * m_Lead;
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
                    m_Target = enter.transform.root.GetComponent<Rigidbody2D>();
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
