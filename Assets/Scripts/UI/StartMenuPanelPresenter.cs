using UnityEngine;

namespace HackedDesign {
	public class StartMenuPanelPresenter : MonoBehaviour {

		State.GameState state;
		StartMenuManager startMenuManager = null;


		public void Repaint()
		{
			if (state.state == State.GameStateEnum.STARTMENU && !gameObject.activeInHierarchy) {
				Show (true);
			} else if (state.state != State.GameStateEnum.STARTMENU && gameObject.activeInHierarchy) {
				Show (false);
			}
		}

		public void Initialize(State.GameState state, StartMenuManager startMenuManager)
		{
			this.state = state;
			this.startMenuManager = startMenuManager;
		}

		private void Show(bool flag)
		{
			Logger.Log(name, "Set start menu ", flag.ToString());
			gameObject.SetActive(flag);
		}

	}
}