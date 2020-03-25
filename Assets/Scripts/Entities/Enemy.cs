using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace HackedDesign.Entities
{
    [RequireComponent(typeof(PolyNav.PolyNavAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField]
        private Animator animator = null; //The parent animator.
        [SerializeField]
        private PolyNav.PolyNavAgent polyNavAgent = null;
        [SerializeField]
        private Transform detection = null;

        [Header("Settings")]
        [SerializeField]
        public Story.Enemy enemy = null;
        [SerializeField]
        private LayerMask playerLayerMask = 0;
        [SerializeField]
        private float visibilityDistance = 3.2f;
        [SerializeField]
        private bool randomStartingDirection = true;
        [SerializeField]
        private Vector2 direction = Vector2.down;
        [SerializeField]
        private bool stationary = true;

        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        [Header("State")]
        public EnemyState state = EnemyState.PASSIVE;

        private readonly List<GameObject> colliders = new List<GameObject>();
        private Transform player;
        private bool playerSeen;

        protected void Start()
        {
            if (animator == null)
            {
                Logger.LogError(this.name, "Enemy without animator set");
            }
            if (polyNavAgent == null)
            {
                Logger.LogError(this.name, "Enemy without polyNavAgent set");
            }

            if (detection == null)
            {
                Logger.LogError(this.name, "Enemy without Alert set");
            }
        }

        public void Initialize(Transform player, PolyNav.PolyNav2D polyNav2D)
        {
            this.player = player;
            if (this.polyNavAgent != null && this.polyNavAgent.isActiveAndEnabled)
            {
                this.polyNavAgent.map = polyNav2D;
            }

            if(randomStartingDirection)
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up;
            }

            if(direction == Vector2.zero)
            {
                direction = Vector2.down;
            }
        }

        public void UpdateBehaviour()
        {
            if(polyNavAgent.currentSpeed > 0.01f)
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
            if (colliders.Contains(CoreGame.Instance.GetPlayer()))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                    state = EnemyState.ALERTED;
                }
            }

            if (playerSeen && !colliders.Contains(CoreGame.Instance.GetPlayer()))
            {
                playerSeen = false;
                playerLeaveEvent.Invoke();
            }

            if (playerSeen)
            {
                if (stationary)
                {
                    direction = player.transform.position - this.transform.position;
                }
                else
                {
                    polyNavAgent.SetDestination(player.transform.position);
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
                    direction = player.transform.position - this.transform.position;
                }
                else
                {
                    polyNavAgent.SetDestination(player.transform.position);
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

            detection.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        }

        private void Animate()
        {
            if (animator == null)
            {
                return;
            }

            if (this.polyNavAgent != null && this.polyNavAgent.currentSpeed > Vector2.kEpsilon)
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

        public RaycastHit2D CanSeePlayer()
        {
            return Physics2D.Raycast(transform.position, (player.position - transform.position), visibilityDistance, playerLayerMask);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if ((other.CompareTag(TagManager.PLAYER)) && !colliders.Contains(other.gameObject))
            {
                colliders.Add(other.gameObject);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if ((other.CompareTag(TagManager.PLAYER)) && colliders.Contains(other.gameObject))
            {
                colliders.Remove(other.gameObject);
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
