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
        [SerializeField] private LayerMask playerLayerMask = 0;
        [SerializeField] private LayerMask poiLayerMask = 0;
        [SerializeField] private float poiDistance = 5f;

        [SerializeField] private float visibilityDistance = 3.2f;
        [SerializeField] private bool randomStartingDirection = true;
        [SerializeField] private Vector2 direction = Vector2.down;
        [SerializeField] private bool stationary = true;
        [SerializeField] private float patrolSpeed = 10f; 


        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        [Header("State")]
        public EnemyState state = EnemyState.PASSIVE;

        private float patrolTimer = 0;

        private readonly List<GameObject> colliders = new List<GameObject>();
        private Transform player;
        private bool playerSeen;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();
            
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
                direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up;
            }
        }

        public void UpdateBehaviour()
        {
            if(polyNavAgent.hasPath)
            {
                direction = polyNavAgent.movingDirection.normalized;
            }

            switch(state)
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
            if (colliders.Contains(CoreGame.Instance.GetPlayer()))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                    state = EnemyState.ALERTED;
                    return;
                }
            }

            

            if (!stationary && (Time.time - patrolTimer) >= patrolSpeed)
            {
                
                patrolTimer = Time.time;

                var pointsOfInterest = GetPointsOfInterestNearby();
                Logger.Log(name, "Patrol ", pointsOfInterest.Length.ToString());

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
            if (colliders.Contains(CoreGame.Instance.GetPlayer()))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                }
            }

            if (playerSeen && !colliders.Contains(CoreGame.Instance.GetPlayer()))
            {
                state = EnemyState.PASSIVE;
                playerSeen = false;
                playerLeaveEvent.Invoke();
            }

            if (playerSeen)
            {
                if (stationary)
                {
                    direction = player.transform.position - transform.position;
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
                animator.SetFloat("directionX", direction.x);
                animator.SetFloat("directionY", direction.y);
                animator.SetBool("isMoving", true);
            }
            else
            {
                direction = Vector2.zero;
                if (animator != null)
                {
                    animator.SetBool("isMoving", false);
                }
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
