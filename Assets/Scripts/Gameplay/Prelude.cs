using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		public class Prelude : MonoBehaviour {

			// public Dialogue.Narration prelude1;
			// public Dialogue.Narration prelude2;
			// public Dialogue.Narration prelude3;
			// public Dialogue.Narration prelude4;

			//public GameObject kitty;

			public GameObject prelude3triggers;
			public GameObject prelude4triggers;

			void Start() {
				//prelude3triggers.SetActive(false);
				//prelude4triggers.SetActive(false);
			}

			// FIXME: Make these register themselves
			public void Prelude1Start () {
				Debug.Log("Prelude 1 Started");
				prelude3triggers.SetActive(false);
				prelude4triggers.SetActive(false);
				//kitty = GameObject.Find("Kitty"); // FIXME: What about if it's saved and restarted
				//FIXME: Create a character manager to pool & find characters
				
				//kitty.SetActive(false);
				

				//var spawn = GameObject.FindWithTag (TagManager.SPAWN);
				//CoreGame.instance.TeleportPlayer (spawn.transform);

				Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
			}

			public void Prelude2Start() {
				Debug.Log("Prelude 2 Start");
				prelude3triggers.SetActive(false);
				prelude4triggers.SetActive(false);				
				//kitty.SetActive(true); // Animate this
				Dialogue.DialogueManager.instance.ShowDialogue("Prelude2");

				//kitty.SetActive(false); // Animate this
				//Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
				//Dialogue.DialogueManager.instance.ShowDialogue(prelude2);
			}

			public void Prelude3Start() {
				Debug.Log("Prelude 3 Start");
				prelude3triggers.SetActive(true);
				prelude4triggers.SetActive(false);
				//kitty.SetActive(true); // Animate this
				
			}	

			public void Prelude4Start() {
				Debug.Log("Prelude 4 Start");
				prelude3triggers.SetActive(false);
				prelude4triggers.SetActive(true);
				//kitty.SetActive(true); // Animate this
				
				//Dialogue.NarrationManager.instance.ShowNarration("Prelude4");
			}	


			// public void Prelude5Start() {
			// 	Debug.Log("Prelude 5 Start");
			// 	kitty.SetActive(false); // Animate this

			// 	// Activate the door
				
			// 	//Dialogue.DialogueManager.instance.ShowDialogue(prelude3);
			// }						
		}
	}
}