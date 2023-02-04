using TMPro;
using UnityEngine;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSourse { Gold, Life, Kills }
        public UpdateSourse sourse;
        private TextMeshProUGUI m_Text;

        private void Start()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            switch (sourse)
            {
                case UpdateSourse.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSourse.Life:
                    TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                    break;
                case UpdateSourse.Kills:
                    TDPlayer.Instance.KillUpdateSubscribe(UpdateText);
                    break;

            }
        }

        private void UpdateText(int value)
        {
            m_Text.text = value.ToString();
        }
    }
}