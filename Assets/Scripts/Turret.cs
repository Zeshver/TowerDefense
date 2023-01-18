using UnityEngine;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        public TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;
                
        private SpaceShip m_Ship;

        #region Unity events

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
            //else if (Mode == TurretMode.Auto)
            //{
            //    Fire();
            //}
        }

        #endregion

        #region Public API

        public void Fire()
        {
            if (m_RefireTimer > 0)
                return;

            if (m_TurretProperties == null)
                return;

            if (m_Ship)
            {
                // кушаем энергию
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                    return;

                // кушаем патроны
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }            
            
            // инстанцируем прожектайл который уже сам полетит.
            var projectile = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            // метод выставления данных прожектайлу о том кто стрелял для избавления от попаданий в самого себя
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX на домашку
            }
        }

        /// <summary>
        /// Установка свойств турели. Будет использовано в дальнейшем для паверапки.
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode)
                return;

            m_TurretProperties = props;
            m_RefireTimer = 0;
        }


        #endregion
    }
}