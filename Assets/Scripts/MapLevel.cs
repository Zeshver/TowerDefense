using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_Episode;
        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultImage;

        public bool IsComplete { get { return gameObject.activeSelf && m_ResultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }

        public void SetLevelData(Episode episode, int score)
        {
            m_Episode = episode;
            m_ResultPanel.gameObject.SetActive(score > 0);
            for (int i = 0; i < score; i++)
            {
                m_ResultImage[i].color = Color.white;
            }
        }
    }
}