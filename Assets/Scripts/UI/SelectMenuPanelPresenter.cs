using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class SelectMenuPanelPresenter : MonoBehaviour {

		SelectMenuManager selectMenuManager;

		// Inject these in
		private Story.InfoPanelPresenter infoPanel;
		private Story.TaskPanelPresenter taskPanel;
		private GameObject StashPanel;
		private GameObject PsychPanel;

		public void Initialize(SelectMenuManager selectMenuManager, Story.InfoPanelPresenter infoPanel, Story.TaskPanelPresenter taskPanel)
		{
			this.selectMenuManager = selectMenuManager;
			this.infoPanel = infoPanel;
			this.taskPanel = taskPanel;
		}

		public void Repaint () {
			if (CoreGame.instance.state.state == GameState.SELECTMENU && !this.gameObject.activeInHierarchy) {
				Show (true);
			} else if (CoreGame.instance.state.state != GameState.SELECTMENU && this.gameObject.activeInHierarchy) {
				Show (false);
			}
			infoPanel.Repaint();
			taskPanel.Repaint();
		}		

		private void Show(bool flag) {
			Debug.Log("Repaint select");
			HideAll();
			this.gameObject.SetActive (flag);
			//Cursor.visible = flag;

			if (!flag) {
				return;
			}

			switch(selectMenuManager.GetMenuState())
			{
				case SelectMenuManager.SelectMenuState.INFO:
				//infoPanel.Show(true);
				break;

				case SelectMenuManager.SelectMenuState.TASKS:
				//taskPanel.Show(true);
				break;

				case SelectMenuManager.SelectMenuState.STASH:
				break;

				case SelectMenuManager.SelectMenuState.PSYCH:
				break;
			}
		}

		public void HideAll()
		{
			if(infoPanel != null) {
				//infoPanel.Show(false);
			}

			if(taskPanel != null) {
				//taskPanel.Show(false);
			}
			
			if(StashPanel != null) {
				StashPanel.SetActive(false);
			}

			if(PsychPanel != null) {
				PsychPanel.SetActive(false);
			}
		}
	
		public void InfoClickEvent()
		{
			Debug.Log("Select Menu Info Clicked");
			selectMenuManager.SetMenuState(SelectMenuManager.SelectMenuState.INFO);
			Show(true);
		}

		public void TaskClickEvent()
		{
			Debug.Log("Select Menu Task Clicked");
			selectMenuManager.SetMenuState(SelectMenuManager.SelectMenuState.TASKS);
			Show(true);
		}
	}
}