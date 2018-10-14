using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class StartMenuPanelPresenter : MonoBehaviour {

		StartMenuManager startMenuManager;

		// Update is called once per frame
		public void Repaint () {
			//Debug.Log("")
			if (CoreGame.instance.state == GameState.STARTMENU) {
				this.gameObject.SetActive (true);
			} else {
				this.gameObject.SetActive (false);
			}

		}


		public void Initialize (StartMenuManager startMenuManager) {
			this.startMenuManager = startMenuManager;
		}

	}
}