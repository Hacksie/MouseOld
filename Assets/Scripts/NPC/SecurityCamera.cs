using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace NPC {
        public class SecurityCamera : BaseNPCController {
            
            
            public Collider2D trigger;
            public GameObject alertLight;

            public float alertTimer = 5.0f;
            
            private bool triggered = false;
            private float triggerStart = 0;

            public void Start()
            {
                if(alertLight != null) {
                    alertLight.SetActive(false);
                }
            }


            public override void UpdateBehaviour() {

            }

            public override void FaceDirection(Vector2 direction) {

            }

			public override void OnTriggerStay2D (Collider2D other) {
                if(!triggered && other.tag == TagManager.PLAYER) {
                    
                    triggered = true;
                    triggerStart = Time.time;
                    Debug.Log("Countdown start: " + triggerStart);
                }

                if(triggered && (Time.time - triggerStart) > alertTimer)
                {
                    alertLight.SetActive(true);
                    triggered = false;
                    Debug.Log("Security Camera Triggered " + Time.time );
                }              
			}    

            public void OnTriggerExit2D (Collider2D other) {
                if(other.tag == TagManager.PLAYER) {
                    triggered = false;
                }
            }
         }
    }
}