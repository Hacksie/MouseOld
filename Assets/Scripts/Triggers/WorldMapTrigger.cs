using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Triggers {
        public class WorldMapTrigger : MonoBehaviour, ITrigger {

            Input.IInputController inputController;

            public void Initialize (Input.IInputController inputController) {
                Debug.Log ("Initialize world map trigger");
                this.inputController = inputController;

            }

            // Update is called once per frame
            public void UpdateTrigger () { }

            public void Invoke () {
                CoreGame.instance.SetWorldMap ();
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