using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerDefense;

namespace SpaceShooter
{
    public class Destructible : Entity
    {
        #region Properties

        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        [SerializeField] private int m_HitPoints;
        
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        #endregion

        #region Unity events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }

        #region Безтеговая коллекция скриптов на сцене

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        #endregion 

        #endregion

        #region Public API

        public void ApplyDamage(int damage)
        {
            if (m_Indestructible)
                return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints < 0)
                OnDeath();
        }

        public void AddHitPoints(float hp)
        {
            m_CurrentHitPoints = (int)Mathf.Clamp(m_CurrentHitPoints + hp, 0, m_HitPoints);
        }

        #endregion

        protected virtual void OnDeath()
        {
            if(m_ExplosionPrefab != null)
            {
                var explosion = Instantiate(m_ExplosionPrefab.gameObject);
                explosion.transform.position = transform.position;
            }

            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        [SerializeField] private ImpactEffect m_ExplosionPrefab;

        #region Teams

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #endregion

        #region Score

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        #endregion

        protected void Use(EnemyAsset asset)
        {
            m_HitPoints = asset.hp;
            m_ScoreValue = asset.score;
        }
    }
}