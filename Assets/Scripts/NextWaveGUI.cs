using UnityEngine;
using SpaceShooter;
using TMPro;

namespace TowerDefense
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_BonusAmount;
        private EnemyWavesManager m_Manager;
        private float m_TimeToNextWave;

        private void Start()
        {
            m_Manager = FindObjectOfType<EnemyWavesManager>();
            EnemyWaves.OnWavePrepare += (float time) =>
            {
                m_TimeToNextWave = time;
            };
        }

        private void Update()
        {
            var bonus = (int)m_TimeToNextWave;
            if (bonus < 0) bonus = 0;
            m_BonusAmount.text = bonus.ToString();
            m_TimeToNextWave -= Time.deltaTime;
        }

        public void CallWave()
        {
            m_Manager.ForceNextWave();
        }
    }
}