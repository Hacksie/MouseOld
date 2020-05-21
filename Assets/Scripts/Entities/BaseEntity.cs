using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign
{
    public class BaseEntity : MonoBehaviour
    {
        protected Animator animator; 
        protected Transform player;

        [Header("Settings")]
        public LayerMask layerMask;

        [Header("State")]
        public Vector2Int direction = Vector2Int.zero;

        protected float visibilityDistance = 3.2f;

        protected void Start()
        {
            animator = transform.GetComponent<Animator>();
        }

        public virtual void Initialize()
        {
            this.player = GameManager.Instance.GetPlayer().transform;
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