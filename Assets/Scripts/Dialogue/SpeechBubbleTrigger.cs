using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {

	public class SpeechBubbleTrigger : MonoBehaviour {

		public float startTime;
		public float showTime = 3.0f;

		public string text;

		public GameObject textFieldPrefab;

		public Text textField;

		public Vector3 offset;

		Input.IInputController inputController;

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
			textField.text = text;
			textField.gameObject.SetActive (false);
		}

		// Update is called once per frame
		void Update () {

			if (textField.gameObject.activeInHierarchy) {
				textField.rectTransform.position = Camera.main.WorldToScreenPoint (transform.position + offset);
			}

			if (Time.time - startTime >= showTime) {
				textField.gameObject.SetActive (false);
			}
		}



		private void OnTriggerStay2D (Collider2D other) {
			//textField.text = text;
			if (inputController.InteractButtonUp ()) {
				textField.gameObject.SetActive (true);
				startTime = Time.time;

				Debug.Log (Camera.main.WorldToScreenPoint (transform.position));
			}

		}
	}
}