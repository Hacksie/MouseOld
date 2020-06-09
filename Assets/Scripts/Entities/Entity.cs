#nullable enable
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using HackedDesign.Story;
using UnityEngine.Experimental.Rendering.Universal;

namespace HackedDesign
{

    public partial class Entity : MonoBehaviour, IEntity
    {
        [Header("Referenced Game Objects")]
        [SerializeField] protected InteractionSpriteOverlay? interactionSprite = null;
        [SerializeField] protected Transform? detection = null;
        protected PolyNav.PolyNavAgent? polyNavAgent = null;
        [SerializeField] protected TripDetection?[]? detections;
        [SerializeField] protected new Collider2D? collider = null;
        [SerializeField] protected ShadowCaster2D? shadow = null;

        [Header("Settings")]
        [SerializeField] public Story.InfoEntity? entity = null;
        [SerializeField] private bool randomStartingDirection = false;
        [SerializeField] protected bool autoInteraction = false;
        [SerializeField] protected bool allowInteraction = false;
        [SerializeField] protected bool allowHack = false;
        [SerializeField] protected bool allowOverload = false;
        [SerializeField] protected bool allowRepeatInteractions = false;
        [SerializeField] protected bool allowNPCAutoInteraction = false;

        [Header("State")]
        [SerializeField] protected AbstractBehaviour? behaviour;
        [SerializeField] protected bool pooled = false;
        [SerializeField] protected Vector2 direction = Vector2.down;
        [SerializeField] protected bool isMoving = false;
        [SerializeField] private bool hasSeenPlayer;

        protected bool overloaded = false;
        protected bool hacked = false;
        protected bool bugged = false;

        // [Header("Events")]
        // public UnityEvent playerSeenEvent;
        // public UnityEvent playerLeaveEvent;
        // public UnityEvent alertEvent;

        [Header("Actions")]
        public UnityEvent? entryActionEvent;
        public UnityEvent? interactActionEvent;
        public UnityEvent? hackActionEvent;
        public UnityEvent? overloadActionEvent;
        public UnityEvent? leaveActionEvent;

        private Animator? animator = null;

        protected readonly List<GameObject> colliders = new List<GameObject>();
        protected PlayerController? playerController = null;
        protected bool playerSeen;

        private int directionXAnimId;
        private int directionYAnimId;
        private int isMovingAnimId;
        public float actionTimer = 0;

        public EntityState State { get; protected set; } = EntityState.Patrol;
        public Vector2 Direction { get => direction; set => direction = value; }
        public TripDetection?[]? Detections { get => detections; private set => detections = value; }
        public Transform Transform => this.transform;
        public InteractionSpriteOverlay? SpriteOverlay => this.interactionSprite;
        public PolyNav.PolyNavAgent? NavAgent => this.polyNavAgent;
        public float LastActionTime { get => this.actionTimer; set => this.actionTimer = value; }
        public InfoEntity? InfoEntity { get => this.entity; set => this.entity = value; }

        public bool AllowInteraction { get => this.allowInteraction; }
        public bool AllowHack { get => this.allowHack; }
        public bool AllowOverload { get => this.allowOverload; }

        public bool Hacked { get => this.hacked; protected set => this.hacked = value; }
        public bool Overloaded { get => this.overloaded; protected set => this.overloaded = value; }


        public AbstractBehaviour? Behaviour
        {
            get
            {
                return behaviour;
            }
            set
            {
                behaviour = value;
                value?.Begin(this);
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
            playerController = GameManager.Instance.Player;
            InitializeDetections();
            colliders.Clear();

            direction = Vector2.down;

            if (randomStartingDirection)
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, Mathf.PI)) * Vector2.up;
            }
            behaviour?.Begin(this);
        }

        protected void InitializeDetections()
        {
            Logger.Log(this, "Initialize Detections");
            detections = GetComponentsInChildren<TripDetection>();
            foreach (var detection in detections)
            {
                detection?.Initialize(this);
            }
        }

        public void UpdateBehaviour()
        {
            UpdateDirection();
            behaviour?.UpdateBehaviour(this);
        }

        // private void LateUpdate()
        // {
        //     Animate();
        // }

        private void UpdateDirection()
        {
            if (polyNavAgent != null && polyNavAgent.hasPath)
            {
                Direction = polyNavAgent.movingDirection.normalized;
                isMoving = (polyNavAgent.movingDirection.sqrMagnitude > Vector2.kEpsilon);
            }
            else
            {
                isMoving = false;
            }
        }


        public virtual void SetPosition(Vector2 position)
        {
            this.transform.position = position;
        }

        public void Activate()
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
            if (shadow != null)
            {
                shadow.enabled = true;
            }
        }

        public void Deactivate()
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (shadow != null)
            {
                shadow.enabled = false;
            }
        }


        public virtual void Entry(GameObject source)
        {
            entryActionEvent?.Invoke();
        }

        public virtual void Invoke(GameObject source)
        {
            if (allowInteraction || hacked || bugged || overloaded)
            {
                interactActionEvent?.Invoke();
            }
        }

        public virtual void Overload(GameObject source)
        {
            if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanOverload && allowOverload)
            {
                Logger.Log(this, "Overload");
                overloaded = true;

                overloadActionEvent?.Invoke();
                GameManager.Instance.Data.Player.ConsumeOverload();
            }
        }

        public virtual void Hack(GameObject source)
        {
            if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanHack && allowHack)
            {
                if (GameManager.Instance.Data.Player.ConsumeHack())
                {
                    hackActionEvent?.Invoke();
                    hacked = true;
                }
            }
            else if (hacked)
            {
                interactActionEvent?.Invoke();
            }
        }

        public virtual void Leave(GameObject source)
        {
            Logger.Log(this, "leave");
            leaveActionEvent?.Invoke();
        }

        public void AddCollider(GameObject collider)
        {
            if (!colliders.Contains(collider))
            {
                colliders.Add(collider);

                if (collider.CompareTag(Tags.PLAYER))
                {
                    interactionSprite?.ShowInteract();
                    playerController?.RegisterTrigger(this);
                    Entry(collider);
                }

                if (collider.CompareTag(Tags.NPC))
                {
                    if (allowNPCAutoInteraction)
                    {
                        Invoke(collider.gameObject);
                    }
                }
            }
        }

        public void RemoveCollider(GameObject collider)
        {
            if (colliders.Contains(collider))
            {
                colliders.Remove(collider);
                if (collider.CompareTag(Tags.PLAYER))
                {
                    interactionSprite?.HideInteract();
                    playerController?.UnregisterTrigger(this);
                    Leave(collider.gameObject);
                }

                if (collider.CompareTag(Tags.NPC))
                {
                    Leave(collider.gameObject);
                }

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
    }
}
