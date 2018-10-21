using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class DoorTrigger : MonoBehaviour, ITrigger {

            Input.IInputController inputController;

            public void Initialize (Input.IInputController inputController) {
                Debug.Log ("Initialize world map trigger");
                this.inputController = inputController;

            }

            // Update is called once per frame
            public void UpdateTrigger () { }

            public void Invoke () {
				CoreGame.instance.GetPlayer().transform.position = transform.position;
                //CoreGame.instance.SetWorldMap ();
            }

            private void OnTriggerStay2D (Collider2D other) {
                if (inputController == null) {
                    Debug.LogWarning ("Trigger isn't tagged as a trigger");
                    return;
                }

                if (inputController.InteractButtonUp ()) {
                    Invoke ();
                }
            }
		}
	}
}