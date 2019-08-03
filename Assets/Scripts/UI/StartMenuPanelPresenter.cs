using UnityEngine;

namespace HackedDesign {
	public class StartMenuPanelPresenter : MonoBehaviour {

		StartMenuManager startMenuManager;

		private void Show (bool flag) {
			Debug.Log ("Set start menu " + flag);
			this.gameObject.SetActive (flag);
			//Cursor.visible = flag;
		}

		public void Repaint () {
			if (CoreGame.instance.state.state == GameState.STARTMENU && !this.gameObject.activeInHierarchy) {
				Show (true);
			} else if (CoreGame.instance.state.state != GameState.STARTMENU && this.gameObject.activeInHierarchy) {
				Show (false);
			}
		}

		public void Initialize (StartMenuManager startMenuManager) {
			this.startMenuManager = startMenuManager;
		}

	}
}