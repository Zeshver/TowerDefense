using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelResultController : MonoSingleton<LevelResultController>
    { 

        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;

        [SerializeField] private Text m_LevelTime;
        [SerializeField] private Text m_TotalPlayTime;
        [SerializeField] private Text m_TotalScore;
        [SerializeField] private Text m_TotalKills;

        public void Show(bool result)
        {
            m_PanelSuccess?.gameObject.SetActive(result);
            m_PanelFailure?.gameObject.SetActive(!result);
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

        public void OnRestartLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
        }


        public class Stats
        {
            public int numKills;
            public float time;
            public int score;
        }

        public static Stats TotalStats { get; private set; }

        public static void ResetPlayerStats()
        {
            TotalStats = new Stats();
        }

        private void UpdateCurrentLevelStats()
        {
            TotalStats.numKills += Player.Instance.NumKills;
            TotalStats.time += LevelController.Instance.LevelTime;
            TotalStats.score += Player.Instance.Score;

            int timeBonus = (int) (LevelController.Instance.ReferenceTime - LevelController.Instance.LevelTime);

            if(timeBonus > 0)
                TotalStats.score += timeBonus;
        }

        private void UpdateVisualStats()
        {
            m_LevelTime.text = System.Convert.ToInt32(LevelController.Instance.LevelTime).ToString();
            m_TotalScore.text = TotalStats.score.ToString();
            m_TotalPlayTime.text = System.Convert.ToInt32(TotalStats.time).ToString();
            m_TotalKills.text = TotalStats.numKills.ToString();
        }
    }
}