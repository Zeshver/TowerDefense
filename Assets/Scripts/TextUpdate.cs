using TMPro;
using UnityEngine;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSourse { Gold, Life }
        public UpdateSourse sourse;
        private TextMeshProUGUI m_Text;

        private void Start()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            switch (sourse)
            {
                case UpdateSourse.Gold:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSourse.Life:
                    TDPlayer.LifeUpdateSubscribe(UpdateText);
                    break;

            }
        }

        private void UpdateText(int value)
        {
            m_Text.text = value.ToString();
        }
    }
}