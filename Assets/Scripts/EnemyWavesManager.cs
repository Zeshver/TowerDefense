using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class EnemyWavesManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;
        [SerializeField] private Enemy m_EnemyPrefabs;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWaves m_CurrentWave;
        public event Action OnAllWavesDead;

        private int m_ActiveEnemyCount = 0;
        private void RecordEnemyDead() 
        {
            if (--m_ActiveEnemyCount == 0)
            {
                ForceNextWave();
            }
        }

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {            
            foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefabs, m_Paths[pathIndex].StartArea.RandomInsideZone, Quaternion.identity);
                        e.OnEnd += RecordEnemyDead;
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(m_Paths[pathIndex]);
                        m_ActiveEnemyCount += 1;
                        OnEnemySpawn?.Invoke(e);
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (m_CurrentWave)
            {
                TDPlayer.Instance.ChangeGold((int)m_CurrentWave.GetRemainingTime());
                SpawnEnemies();
            }
            else
            {
                if (m_ActiveEnemyCount == 0)
                {
                    OnAllWavesDead?.Invoke();
                }
            }
        }
    }
}