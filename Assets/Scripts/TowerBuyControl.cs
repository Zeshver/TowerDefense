using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TowerBuyControl : MonoBehaviour
    {

        [SerializeField] private TowerAsset m_TowerAsset;

        [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;

        [SerializeField] private Button m_Button;

        [SerializeField] private Transform m_BuildSite;
        public void SetBuildSite (Transform value)
        {
            m_BuildSite = value;
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
            m_TextMeshProUGUI.text = m_TowerAsset.goldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TowerAsset.towerGUI;
        }

        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TowerAsset.goldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_TextMeshProUGUI.color = m_Button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, m_BuildSite);
            BuildSite.HideControls();
        }
    }
}