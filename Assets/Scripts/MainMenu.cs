using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;

        private void Start()
        {
            m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
        }

        public void NewGame()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);
            SceneManager.LoadScene(1);
        }
        public void Continue()
        {
            SceneManager.LoadScene(1);
        }
        public void ReturnMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}