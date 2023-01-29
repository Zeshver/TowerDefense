using UnityEngine;
using System;
using SpaceShooter;

namespace TowerDefense
{
    public class Upgrades : MonoSingleton<Upgrades>
    {
        public const string filename = "upgrades.dat";

        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }

        [SerializeField] private UpgradeAsset m_HealthUpgrade;
        public int Health { get { return GetUpgradeLevel(m_HealthUpgrade); } }

        [SerializeField] private UpgradeAsset m_RadiusUpgrade;
        public float Radius { get { return GetUpgradeLevel(m_RadiusUpgrade); } }

        [SerializeField] private UpgradeAsset m_AttakSpeedUpgrade;
        public float AttakSpeed { get { return GetUpgradeLevel(m_AttakSpeedUpgrade); } }

        [SerializeField] private UpgradeSave[] m_Save;

        private new void Awake()
        {
            base.Awake();
            Saver<UpgradeSave[]>.TryLoad(filename, ref m_Save);
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Save)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level += 1;
                    Saver<UpgradeSave[]>.Save(filename, Instance.m_Save);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach (var upgrade in Instance.m_Save)
            {
                for (int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costByLevel[i];
                }
            }
            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Save)
            {
                if (upgrade.asset == asset)
                {
                    return upgrade.level;
                }
            }

            return 0;
        }
    }
}