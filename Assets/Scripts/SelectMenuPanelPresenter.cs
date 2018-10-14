using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class SelectMenuPanelPresenter : MonoBehaviour {

		SelectMenuManager selectMenuManager;


		public void Repaint () {
			Debug.Log("Repaint select");
			if (CoreGame.instance.state == GameState.SELECTMENU) {
				this.gameObject.SetActive (true);
			} else {
				this.gameObject.SetActive (false);
			}

		}

		void Show () {

		}

		public void Initialize (SelectMenuManager selectMenuManager) {
			this.selectMenuManager = selectMenuManager;
		}

	}
}