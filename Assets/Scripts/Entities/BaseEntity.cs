using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign
{
    namespace Entity
    {
        public class BaseEntity : MonoBehaviour
        {
            
            protected Animator anim; //The parent animator.
            protected List<SpriteRenderer> sprites = new List<SpriteRenderer>();
            protected Transform player;

            [Header("Settings")]
            public LayerMask layerMask;            

            [Header("State")]
            public Vector2Int direction = Vector2Int.zero;
            

            public bool facePlayer = true;





            // public List<Vector3> patrolPath;
            // public int patrolPathLength = 4;

            protected float visibilityDistance = 3.2f;

            // Use this for initialization
            protected void Start()
            {
                anim = transform.GetComponent<Animator>();
                sprites.Add(GetComponent<SpriteRenderer>());
                sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
                //polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();
                //NavMeshAgent2D navMeshAgent = GetComponent<NavMeshAgent2D> ();

                //player = CoreGame.instance.GetPlayer ().transform;

            }

            public virtual void Initialize()
            {
                this.player = CoreGame.Instance.GetPlayer().transform;
            }

            public virtual void FaceDirection(Vector2 direction)
            {
                this.direction = NormaliseDirectionVector(direction);

                if (anim != null)
                {
			        anim.SetFloat ("moveX", this.direction.x);
				    anim.SetFloat ("moveY", this.direction.y);
				//anim.SetFloat ("directionX", movementVector.x);
				//anim.SetFloat ("directionY", movementVector.y);
				    anim.SetBool ("isMoving", true);                    
                    // anim.SetFloat("directionX", this.direction.x);
                    // anim.SetFloat("directionY", this.direction.y);
                    // anim.SetBool("isMoving", false);
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

            public void UpdateLayer(Vector2 direction)
            {

                if (direction.y >= 0)
                {
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        sprites[i].sortingOrder = 160 + i;
                    }
                }
                else
                {
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        sprites[i].sortingOrder = 100 + i;
                    }
                }
            }

            public virtual void OnTriggerStay2D(Collider2D other)
            {

            }



            // public enum NPCState
            // {
            //     STANDING,
            //     PATROLLING,
            //     SEEKING,
            //     HUNTING,
            //     FIGHTING,
            //     STUNNED
            // }

        }
    }
}