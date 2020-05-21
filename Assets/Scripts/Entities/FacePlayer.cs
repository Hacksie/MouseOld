using UnityEngine;

namespace HackedDesign.Entities
{
    [RequireComponent(typeof(Animator))]
    public class FacePlayer : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField]
        private Animator animator = null; //The parent animator.
        private Transform player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            player = GameManager.Instance.GetPlayer().transform;
        }

        protected void Start()
        {
            if (animator == null)
            {
                Logger.LogError(name, "No animation set");
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.GameState.IsPlaying())
                return;

            var tempdirection = NormaliseDirectionVector(DirectionToPlayer());

            if (animator != null)
            {
                animator.SetFloat("directionX", tempdirection.x);
                animator.SetFloat("directionY", tempdirection.y);
                //animator.SetBool("isMoving", true);
            }

        }

        protected Vector2Int NormaliseDirectionVector(Vector2 direction)
        {
            return Vector2Int.RoundToInt(direction.normalized);
        }

        private Vector3 DirectionToPlayer()
        {
            return (player.position - transform.position);
        }
    }
}

