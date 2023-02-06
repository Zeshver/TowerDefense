using UnityEngine;
using System;

namespace SpaceShooter
{
    public class Player : MonoSingleton<Player>
    {

        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;
        public event Action OnPlayerDead;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        private void Start()
        {
            if (m_Ship)
            {
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
            else
                LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }


        #region Score (current level only)

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void RemoveKill(int killScore)
        {
            NumKills -= killScore;
        }

        public void AddScore(int num)
        {
            Score += num;
        }

        protected void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
            }
        }

        #endregion
    }
}