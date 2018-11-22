using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
	namespace Triggers {
		public class SpeechBubbleTrigger : BaseTrigger, ITrigger {

			public float startTime;
			public float showTime = 3.0f;

			private int currentTextItem = 0;
			public Dialogue.SpeechBubble[] speechBubbles;

			public GameObject textFieldPrefab;
			private GameObject canvas;

			public Text textField;

			public Vector3 offset;


			public bool loop = false;
			bool triggered;


			public new void Initialize (Input.IInputController inputController) {
				base.Initialize(inputController);
				Debug.Log ("Initialize speech bubble");
				
				canvas = GameObject.Instantiate (textFieldPrefab);
				canvas.transform.SetParent (this.transform);
				textField = canvas.GetComponentInChildren<Text> ();
				textField.text = speechBubbles[currentTextItem].text;
				textField.gameObject.SetActive (false);
				
			}

			// Update is called once per frame
			public override void UpdateTrigger () {
				//Debug.Log("Update trigger");


				if (textField.gameObject.activeInHierarchy) {
				 	textField.rectTransform.position = Camera.main.WorldToScreenPoint (transform.position + offset);

				 	if (Time.time - startTime >= showTime) {
				 		textField.gameObject.SetActive (false);
				 	}
				}

			}

			public override void Invoke () {
				Debug.Log("Invoke trigger");
				if (currentTextItem < speechBubbles.Length) {
					textField.text = speechBubbles[currentTextItem].text;
					textField.gameObject.SetActive (true);
					startTime = Time.time;
					currentTextItem++;
					if(loop && currentTextItem >= speechBubbles.Length) currentTextItem = 0;
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