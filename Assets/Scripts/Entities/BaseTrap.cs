using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Entity {
        public class BaseTrap : BaseEntity {

            public Collider2D trigger;

            public GameObject alertLight;

            public float alertTimer = 5.0f;

            private bool flagged = false;
            private float triggerStart = 0;

            public void Start () {
                if (alertLight != null) {
                    alertLight.SetActive (false);
                }
            }

            public void Initialize () {

            }

            public override void UpdateBehaviour () {
                alertLight.SetActive (CoreGame.instance.state.alertTrap != null && CoreGame.instance.state.alertTrap == this);
            }

            public override void OnTriggerStay2D (Collider2D other) {
                if (!flagged && other.tag == TagManager.PLAYER) {

                    flagged = true;
                    triggerStart = Time.time;
                    Debug.Log ("Countdown start: " + triggerStart);
                }

                if (flagged && (Time.time - triggerStart) > alertTimer) {

                    CoreGame.instance.SetAlert (this);

                    flagged = false;
                    Debug.Log ("Security Camera Triggered " + Time.time);
                }
            }

            public void OnTriggerExit2D (Collider2D other) {
                if (other.tag == TagManager.PLAYER) {
                    flagged = false;
                }
            }

        }
    }
}