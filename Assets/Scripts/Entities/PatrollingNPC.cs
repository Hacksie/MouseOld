using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HackedDesign
{
    [RequireComponent(typeof(PolyNav.PolyNavAgent))]
    public class PatrollingNPC : Entity
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Transform detection = null;

        [Header("Settings")]
        [SerializeField] private float patrolSpeed = 0.5f;
        [SerializeField] private float patrolTime = 10.0f;
        [SerializeField] private LayerMask poiLayerMask = 0;
        [SerializeField] private float poiDistance = 5f;

        private float patrolTimer = 0;
        private PolyNav.PolyNavAgent polyNavAgent = null;
        private bool tripped = false;

        protected override void Awake()
        {
            base.Awake();
            polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();
        }

        public override void Initialize(bool pooled, Transform playerTransform)
        {
            Logger.Log(this, "Initializing Patrolling Enemy");

            base.Initialize(pooled, playerTransform);
            isMoving = false;

            if (polyNavAgent != null && polyNavAgent.isActiveAndEnabled)
            {
                polyNavAgent.map = GameManager.Instance.PolyNav;
            }
            polyNavAgent.maxSpeed = patrolSpeed;
        }

        public override void UpdateBehaviour()
        {
            switch (State)
            {
                case EntityState.Patrol:
                    UpdatePassive();
                    break;
                case EntityState.Alert:
                    UpdateAlerted();
                    break;
                default:
                    break;
            }

            UpdateDetection();
        }

        protected override void UpdatePassive()
        {
            if (polyNavAgent != null && polyNavAgent.hasPath)
            {
                direction = polyNavAgent.movingDirection.normalized;
                isMoving = (polyNavAgent.movingDirection.sqrMagnitude > Vector2.kEpsilon);
            }
            else
            {
                isMoving = false;
            }

            if ((Time.time - patrolTimer) >= patrolTime)
            {
                patrolTimer = Time.time;

                var pointsOfInterest = GetPointsOfInterestNearby();
                Logger.Log(this, "Patrolling");

                if (pointsOfInterest.Length > 0)
                {
                    var point = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];

                    polyNavAgent.SetDestination(point.transform.position);
                }
                else
                {
                    Logger.Log(this, "No points of interest nearby");
                }
            }

            if (colliders.Contains(playerTransform.gameObject) && !tripped)
            {
                tripped = true;
                //resetTimer = Time.time;
                GameManager.Instance.IncreaseAlert();
            }

            //base.UpdatePassive();
        }

        private void UpdateDetection()
        {
            if (detection == null)
                return;

            if (direction != Vector2.zero)
            {
                detection.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
            }
        }


        protected Collider2D[] GetPointsOfInterestNearby()
        {
            return Physics2D.OverlapCircleAll(transform.position, poiDistance, poiLayerMask);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (polyNavAgent != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, polyNavAgent.primeGoal);
            }
        }

    }
}