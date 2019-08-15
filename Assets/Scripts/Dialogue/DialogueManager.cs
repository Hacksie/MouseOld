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
					CoreGame.Instance.SetDialogue ();
				} else {
					Debug.LogError ("No dialogue to show");
				}
			}

			public void ShowDialogue (string name) {
				ShowDialogue (dialogueList.FirstOrDefault (e => e.name == name));
			}

			public void DialogueButton1Event () {
				Debug.Log ("Dialogue Button 1 Event");

				if (currentDialogue.dialogueAction1 != null) {
					DialogueAction dialogueAction = currentDialogue.dialogueAction1;
					currentDialogue = null;
					CoreGame.Instance.SetPlaying ();
					dialogueAction.Invoke ();
				}
			}

			public void DialogueButton2Event () {
				Debug.Log ("Dialogue Button 2 Event");

				if (currentDialogue.dialogueAction2 != null) {
					DialogueAction dialogueAction = currentDialogue.dialogueAction2;
					currentDialogue = null;
					CoreGame.Instance.SetPlaying ();
					dialogueAction.Invoke ();
				}
			}			

			public void DialogueButton3Event () {
				Debug.Log ("Dialogue Button 3 Event");

				if (currentDialogue.dialogueAction3 != null) {
					DialogueAction dialogueAction = currentDialogue.dialogueAction3;
					currentDialogue = null;
					CoreGame.Instance.SetPlaying ();
					dialogueAction.Invoke ();
				}
			}		

			public void DialogueButton4Event () {
				Debug.Log ("Dialogue Button 4 Event");

				if (currentDialogue.dialogueAction4 != null) {
					DialogueAction dialogueAction = currentDialogue.dialogueAction4;
					currentDialogue = null;
					CoreGame.Instance.SetPlaying ();
					dialogueAction.Invoke ();
				}
			}						

			public void SetCurrentDialogue (string name) {

			}

			public Dialogue GetCurrentDialogue () {
				return currentDialogue;
			}
		}
	}
}