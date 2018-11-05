﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign {
	namespace Triggers {
		public class BaseTrigger : MonoBehaviour, ITrigger {

			protected Input.IInputController inputController;
			protected Collider2D collider;
			public bool enabled = true;

			public void Initialize (Input.IInputController inputController) {
				Debug.Log ("Initialize base trigger");
				this.inputController = inputController;
				collider = GetComponent<Collider2D> ();
				if (enabled) {
					Activate ();
				} else {
					Deactivate ();
				}

			}

			public void Activate () {
				Debug.Log ("Activate collider");
				if (collider != null) {
					collider.enabled = true;
				}
			}

			public void Deactivate () {
				if (collider != null) {
					collider.enabled = false;
				}
			}

			public void UpdateTrigger () {

			}

			public void Invoke () {

			}

		}
	}
}