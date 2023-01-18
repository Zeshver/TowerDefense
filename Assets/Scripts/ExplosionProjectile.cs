using UnityEngine;
using SpaceShooter;
using UnityEditor;

namespace TowerDefense
{
    public class ExplosionProjectile : Projectile
    {
        [SerializeField] private ParticleSystem m_ParticleSystemExplosion;

        [SerializeField] private float m_Radius;
        [SerializeField] private int m_ExplosionDamage;
        [SerializeField] private int m_LifeTimeParticleExplosion;

        private void OnDestroy()
        {
            ParticleExplosion();

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, m_Radius);

            foreach (Collider2D target in cols)
            {
                Destructible dest = target.transform.root.GetComponent<Destructible>();

                if (dest != null)
                {
                    dest.ApplyDamage(m_ExplosionDamage);
                }
            }
        }

        private void ParticleExplosion()
        {
            m_ParticleSystemExplosion.transform.parent = null;
            m_ParticleSystemExplosion.Play();
            Destroy(m_ParticleSystemExplosion.gameObject, m_LifeTimeParticleExplosion);
        }

#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}