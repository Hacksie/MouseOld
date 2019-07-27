using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Triggers {
        public class InteractTrigger: BaseTrigger {
            // Start is called before the first frame update

            // Update is called once per frame
            public override void UpdateTrigger () {
                //Debug.Log("update trigger " + open);

             }

            public override void Invoke () {
                
                Debug.Log("End level");
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