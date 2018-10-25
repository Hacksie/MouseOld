using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class DoorTrigger : MonoBehaviour, ITrigger {

            Input.IInputController inputController;
            public GameObject doorObject;
            public Collider2D doorCollider;

            public void Initialize (Input.IInputController inputController) {
                Debug.Log ("Initialize world map trigger");
                this.inputController = inputController;

            }

            // Update is called once per frame
            public void UpdateTrigger () { }

            public void Invoke () {
				//CoreGame.instance.GetPlayer().transform.position = transform.position;
                if(doorObject != null )
                    doorObject.SetActive(false);

                if(doorCollider != null)                    
                    doorCollider.enabled = false;
                //CoreGame.instance.SetWorldMap ();
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

                if(doorObject != null )
                    doorObject.SetActive(true);

                if(doorCollider != null)
                    doorCollider.enabled = true;                
            }

            
		}
	}
}