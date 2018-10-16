using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class LightSwitchTrigger : MonoBehaviour, ITrigger {

			Input.IInputController inputController;

			public void Initialize (Input.IInputController inputController) {
				Debug.Log ("Initialize light switch trigger");
				this.inputController = inputController;

			}

			// Update is called once per frame
			public void UpdateTrigger () { }

			public void Invoke () {
				RenderSettings.ambientLight = RenderSettings.ambientLight == Color.gray? Color.black : Color.gray;
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