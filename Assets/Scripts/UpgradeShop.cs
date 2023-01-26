using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int m_Money;
        [SerializeField] private TextMeshProUGUI m_MoneyText;
        [SerializeField] private BuyUpgrade[] m_Sales;

        private void Start()
        {
            foreach (var slot in m_Sales)
            {
                slot.Initialize();
                slot.transform.Find("Button").GetComponent<Button>().onClick.AddListener(UpdateMoney);
            }

            UpdateMoney();
        }

        public void UpdateMoney()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_Money -= Upgrades.GetTotalCost();
            m_MoneyText.text = m_Money.ToString();
            foreach (var slot in m_Sales)
            {
                slot.CheckCost(m_Money);
            }
        }
    }
}