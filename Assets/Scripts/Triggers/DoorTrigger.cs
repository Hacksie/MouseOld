using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class DoorTrigger : BaseTrigger {


            //public GameObject doorObject;
            public Collider2D doorCollider;
            public Animator animator;
            bool open = false;

            public void Initialize (Input.IInputController inputController) {
                base.Initialize(inputController);
                Debug.Log ("Initialize door trigger");
                base.Activate();
                
                animator = GetComponent<Animator>();

            }

            // Update is called once per frame
            public override void UpdateTrigger () {
                //Debug.Log("update trigger " + open);
                if(animator != null)
                    animator.SetBool("open", open);
             }

            public override void Invoke () {
                
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