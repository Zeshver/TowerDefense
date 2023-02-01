using UnityEngine;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine.UI;

namespace TowerDefense
{
    public class Abilities : MonoSingleton<Abilities>
    {
        public interface Usable { void Use(); }
        [Serializable]
        public class FireAbility: Usable
        {
            [SerializeField] private int m_Cost = 5;
            [SerializeField] private int m_Damage = 2;
            public void Use() { }
        }
        [Serializable]
        public class TimeAbility: Usable
        {
            [SerializeField] private int m_Cost = 10;
            [SerializeField] private float m_Cooldown = 15f;
            [SerializeField] private float m_Duration = 5f;
            public void Use() 
            {
                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }                
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                {
                    ship.HalfMaxLinearVelocity();
                }
                EnemyWavesManager.OnEnemySpawn += Slow;

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                    {
                        ship.RestorMaxLinearVelocity();
                    }
                    EnemyWavesManager.OnEnemySpawn -= Slow;
                }
                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    Instance.m_TimeButton.interactable = false;
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_TimeButton.interactable = true;
                }
                Instance.StartCoroutine(TimeAbilityButton());
            }
        }
        [SerializeField] private Button m_TimeButton;

        [SerializeField] private FireAbility m_FireAbility;
        public void UseFireAbility() => m_FireAbility.Use();

        [SerializeField] private TimeAbility m_TimeAbility;
        public void UseTimeAbility() => m_TimeAbility.Use();
    }
}