using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using HackedDesign.Story;

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
        [SerializeField] private bool facePlayer = false;

        [Header("State")]
        [SerializeField] protected bool pooled = false;
        [SerializeField] protected Vector2 direction = Vector2.down;
        [SerializeField] protected bool isMoving = false;
        [SerializeField] private bool hasSeenPlayer;

        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        public EntityState State { get; protected set; } = EntityState.Passive;

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



            InitializeDetections();
        }

        protected void InitializeDetections()
        {
            Logger.Log(this, "InitializeDetections");
            var detections = GetComponentsInChildren<TripDetection>();
            foreach (var detection in detections)
            {
                detection.Initialize(this);
            }
        }

        public virtual void UpdateBehaviour()
        {
            if(facePlayer)
            {
                direction = (playerTransform.position - this.transform.position).normalized;
            }
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


        public virtual void SetEntityDefinition(InfoEntity entity)
        {
            this.entity = entity;
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
                    State = EntityState.Alerted;
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

        public void AddCollider(GameObject collider)
        {
            if (!colliders.Contains(collider.gameObject))
            {
                colliders.Add(collider.gameObject);
            }
        }

        public void RemoveCollider(GameObject collider)
        {
            if (colliders.Contains(collider.gameObject))
            {
                colliders.Remove(collider.gameObject);
            }
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER))
            {
                AddCollider(other.gameObject);
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.PLAYER))
            {
                RemoveCollider(other.gameObject);
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

        private void OnDrawGizmos()
        {
            switch (State)
            {
                case EntityState.Passive:
                    Gizmos.DrawIcon(transform.position + Vector3.up, "refresh-hs.png", true);
                    break;
                case EntityState.Alerted:
                    Gizmos.DrawIcon(transform.position + Vector3.up, "skull-hs.png", true);
                    break;
            }
        }

        public enum EntityState
        {
            Passive,
            Alerted,
            Seeking,
            Responding,
            Hunting,
            Fighting,
            Stunned
        }
    }
}
