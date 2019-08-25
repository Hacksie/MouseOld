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

			private Input.IInputController input;

			NarrationManager () {
				instance = this;
			}

			public void Initialize (Input.IInputController input) {
				this.input = input;
			}

			public void ShowNarration (Narration narration) {
				input.ResetInput();
				//Input.ResetInputAxes();
				if (narration != null) {
					Debug.Log(this.name + ": show narration " + narration.name);
					currentNarration = narration;
					CoreGame.Instance.SetNarration ();
				} else {
					Debug.LogError (this.name + ": no narration to show");
				}
			}

			public void ShowNarration (string name) {
				ShowNarration (narrationList.FirstOrDefault (e => e != null && e.name == name));
			}

			public void NarrationButtonEvent () {
				Debug.Log (this.name + ": narration button event");

				string nextAction = "";
				if (!string.IsNullOrWhiteSpace (currentNarration.narrationAction)) {
					nextAction = currentNarration.narrationAction;
				}

				currentNarration = null;
				CoreGame.Instance.SetPlaying ();

				Story.ActionManager.instance.Invoke (nextAction);

				// if(currentDialogue.nextDialogue == null)
				// {
				// 	currentDialogue = null;
				// 	GameManager.instance.Resume();
				// }
				// else
				// {
				// 	ShowDialogue(currentDialogue.nextDialogue);
				// }

			}

			public void SetCurrentDialogue (string name) {

			}

			public Narration GetCurrentNarration () {
				return currentNarration;
			}
		}
	}
}