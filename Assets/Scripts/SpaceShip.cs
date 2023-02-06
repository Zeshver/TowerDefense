using TowerDefense;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        [SerializeField] private float m_Thrust;

        [SerializeField] private float m_Mobility;

        [SerializeField] private float m_MaxLinearVelocity;
        private float m_MaxVelocityBackup;
        public void HalfMaxLinearVelocity() 
        { 
            m_MaxVelocityBackup = m_MaxLinearVelocity;
            m_MaxLinearVelocity /= 2; 
        }
        public void RestorMaxLinearVelocity() { m_MaxLinearVelocity = m_MaxVelocityBackup; }

        [SerializeField] private float m_MaxAngularVelocity;

        private Rigidbody2D m_Rigid;
        public Rigidbody2D Rigid => m_Rigid;

        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity events

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            // единичная инерция для того чтобы упростить баланс кораблей.
            // либо неравномерные коллайдеры будут портить вращение
            // решается домножением торка на момент инерции
            m_Rigid.inertia = 1;

            // InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
            // UpdateEnergyRegen();
        }

        #endregion

        private void UpdateRigidbody()
        {
            // прибавляем толкающую силу
            m_Rigid.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            // линейное вязкое трение -V * C
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            // добавляем вращение
            m_Rigid.AddTorque(m_Mobility * TorqueControl * Time.fixedDeltaTime, ForceMode2D.Force);

            // вязкое вращательное трение
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        public bool DrawAmmo(int count)
        {
            return true;
        }

        public bool DrawEnergy(int count)
        {
            return true;
        }

        public void Fire(TurretMode mode)
        {
            return;
        }

        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }
    }
}