using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Dialogue {
		public class DialogueManager : MonoBehaviour, IDialogueManager {
			public static DialogueManager instance;

			public Dialogue currentDialogue;
			public List<Dialogue> dialogueList = new List<Dialogue> ();

			private Input.IInputController input;

			DialogueManager () {
				instance = this;
			}

			public void Initialize (Input.IInputController input) {
				this.input = input;
			}

			public void ShowDialogue (Dialogue dialogue) {
				//input.ResetInput();
				//Input.ResetInputAxes();
				if (dialogue != null) {
					currentDialogue = dialogue;
					CoreGame.instance.SetNarration ();
				} else {
					Debug.LogError ("No dialogue to show");
				}
			}

			public void ShowDialogue (string name) {
				ShowDialogue (dialogueList.FirstOrDefault (e => e.name == name));
			}

			public void DialogueButtonEvent () {
				Debug.Log ("Narration Button Event");

				//currentNarration = null;
				//CoreGame.instance.SetResume ();

				if (currentDialogue.dialogueAction != null) {
					DialogueAction dialogueAction = currentDialogue.dialogueAction;
					currentDialogue = null;
					CoreGame.instance.SetResume ();
					dialogueAction.Invoke ();
				}

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

			public Dialogue GetCurrentDialogue () {
				return currentDialogue;
			}
		}
	}
}