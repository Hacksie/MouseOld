using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class ExitTrigger : BaseTrigger {


            //public GameObject doorObject;
            public Collider2D doorCollider;
            public Animator animator;
            //bool open = false;

            public override void Initialize () {
                base.Initialize();
                Debug.Log ("Initialize door trigger");
                base.Activate();
                
                animator = GetComponent<Animator>();
                requireInteraction = true;
                

                triggerAction = CoreGame.Instance.State.currentLevel.template.exitAction;

            }

            // Update is called once per frame
            public override void UpdateTrigger (Input.IInputController inputController) {
                //Debug.Log("update trigger " + open);
                // if(animator != null)
                //     animator.SetBool("open", open);
             }

             

  
		}
	}
}