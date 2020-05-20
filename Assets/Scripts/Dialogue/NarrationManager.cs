using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
	namespace Dialogue
	{
		public partial class NarrationManager : MonoBehaviour {
			public static NarrationManager instance;

			public Narration currentNarration;
			public List<Narration> narrationList = new List<Narration> ();
			public string narrationResource = @"Narration/";


			NarrationManager () {
				instance = this;
			}

			public void Initialize () {
				LoadNarration();
			}

			private void LoadNarration()
			{
                var jsonTextFiles = Resources.LoadAll<TextAsset>(narrationResource);
				

				foreach(var file in jsonTextFiles)
				{
					var narrations = JsonUtility.FromJson<NarrationHolder>(file.text);
					narrationList.AddRange(narrations.narrations);
					Logger.Log(name, "Narration added " + file.name);
				}				
			}

			public void ShowNarration (Narration narration) {
				if (narration != null) {
					Logger.Log(name, "Show narration " + narration.id);
					currentNarration = narration;
					GameManager.Instance.SetNarration ();
				} else {
					Logger.LogError (name, "No narration to show");
				}
			}

			public void ShowNarration (string id) {
				ShowNarration (narrationList.FirstOrDefault (e => e != null && e.id == id));
			}

			public void NarrationButtonEvent () {
				Logger.Log (name, "Narration button event");

				string nextAction = currentNarration.action;

				currentNarration = null;
				GameManager.Instance.SetPlaying ();

				Story.ActionManager.instance.Invoke (nextAction);
			}

			public Narration GetCurrentNarration () {
				return currentNarration;
			}
		}
	}
}