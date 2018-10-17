using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class StartMenuPanelPresenter : MonoBehaviour {

		StartMenuManager startMenuManager;

		// Update is called once per frame
		public void Repaint () {
			
			if (CoreGame.instance.state == GameState.STARTMENU) {
				Debug.Log("Set start menu active");
				this.gameObject.SetActive (true);
			} else {
				Debug.Log("Set start menu inactive");
				this.gameObject.SetActive (false);
			}

		}


		public void Initialize (StartMenuManager startMenuManager) {
			this.startMenuManager = startMenuManager;
		}

	}
}