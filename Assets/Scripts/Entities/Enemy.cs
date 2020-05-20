using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace HackedDesign.Entities
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PolyNav.PolyNavAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("Runtime Game Objects")]
        private Animator animator = null; //The parent animator.
        private PolyNav.PolyNavAgent polyNavAgent = null;

        [Header("Referenced Game Objects")]
        [SerializeField] private Transform detection = null;

        [Header("Settings")]
        [SerializeField] public Story.Enemy enemy = null;
        [SerializeField] private float patrolSpeed = 0.5f;
        [SerializeField] private LayerMask playerLayerMask = 0;
        [SerializeField] private LayerMask poiLayerMask = 0;
        [SerializeField] private float poiDistance = 5f;
        [SerializeField] private float visibilityDistance = 3.2f;
        [SerializeField] private bool randomStartingDirection = true;
        [SerializeField] private Vector2 direction = Vector2.down;
        [SerializeField] private bool stationary = true;
        [SerializeField] private float patrolTime = 10.0f;
        [SerializeField] private float rotateSpeed = 45.0f;


        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        public EnemyState State { get; private set; } = EnemyState.PASSIVE;

        private float patrolTimer = 0;

        private readonly List<GameObject> colliders = new List<GameObject>();
        private Transform player;
        private bool playerSeen;

        private int directionXAnimId;
        private int directionYAnimId;
        private int isMovingAnimId;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();

            directionXAnimId = Animator.StringToHash("directionX");
            directionYAnimId = Animator.StringToHash("directionY");
            isMovingAnimId = Animator.StringToHash("isMoving");
            if (stationary)
            {
                polyNavAgent.enabled = false;
            }

        }

        protected void Start()
        {

            if (animator == null)
            {
                Logger.LogError(name, "Enemy without animator set");
            }
            if (polyNavAgent == null)
            {
                Logger.LogError(name, "Enemy without polyNavAgent set");
            }


        }

        public void Initialize(Transform player, PolyNav.PolyNav2D polyNav2D)
        {
            this.player = player;
            if (polyNavAgent != null && polyNavAgent.isActiveAndEnabled)
            {
                polyNavAgent.map = polyNav2D;
            }

            direction = Vector2.down;

            if (randomStartingDirection)
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, Mathf.PI)) * Vector2.up;
            }

            polyNavAgent.maxSpeed = patrolSpeed;
        }

        public void UpdateBehaviour()
        {

            switch (State)
            {
                case EnemyState.PASSIVE:
                    UpdatePassive();
                    break;
                case EnemyState.ALERTED:
                    UpdateAlerted();
                    break;
                default:
                    break;
            }

            UpdateDetection();
            Animate();
        }

        private void UpdatePassive()
        {
            // The player triggered our detection, but there anything in the way?
            if (colliders.Contains(GameManager.Instance.GetPlayer()))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                    State = EnemyState.ALERTED;
                    return;
                }
            }

            if (stationary)
            {
                var arc = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
                direction = arc * direction;
            }

            if (!stationary && (Time.time - patrolTimer) >= patrolTime)
            {
                if (polyNavAgent != null && polyNavAgent.hasPath)
                {
                    direction = polyNavAgent.movingDirection.normalized;
                }

                patrolTimer = Time.time;

                var pointsOfInterest = GetPointsOfInterestNearby();
                Logger.Log(name, "Patroling");

                if (pointsOfInterest.Length > 0)
                {
                    var point = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];

                    polyNavAgent.SetDestination(point.transform.position);
                }
                else
                {
                    Logger.Log(name, "No points of interest nearby");
                }
            }
        }

        private void UpdateAlerted()
        {
            if (colliders.Contains(GameManager.Instance.GetPlayer()))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                }
            }

            if (playerSeen && !colliders.Contains(GameManager.Instance.GetPlayer()))
            {
                State = EnemyState.PASSIVE;
                playerSeen = false;
                playerLeaveEvent.Invoke();
            }

            if (playerSeen)
            {
                if (stationary)
                {
                    //direction = player.transform.position - transform.position;
                }
                else
                {
                    //polyNavAgent.SetDestination(player.transform.position);
                }
            }
        }


        public void SetAlert()
        {

        }

        public void UpdateDetection()
        {
            if (detection == null)
                return;

            if (direction != Vector2.zero)
            {
                detection.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
            }
        }

        public RaycastHit2D CanSeePlayer()
        {
            return Physics2D.Raycast(transform.position, (player.position - transform.position), visibilityDistance, playerLayerMask);
        }


        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER) && !colliders.Contains(other.gameObject))
            {
                colliders.Add(other.gameObject);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER) && colliders.Contains(other.gameObject))
            {
                colliders.Remove(other.gameObject);
            }
        }

        private Collider2D[] GetPointsOfInterestNearby()
        {
            return Physics2D.OverlapCircleAll(transform.position, poiDistance, poiLayerMask);
        }


        private void Animate()
        {
            if (animator == null)
            {
                return;
            }

            if (polyNavAgent != null && polyNavAgent.currentSpeed > Vector2.kEpsilon)
            {
                //anim.SetFloat ("moveX", movementVector.x);
                //anim.SetFloat ("moveY", movementVector.y);
                animator.SetFloat(directionXAnimId, direction.x);
                animator.SetFloat(directionYAnimId, direction.y);
                animator.SetBool(isMovingAnimId, true);
            }
            else
            {
                //direction = Vector2.zero;

                animator.SetBool(isMovingAnimId, false);

            }
        }



        public enum EnemyState
        {
            PASSIVE,
            ALERTED,
            SEEKING,
            RESPONDING,
            HUNTING,
            FIGHTING,
            STUNNED
        }
    }
}
