using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using HackedDesign.Story;

namespace HackedDesign
{
    [RequireComponent(typeof(Animator))]
    public partial class Entity : MonoBehaviour, IEntity
    {
        [Header("Referenced Game Objects")]
        [SerializeField] protected InteractionSpriteOverlay interactionSprite;
        protected PolyNav.PolyNavAgent? polyNavAgent = null;

        [Header("Settings")]
        [SerializeField] public Story.InfoEntity entity = null;
        
        
        [SerializeField] private bool randomStartingDirection = true;
        

        [Header("State")]
        [SerializeField] protected AbstractBehaviour behaviour;

        [SerializeField] protected bool pooled = false;
        [SerializeField] protected Vector2 direction = Vector2.down;
        [SerializeField] protected bool isMoving = false;
        [SerializeField] private bool hasSeenPlayer;

        [Header("Events")]
        public UnityEvent playerSeenEvent;
        public UnityEvent playerLeaveEvent;
        public UnityEvent alertEvent;

        public EntityState State { get; protected set; } = EntityState.Patrol;
        public Vector2 Direction { get => direction; set => direction = value; }

        private Animator animator = null; 

        protected readonly List<GameObject> colliders = new List<GameObject>();
        protected Transform playerTransform;
        protected bool playerSeen;

        private int directionXAnimId;
        private int directionYAnimId;
        private int isMovingAnimId;

        public float actionTimer = 0;

        public AbstractBehaviour Behaviour
        {
            get
            {
                return behaviour;
            }
            set
            {
                behaviour = value;
                value.Begin();
            }
        }


        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            interactionSprite = GetComponentInChildren<InteractionSpriteOverlay>();
            polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();
            directionXAnimId = Animator.StringToHash("directionX");
            directionYAnimId = Animator.StringToHash("directionY");
            isMovingAnimId = Animator.StringToHash("isMoving");
            
        }

        public Transform Transform => this.transform;

        public InteractionSpriteOverlay SpriteOverlay => this.interactionSprite;

        public PolyNav.PolyNavAgent NavAgent => this.polyNavAgent;

        public float LastActionTime { get => this.actionTimer; set => this.actionTimer = value; }

        public virtual void Initialize(bool pooled, Transform playerTransform)
        {
            Logger.Log(this, "Initializing Entity");
            this.pooled = pooled;
            this.playerTransform = playerTransform;

            direction = Vector2.down;

            if (randomStartingDirection)
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, Mathf.PI)) * Vector2.up;
            }

            if (polyNavAgent != null && polyNavAgent.isActiveAndEnabled)
            {
                polyNavAgent.map = GameManager.Instance.PolyNav;

            }
            InitializeDetections();
            behaviour.Begin();
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

            behaviour.UpdateBehaviour(this);
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

        public void InvokeSeenPlayer()
        {
            //this.playerSeen.invoke();
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

        }

        

        protected virtual void UpdateAlerted()
        {
            // var hit = CanSeePlayer();
            // if (hit.collider != null && hit.collider.CompareTag(Tags.PLAYER))
            // {
            // }
            // else
            // {
            //     State = EntityState.Seek;
            //     playerSeen = false;
            //     playerLeaveEvent.Invoke();
            // }

            /*
            if (colliders.Contains(playerTransform.gameObject))
            {
                var hit = CanSeePlayer();
                if (hit.collider != null && hit.collider.CompareTag(Tags.PLAYER))
                {
                    Logger.Log(this, "hit");
                    playerSeen = true;
                    playerSeenEvent.Invoke();
                    State = EntityState.Alerted;
                    return;
                }
            }            

            if (playerSeen && !colliders.Contains(playerTransform.gameObject))
            {
                State = EntityState.Seeking;
                playerSeen = false;
                playerLeaveEvent.Invoke();
            }   */

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

        protected virtual void UpdateSeek()
        {
        }

        protected virtual void UpdateHunt()
        {

        }

        // protected RaycastHit2D CanSeePlayer()
        // {
        //     return Physics2D.Raycast(transform.position, (playerTransform.position - transform.position), visibilityDistance, playerLayerMask);
        // }

        public void AddCollider(GameObject collider)
        {
            if (!colliders.Contains(collider))
            {
                colliders.Add(collider);
            }
        }

        public void RemoveCollider(GameObject collider)
        {
            if (colliders.Contains(collider))
            {
                colliders.Remove(collider);
            }
        }

        public List<GameObject> GetColliders()
        {
            return this.colliders;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                AddCollider(other.gameObject);
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                RemoveCollider(other.gameObject);
            }
        }

        public void Animate()
        {
            if (animator == null || !animator.enabled)
            {
                return;
            }

            animator.SetFloat(directionXAnimId, direction.x);
            animator.SetFloat(directionYAnimId, direction.y);
            animator.SetBool(isMovingAnimId, isMoving);
        }

        protected virtual void OnDrawGizmos()
        {

            //Physics2D.Raycast(transform.position, (playerTransform.position - transform.position), visibilityDistance, playerLayerMask);

            //Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position));
            /*
            switch (State)
            {
                case EntityState.Passive:
                    Gizmos.DrawIcon(transform.position + Vector3.up, "refresh-hs.png", true);
                    break;
                case EntityState.Alerted:
                    Gizmos.DrawIcon(transform.position + Vector3.up, "skull-hs.png", true);
                    break;
            }*/
        }
    }
}
