using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        private bool isCompleted;

        private void Start()
        {
            FindObjectOfType<EnemyWavesManager>().OnAllWavesDead += () =>
            {
                isCompleted = true;
            };
        }

        public bool IsCompleted { get { return isCompleted; } }
    }
}