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

            public Vector2 direction = Vector2.zero;
            public LayerMask layerMask;            

            public bool facePlayer = true;

            protected Animator anim; //The parent animator.
            protected List<SpriteRenderer> sprites = new List<SpriteRenderer>();
            protected Transform player;



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

            public void Initialize()
            {
                this.player = CoreGame.Instance.GetPlayer().transform;
            }

            public void Activate()
            {
                //gameObject.SetActive(true);
            }

            public void Deactivate()
            {
                //gameObject.SetActive(false);
            }

            public virtual void FaceDirection(Vector2 direction)
            {
                if (anim != null)
                {
                    anim.SetFloat("directionX", direction.x);
                    anim.SetFloat("directionY", direction.y);
                    anim.SetBool("isMoving", false);
                }
            }


            public virtual void UpdateBehaviour()
            {
                //Debug.Log(this.name + ": update base entity");
                // FIXME: This happens because this can be called before the scene finishes loading
                // if(CoreGame.instance.state.state != GameState.PLAYING)
                // {
                // 	anim.speed = 0;
                // }
                // else if(anim.speed == 0 && CoreGame.instance.state.state == GameState.PLAYING)
                // {
                // 	anim.speed = 1;
                // }


                UpdateLayer(player.position - transform.position);
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