using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
	namespace Triggers {
		public class SpeechBubbleTrigger : MonoBehaviour, ITrigger {

			public float startTime;
			public float showTime = 3.0f;

			private int currentTextItem = 0;
			public string[] text;

			public GameObject textFieldPrefab;

			public Text textField;

			public Vector3 offset;

			Input.IInputController inputController;

			public bool loop = false;
			bool triggered;

			// Use this for initialization
			void Start () {

			}

			public void Initialize (Input.IInputController inputController) {
				Debug.Log ("Initialize speech bubble");
				this.inputController = inputController;

				GameObject canvas = GameObject.Instantiate (textFieldPrefab);
				canvas.transform.SetParent (this.transform);
				textField = GetComponentInChildren<Text> ();
				textField.text = text[currentTextItem];
				textField.gameObject.SetActive (false);
			}

			// Update is called once per frame
			public void UpdateTrigger () {
				if (textField.gameObject.activeInHierarchy) {
					textField.rectTransform.position = Camera.main.WorldToScreenPoint (transform.position + offset);

					if (Time.time - startTime >= showTime) {
						textField.gameObject.SetActive (false);
					}
				}

			}

			public void Invoke () {
				if (currentTextItem < text.Length) {
					textField.text = text[currentTextItem];
					textField.gameObject.SetActive (true);
					startTime = Time.time;
					currentTextItem++;
					if(loop && currentTextItem >= text.Length) currentTextItem = 0;
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