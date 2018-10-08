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
					CoreGame.instance.SetNarration ();
				} else {
					Debug.LogError ("No dialogue to show");
				}
			}

			public void ShowNarration (string name) {
				ShowNarration (narrationList.FirstOrDefault (e => e.name == name));
			}

			public void NarrationButtonEvent () {
				Debug.Log ("Narration Button Event");

				//currentNarration = null;
				//CoreGame.instance.SetResume ();

				if (currentNarration.narrationAction != null) {
					NarrationAction narrationAction = currentNarration.narrationAction;
					currentNarration = null;
					CoreGame.instance.SetResume ();
					narrationAction.Invoke ();
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

			public Narration GetCurrentNarration () {
				return currentNarration;
			}
		}
	}
}