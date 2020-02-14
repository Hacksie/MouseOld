using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Dialogue {
		public class NarrationManager : MonoBehaviour, INarrationManager {
			public static NarrationManager instance;

			public Narration currentNarration;
			public List<Narration> narrationList = new List<Narration> ();
			public string narrationResource = @"Narration/";

			private Input.IInputController input;

			NarrationManager () {
				instance = this;
			}

			public void Initialize (Input.IInputController input) {
				this.input = input;
				LoadNarration();
			}

			private void LoadNarration()
			{
                var jsonTextFiles = Resources.LoadAll<TextAsset>(narrationResource);
				

				foreach(var file in jsonTextFiles)
				{
					var narrations = JsonUtility.FromJson<NarrationHolder>(file.text);
					narrationList.AddRange(narrations.narrations);
					Debug.Log(this.name +": narration added");
				}				
			}

			public void ShowNarration (Narration narration) {
				input.ResetInput(); // Is there a better way of doing this? Move to the presenter
				//Input.ResetInputAxes();
				if (narration != null) {
					Debug.Log(this.name + ": show narration " + narration.id);
					currentNarration = narration;
					CoreGame.Instance.SetNarration ();
				} else {
					Debug.LogError (this.name + ": no narration to show");
				}
			}

			public void ShowNarration (string id) {
				ShowNarration (narrationList.FirstOrDefault (e => e != null && e.id == id));
			}

			public void NarrationButtonEvent () {
				Debug.Log (this.name + ": narration button event");

				string nextAction = currentNarration.action;

				currentNarration = null;
				CoreGame.Instance.SetPlaying ();

				Story.ActionManager.instance.Invoke (nextAction);
			}

			public void SetCurrentDialogue (string name) {

			}

			public Narration GetCurrentNarration () {
				return currentNarration;
			}

			public class NarrationHolder {
				public List<Narration> narrations;
			}
		}
	}
}