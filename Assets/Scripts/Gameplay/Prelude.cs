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

			public GameObject kitty;



			// FIXME: Make these register themselves
			public void Prelude1Start () {
				
				Debug.Log("Prelude 1 Started");
				kitty = GameObject.Find("Kitty"); // FIXME: What about if it's saved and restarted
				//FIXME: Create a character manager to pool & find characters
				
				kitty.SetActive(false);
				

				//var spawn = GameObject.FindWithTag (TagManager.SPAWN);
				//CoreGame.instance.TeleportPlayer (spawn.transform);

				Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
			}

			public void Prelude2Start() {
				Debug.Log("Prelude 2 Start");
				kitty.SetActive(false); // Animate this
				Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
				//Dialogue.DialogueManager.instance.ShowDialogue(prelude2);
			}

			public void Prelude3Start() {
				Debug.Log("Prelude 3 Start");
				//kitty.SetActive(true); // Animate this
				Dialogue.NarrationManager.instance.ShowNarration("Prelude3");
			}	

			public void Prelude4Start() {
				Debug.Log("Prelude 4 Start");
				kitty.SetActive(true); // Animate this
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