using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        public Sprite sprite;
        public int[] costByLevel = { 3 };
    }
}