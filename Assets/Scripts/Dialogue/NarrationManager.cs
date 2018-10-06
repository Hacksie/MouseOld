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
				//input.ResetInput();
				//Input.ResetInputAxes();
				if (narration != null) {
					currentNarration = narration;
					CoreGame.instance.Dialogue ();
				} else {
					Debug.LogError ("No dialogue to show");
				}
			}

			public void ShowNarration (string name) {
				ShowNarration (narrationList.FirstOrDefault (e => e.name == name));
			}

			public void NarrationButtonEvent () {
				Debug.Log ("Narration Button Event");

				currentNarration = null;
				CoreGame.instance.Resume ();

				// if (currentNarration.dialogueAction != null) {
				// 	DialogueAction dialogueAction = currentNarration.dialogueAction;
				// 	currentNarration = null;
				// 	CoreGame.instance.Resume ();
				// 	dialogueAction.Invoke ();
				// }

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