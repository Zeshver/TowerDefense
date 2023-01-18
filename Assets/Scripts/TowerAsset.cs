using SpaceShooter;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public int goldCost = 15;
        public Sprite towerGUI;
        public Sprite sprite;
        public TurretProperties turretProperties;
    }    
}