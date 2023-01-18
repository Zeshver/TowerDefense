using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerShipSelectionController : MonoBehaviour
    {
        [SerializeField] private SpaceShip m_Prefab;

        public void OnShipSelected()
        {
            LevelSequenceController.PlayerShipPrefab = m_Prefab;
        }
    }
}