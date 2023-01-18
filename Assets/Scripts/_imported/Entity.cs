using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private string m_Nickname;
        public string Nickname { get => m_Nickname; set => m_Nickname = value; }
    }
}