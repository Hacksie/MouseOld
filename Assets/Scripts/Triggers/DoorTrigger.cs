using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class DoorTrigger : BaseTrigger {


            //public GameObject doorObject;
            public Collider2D doorCollider;
            Animator animator;
            bool open = false;

            public new void Initialize (Input.IInputController inputController) {
                base.Initialize(inputController);
                Debug.Log ("Initialize door trigger");
                
                animator = GetComponent<Animator>();

            }

            // Update is called once per frame
            public new void UpdateTrigger () {
                if(animator != null)
                    animator.SetBool("Open", open);
             }

            public new void Invoke () {
                open = true;            
            }

            // TODO: Make door close trigger
            private void OnTriggerStay2D (Collider2D other) {
                if (inputController == null) {
                    Debug.LogWarning ("Trigger isn't tagged as a trigger");
                    return;
                }

                if (inputController.InteractButtonUp ()) {
                    Invoke ();
                }
            }

            private void OnTriggerExit2D (Collider2D other) {
                open = false;         
            }          
		}
	}
}