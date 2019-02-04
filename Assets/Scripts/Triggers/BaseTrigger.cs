using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign {
	namespace Triggers {
		public abstract class BaseTrigger : MonoBehaviour, ITrigger {

			protected Input.IInputController inputController;
			protected new Collider2D collider;
			public new bool enabled = true;

			public void Initialize (Input.IInputController inputController) {
				this.inputController = inputController;
				collider = GetComponent<Collider2D> ();
				if (enabled) {
					Activate ();
				} else {
					Deactivate ();
				}

			}

			public void Activate () {
				if (collider != null) {
					collider.enabled = true;
				}
			}

			public void Deactivate () {
				if (collider != null) {
					collider.enabled = false;
				}
			}

			public abstract void UpdateTrigger ();

			public abstract void Invoke (); 
		}
	}
}