using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
	namespace Triggers {
		public class StoryInteractiveTrigger : MonoBehaviour, ITrigger {

			public Story.StoryEvent currentStoryEvent;
			public Story.StoryEvent nextStoryEvent;

			Input.IInputController inputController;

			bool triggered;
			public bool loop = false;

			public void Initialize (Input.IInputController inputController) {

				Debug.Log ("Initialize story trigger");

				this.inputController = inputController;

			}

			public void UpdateTrigger () {

			}
			//FIXME: Replace this stuff with StoryEventTransition.cs
			public void Invoke () {
				Debug.Log ("Invoking  story trigger " + currentStoryEvent.name + " -> " + nextStoryEvent.name);
				if (currentStoryEvent != null && currentStoryEvent.currentState == Story.StoryEvent.StoryEventState.STARTED) {
					currentStoryEvent.Complete ();
				}

				if (nextStoryEvent != null) {
					nextStoryEvent.Start ();
				}

			}

			private void OnTriggerStay2D (Collider2D other) {
				//textField.text = text;
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