using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign
{
    namespace Entities
    {
        public class BaseEntity : MonoBehaviour
        {
            
            protected Animator animator; //The parent animator.
            protected Transform player;

            [Header("Settings")]
            public LayerMask layerMask;            

            [Header("State")]
            public Vector2Int direction = Vector2Int.zero;
            




            protected float visibilityDistance = 3.2f;

            // Use this for initialization
            protected void Start()
            {
                animator = transform.GetComponent<Animator>();
            }

            public virtual void Initialize()
            {
                this.player = GameManager.Instance.GetPlayer().transform;
            }

            public virtual void FaceDirection(Vector2 direction)
            {
                this.direction = NormaliseDirectionVector(direction);

                if (animator != null)
                {
			        animator.SetFloat ("moveX", this.direction.x);
				    animator.SetFloat ("moveY", this.direction.y);
				    animator.SetBool ("isMoving", true);                    
                }
            }

            protected Vector2Int NormaliseDirectionVector(Vector2 direction)
            {
                return Vector2Int.RoundToInt(direction.normalized);
            }


            public virtual void UpdateBehaviour()
            {

            }

            public RaycastHit2D CanSeePlayer()
            {
                return Physics2D.Raycast(transform.position, (player.position - transform.position), visibilityDistance, layerMask);
            }

            public Vector3 DirectionToPlayer()
            {
                return (player.position - transform.position);
            }



            public virtual void OnTriggerStay2D(Collider2D other)
            {

            }


        }
    }
}