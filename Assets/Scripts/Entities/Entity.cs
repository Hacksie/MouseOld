using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace HackedDesign
{
    [RequireComponent(typeof(Animator))]
    public class Entity : MonoBehaviour, IEntity
    {


        [Header("Settings")]
        [SerializeField] public Story.InfoEntity entity = null;
        [SerializeField] private LayerMask playerLayerMask = 0;
        [SerializeField] private float visibilityDistance = 3.2f;
        [SerializeField] private bool randomStartingDirection = true;

        [Header("State")]
        [SerializeField] protected bool pooled = false;
        [SerializeField] protected Vector2 direction = Vector2.down;
        [SerializeField] protected bool isMoving = false;
        [SerializeField] private bool hasSeenPlayer;

        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        public EntityState State { get; protected set; } = EntityState.PASSIVE;

        private Animator animator = null; //The parent animator.
        protected readonly List<GameObject> colliders = new List<GameObject>();
        protected Transform playerTransform;
        protected bool playerSeen;

        private int directionXAnimId;
        private int directionYAnimId;
        private int isMovingAnimId;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            directionXAnimId = Animator.StringToHash("directionX");
            directionYAnimId = Animator.StringToHash("directionY");
            isMovingAnimId = Animator.StringToHash("isMoving");
        }


        public virtual void Initialize(bool pooled, Transform playerTransform)
        {
            Logger.Log(this, "Initializing Enemy");
            this.pooled = pooled;
            this.playerTransform = playerTransform;

            direction = Vector2.down;

            if (randomStartingDirection)
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, Mathf.PI)) * Vector2.up;
            }
        }

        public virtual void UpdateBehaviour()
        {
            // switch (State)
            // {
            //     case EntityState.PASSIVE:
            //         UpdatePassive();
            //         break;
            //     case EntityState.ALERTED:
            //         UpdateAlerted();
            //         break;
            //     default:
            //         break;
            // }

            //UpdateDetection();
            //Animate();
        }

        public virtual Story.InfoEntity GetEntityDefinition()
        {
            return this.entity;
        }

        public virtual void SetPosition(Vector2 position)
        {
            this.transform.position = position;
        }

        public virtual void Activate()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            this.gameObject.SetActive(false);
        }

        protected virtual void UpdatePassive()
        {
            // The player triggered our detection, but there anything in the way?
            if (colliders.Contains(playerTransform.gameObject))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
                {
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                    State = EntityState.ALERTED;
                    return;
                }
            }
        }

        protected virtual void UpdateAlerted()
        {

            // if (colliders.Contains(GameManager.Instance.GetPlayer()))
            // {
            //     var hit = CanSeePlayer();
            //     if (hit.collider != null && hit.collider.CompareTag(TagManager.PLAYER))
            //     {
            //         playerSeen = true;
            //         playerSeenEvent.Invoke();
            //     }
            // }

            // if (playerSeen && !colliders.Contains(GameManager.Instance.GetPlayer()))
            // {
            //     State = EntityState.PASSIVE;
            //     playerSeen = false;
            //     playerLeaveEvent.Invoke();
            // }

            // if (playerSeen)
            // {
            //     if (scanning)
            //     {
            //         //direction = player.transform.position - transform.position;
            //     }
            //     else
            //     {
            //         //polyNavAgent.SetDestination(player.transform.position);
            //     }
            // }
        }



        protected RaycastHit2D CanSeePlayer()
        {
            return Physics2D.Raycast(transform.position, (playerTransform.position - transform.position), visibilityDistance, playerLayerMask);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER) && !colliders.Contains(other.gameObject))
            {
                colliders.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER) && colliders.Contains(other.gameObject))
            {
                colliders.Remove(other.gameObject);
            }
        }

        public void Animate()
        {
            if (animator == null)
            {
                return;
            }

            animator.SetFloat(directionXAnimId, direction.x);
            animator.SetFloat(directionYAnimId, direction.y);
            animator.SetBool(isMovingAnimId, isMoving);

        }

        public enum EntityState
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
