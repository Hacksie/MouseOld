using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
	namespace Triggers {
		public class SpeechBubbleTrigger : BaseTrigger {

			public float startTime;
			public float showTime = 3.0f;

			private int currentTextItem = 0;
			public Dialogue.SpeechBubble[] speechBubbles;

			public GameObject textFieldPrefab;
			private GameObject canvas;

			public Text textField;

			public Vector3 offset;


			public bool loop = false;


			public new void Initialize () {
				base.Initialize();
				Debug.Log (this.name + ": initialize speech bubble");
				
				canvas = GameObject.Instantiate (textFieldPrefab);
				canvas.transform.SetParent (this.transform);
				textField = canvas.GetComponentInChildren<Text> ();
				textField.text = speechBubbles[currentTextItem].text;
				textField.gameObject.SetActive (false);
				
			}

			// Update is called once per frame
			public override void UpdateTrigger (Input.IInputController inputController) {

				if (textField.gameObject.activeInHierarchy) {
				 	textField.rectTransform.position = Camera.main.WorldToScreenPoint (transform.position + offset);

				 	if (Time.time - startTime >= showTime) {
				 		textField.gameObject.SetActive (false);
				 	}
				}
			}

			public override void Invoke (UnityEngine.GameObject source) {
				Debug.Log("Invoke trigger");
				if (currentTextItem < speechBubbles.Length) {
					textField.text = speechBubbles[currentTextItem].text;
					textField.gameObject.SetActive (true);
					startTime = Time.time;
					currentTextItem++;
					if(loop && currentTextItem >= speechBubbles.Length) currentTextItem = 0;
				}
			}

		}
	}
}