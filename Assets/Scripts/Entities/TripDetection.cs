
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HackedDesign
{

    public class TripDetection : MonoBehaviour
    {
        private IEntity parent;


        public void Initialize(IEntity parent)
        {
            this.parent = parent;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                this.parent?.AddCollider(other.gameObject);
            }
        }        

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                this.parent?.AddCollider(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                this.parent?.RemoveCollider(other.gameObject);
            }
        }


    }
}