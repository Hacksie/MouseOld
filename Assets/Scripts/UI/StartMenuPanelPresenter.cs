using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class StartMenuPanelPresenter : MonoBehaviour {

		StartMenuManager startMenuManager;

		public void Show(bool flag)
		{
				Debug.Log("Set start menu " + flag);
				this.gameObject.SetActive (flag);			
		}


		public void Initialize (StartMenuManager startMenuManager) {
			this.startMenuManager = startMenuManager;
		}

	}
}