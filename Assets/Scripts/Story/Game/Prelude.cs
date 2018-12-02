using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		public class Prelude : MonoBehaviour {

			public GameObject kari;
			public GameObject spawnPoint;
			public GameObject startPoint;

			void Start () { }

			// FIXME: Make these register themselves
			public void Prelude1Start () {
				Debug.Log ("Prelude 1 Started");
				kari.SetActive (false);
				GameObject playerObj = CoreGame.instance.GetPlayer ();

				PlayerController pc = playerObj.GetComponent<PlayerController> ();
				pc.LieDown ();

				playerObj.transform.position = spawnPoint.transform.position;

				//prelude4triggers.SetActive(false);
				//prelude5triggers.SetActive(false);
				//RenderSettings.ambientLight = new Color(26, 5, 5);
				//RenderSettings.ambientLight = Color.black;
				//kitty = GameObject.Find("Kitty"); // FIXME: What about if it's saved and restarted
				//FIXME: Create a character manager to pool & find characters

				//kitty.SetActive(false);

				//var spawn = GameObject.FindWithTag (TagManager.SPAWN);
				//CoreGame.instance.TeleportPlayer (spawn.transform);

				Dialogue.NarrationManager.instance.ShowNarration ("Mouse1");
			}

			public void Prelude2Start () {
				Debug.Log ("Prelude 2 Start");
				kari.SetActive (true);

				GameObject playerObj = CoreGame.instance.GetPlayer ();
				playerObj.transform.position = startPoint.transform.position;

				PlayerController pc = playerObj.GetComponent<PlayerController> ();
				pc.StandUp ();

				//prelude4triggers.SetActive(false);
				//prelude5triggers.SetActive(false);				
				//kitty.SetActive(true); // Animate this
				//Dialogue.DialogueManager.instance.ShowDialogue("Kitty1");

				//kitty.SetActive(false); // Animate this
				//Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
				//Dialogue.DialogueManager.instance.ShowDialogue(prelude2);
			}

			public void Prelude3_No_Start () {
				Debug.Log ("Prelude 3 No Start");
				//prelude4triggers.SetActive(false);
				//prelude5triggers.SetActive(false);		
				Dialogue.DialogueManager.instance.ShowDialogue ("Kitty2No");

				//kitty.SetActive(true); // Animate this

			}

			public void Prelude3_Yes_Start () {
				Debug.Log ("Prelude 3 Yes Start");
				//prelude4triggers.SetActive(false);
				//prelude5triggers.SetActive(false);	
				RenderSettings.ambientLight = Color.gray;
				Dialogue.DialogueManager.instance.ShowDialogue ("Kitty2Yes");

				//kitty.SetActive(true); // Animate this

			}

			public void Prelude4Start () {
				Debug.Log ("Prelude 4 Start");
				//prelude4triggers.SetActive(true);
				//prelude5triggers.SetActive(false);
				//kitty.SetActive(true); // Animate this

				//Dialogue.NarrationManager.instance.ShowNarration("Prelude4");
			}

			public void Prelude5Start () {
				Debug.Log ("Prelude 5 Start");
				//prelude4triggers.SetActive(false);
				//prelude5triggers.SetActive(true);
				//kitty.SetActive(true); // Animate this

				//Dialogue.NarrationManager.instance.ShowNarration("Prelude4");
			}

			public void Kari1Start () {
				Debug.Log ("Kari 1 start");
				Dialogue.DialogueManager.instance.ShowDialogue ("Kitty1");
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