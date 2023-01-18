using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,

            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        #region Unity events

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitActionTimers();
        }

        private void Update()
        {
            UpdateActionTimers();
            UpdateAI();
        }

        #endregion


        private void UpdateAI()
        {
            switch (m_AIBehaviour)
            {
                case AIBehaviour.Null:
                    break;

                case AIBehaviour.Patrol:
                    UpdateBehaviourPatrol();
                    break;
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
        }

        private const float MaxAngle = 45.0f;

        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;
            
            return -angle;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_MovePosition, 1.0f);
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                if (m_PatrolPoint != null)
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                    if (isInsidePatrolZone)
                    {
                        GetNewPoint();
                    }
                    else
                    {
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }

            }

        }

        protected virtual void GetNewPoint()
        {
            if (IsActionTimerFinished(ActionTimerType.RandomizeDirection))
            {
                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                m_MovePosition = newPoint;


                SetActionTimer(ActionTimerType.RandomizeDirection, m_RandomSelectMovePointTime);
            }
        }

        #region Action timers

        private enum ActionTimerType
        {
            Null,

            RandomizeDirection,

            Fire,

            FindNewTarget,

            MaxValues
        }

        private float[] m_ActionTimers;

        private void InitActionTimers()
        {
            m_ActionTimers = new float[(int)ActionTimerType.MaxValues];
        }

        private void UpdateActionTimers()
        {
            for (int i = 0; i < m_ActionTimers.Length; i++)
            {
                if (m_ActionTimers[i] > 0)
                    m_ActionTimers[i] -= Time.deltaTime;
            }
        }

        private void SetActionTimer(ActionTimerType e, float time)
        {
            m_ActionTimers[(int)e] = time;
        }

        private bool IsActionTimerFinished(ActionTimerType e)
        {
            return m_ActionTimers[(int)e] <= 0;
        }

        #endregion

        
                
        private void ActionFindNewAttackTarget()
        {
            if (IsActionTimerFinished(ActionTimerType.FindNewTarget))
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                SetActionTimer(ActionTimerType.FindNewTarget, 1 + UnityEngine.Random.Range(0, m_FindNewTargetTime));
            }
        }

        private void ActionFire()
        {
            if(m_SelectedTarget != null)
            {
                if(IsActionTimerFinished(ActionTimerType.Fire))
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    SetActionTimer(ActionTimerType.Fire, UnityEngine.Random.Range(0, m_ShootDelay));
                }
            }
            
        }

        public static Vector3 MakeLead(
        Vector3 launchPoint,
        Vector3 launchVelocity,
        Vector3 targetPos,
        Vector3 targetVelocity)
        {
            Vector3 V = targetVelocity;
            Vector3 D = targetPos - launchPoint;
            float A = V.sqrMagnitude - launchVelocity.sqrMagnitude;
            float B = 2 * Vector3.Dot(D, V);
            float C = D.sqrMagnitude;

            if (A >= 0)
                return targetPos;

            float rt = Mathf.Sqrt(B * B - 4 * A * C);
            float dt1 = (-B + rt) / (2 * A);
            float dt2 = (-B - rt) / (2 * A);
            float dt = (dt1 < 0 ? dt2 : dt1);
            return targetPos + V * dt;
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float dist2 = -1;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip)
                    continue;

                if (Destructible.TeamIdNeutral == v.TeamId)
                    continue;

                if (m_SpaceShip.TeamId == v.TeamId)
                    continue;

                float d2 = (m_SpaceShip.transform.position - v.transform.position).sqrMagnitude;

                if (dist2 < 0 || d2 < dist2)
                {
                    potentialTarget = v;
                    dist2 = d2;
                }
            }

            return potentialTarget;
        }

        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }

        #region AI collision evade



        private void ActionEvadeCollision()
        {
            if(Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        #endregion
    }
}