using UnityEngine;

namespace HackedDesign.Entities
{
    public class FacePlayer : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField]
        private Animator animator = null; //The parent animator.

        protected void Start()
        {
            if (animator == null)
            {
                Logger.LogError(this.name, "No animation set");
            }          
        }

        private void Update()
        {

            var tempdirection = NormaliseDirectionVector(DirectionToPlayer());

            if (animator != null)
            {
                animator.SetFloat("moveX", tempdirection.x);
                animator.SetFloat("moveY", tempdirection.y);
                //animator.SetBool("isMoving", true);
            }
        }

        protected Vector2Int NormaliseDirectionVector(Vector2 direction)
        {
            return Vector2Int.RoundToInt(direction.normalized);
        }

        private Vector3 DirectionToPlayer()
        {
            return (CoreGame.Instance.GetPlayer().transform.position - transform.position);
        }
    }
}
