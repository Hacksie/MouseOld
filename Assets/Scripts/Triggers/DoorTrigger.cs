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

            public override void Initialize (Input.IInputController inputController) {
                base.Initialize(inputController);
                Debug.Log ("Initialize door trigger");
                base.Activate();
                
                animator = GetComponent<Animator>();
                allowRepeatTriggers = true;

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

            public override void Leave() {
                open = false;
            }     
		}
	}
}