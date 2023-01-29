using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TowerDefense
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset m_Asset;
        [SerializeField] private Image m_UpgradeIcon;
        private int costNumber = 0;
        [SerializeField] private TextMeshProUGUI m_Level, m_CostText;
        [SerializeField] private Button m_BuyButton;

        public void Initialize()
        {
            m_UpgradeIcon.sprite = m_Asset.sprite;
            var savedLevel = Upgrades.GetUpgradeLevel(m_Asset);
            if (savedLevel >= m_Asset.costByLevel.Length)
            {
                m_Level.text += $"Lvl : {savedLevel} (Max)";
                m_BuyButton.interactable = false;
                m_BuyButton.transform.Find("Image").gameObject.SetActive(false);
                m_BuyButton.transform.Find("Text (TMP)").gameObject.SetActive(false);
                m_CostText.text = "";
                costNumber = int.MaxValue;
            }
            else
            {
                m_Level.text = $"Lvl : {savedLevel + 1}";
                costNumber = m_Asset.costByLevel[savedLevel];
                m_CostText.text = costNumber.ToString();
            }
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(m_Asset);
            Initialize();
        }

        public void CheckCost(int m_Money)
        {
            m_BuyButton.interactable = m_Money >= costNumber;
        }
    }
}