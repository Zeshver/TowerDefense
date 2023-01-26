using SpaceShooter;
using UnityEngine;
using System;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance
        {
            get { return Player.Instance as TDPlayer; }
        }

        private static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }

        public static event Action<int> OnLifeUpdate;
        public static void LifeUpdateSubscribe(Action<int> act)
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
            var tower = Instantiate(m_TowerPrefab, buildSite.position, Quaternion.identity); ;
            tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.sprite;
            tower.GetComponentInChildren<Turret>().m_TurretProperties = towerAsset.turretProperties;
            Destroy(buildSite.gameObject);
        }

        [SerializeField] private UpgradeAsset m_HealthUpgrade;

        private new void Awake()
        {
            base.Awake();
            var level = Upgrades.GetUpgradeLevel(m_HealthUpgrade);
            TakeDamage(-level * 5);
        }
    }
}