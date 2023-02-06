using UnityEngine;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TowerDefense
{
    public class Abilities : MonoSingleton<Abilities>
    {
        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost = 5;
            public int Cost => m_Cost;
            [SerializeField] private int m_Damage = 2;
            [SerializeField] private Color m_TargetingColor;
            [SerializeField] private TextMeshProUGUI m_CostText;
            public void Use()
            {
                Player.Instance.RemoveKill(m_Cost);
                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);
                    foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
                    {
                        if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            if (Upgrades.Instance.Fire == 2)
                            {
                                m_Damage = 5;
                            }
                            enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
                        }
                    }
                });
            }

            public void SetText()
            {
                m_CostText.text = m_Cost.ToString();
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Cost = 10;
            public int Cost => m_Cost;
            [SerializeField] private float m_Cooldown = 15f;
            [SerializeField] private float m_Duration = 5f;
            [SerializeField] private TextMeshProUGUI m_CostText;
            public void Use()
            {
                Player.Instance.RemoveKill(m_Cost);
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
                    if (Upgrades.Instance.Ice == 2)
                    {
                        m_Duration = 10f;
                    }
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

            public void SetText()
            {
                m_CostText.text = m_Cost.ToString();
            }
        }
        [SerializeField] private Button m_TimeButton;
        [SerializeField] private Button m_FireButton;

        [SerializeField] private Image m_TargetingCircle;

        [SerializeField] private FireAbility m_FireAbility;
        public void UseFireAbility() => m_FireAbility.Use();

        [SerializeField] private TimeAbility m_TimeAbility;
        public void UseTimeAbility() => m_TimeAbility.Use();

        private List<Abilities> m_Abilities = new List<Abilities>() { };

        private void Start()
        {
            Instance.m_FireButton.interactable = false;
            m_FireAbility.SetText();

            Instance.m_TimeButton.interactable = false;
            m_TimeAbility.SetText();
        }

        private void Update()
        {
            if (Upgrades.Instance.Fire > 0 && Player.Instance.NumKills >= Instance.m_FireAbility.Cost)
            {
                Instance.m_FireButton.interactable = true;
            }
            else
            {
                Instance.m_FireButton.interactable = false;
            }
            if (Upgrades.Instance.Ice > 0 && Player.Instance.NumKills >= Instance.m_TimeAbility.Cost)
            {
                Instance.m_TimeButton.interactable = true;
            }
            else
            {
                Instance.m_TimeButton.interactable = false;
            }
        }
    }
}