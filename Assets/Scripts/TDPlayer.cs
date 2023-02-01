using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance
        {
            get { return Player.Instance as TDPlayer; }
        }

        [SerializeField] private UpgradeAsset m_HealthUpgrade;

        private void Start()
        {
            var healthLevel = Upgrades.Instance.Health;
            TakeDamage(-healthLevel * 5);
        }

        private event Action<int> OnGoldUpdate;
        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }

        public event Action<int> OnLifeUpdate;
        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }

        [SerializeField] private int m_Gold = 0;

        public void ChangeGold(int change)
        {
            m_Gold += change;
            OnGoldUpdate(m_Gold);
        }

        public void ChangeLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }

        [SerializeField] private Tower m_TowerPrefab;

        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_TowerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildSite.gameObject);
        }        
    }
}