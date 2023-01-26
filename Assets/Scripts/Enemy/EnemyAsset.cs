using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("Visual")]
        public Color color = Color.white;
        public Vector2 spriteScale = new Vector2(3, 3);
        public RuntimeAnimatorController animation;

        [Header("Game parameters")]
        public float moveSpeed = 1;
        public int hp = 1;
        public int armor = 0;
        public int score = 1;
        public float radius = 0.2f;
        public int damage = 1;
        public int gold = 1;
    }
}