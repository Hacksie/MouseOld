using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	public class SelectMenuPanelPresenter : MonoBehaviour {

		SelectMenuManager selectMenuManager;

		// Inject these in
		public GameObject InfoPanel;
		public GameObject TaskPanel;
		public GameObject StashPanel;
		public GameObject PsychPanel;


		public void Repaint () {
			Debug.Log("Repaint select");
			if (CoreGame.instance.state == GameState.SELECTMENU) {
				this.gameObject.SetActive (true);
			} else {
				this.gameObject.SetActive (false);
				return;
			}

			HideAll();

			switch(selectMenuManager.GetMenuState())
			{
				case SelectMenuManager.SelectMenuState.INFO:
				break;

				case SelectMenuManager.SelectMenuState.TASKS:
				ShowTasks();
				break;

				case SelectMenuManager.SelectMenuState.STASH:
				break;

				case SelectMenuManager.SelectMenuState.PSYCH:
				break;
			}
		}

		public void HideAll()
		{
			if(InfoPanel != null) {
				InfoPanel.SetActive(false);
			}

			if(TaskPanel != null) {
				TaskPanel.SetActive(false);
			}
			
			if(StashPanel != null) {
				StashPanel.SetActive(false);
			}

			if(PsychPanel != null) {
				PsychPanel.SetActive(false);
			}
		}

		public void ShowTasks()
		{
			TaskPanel.SetActive(true);
		}

		
		public void InfoClickEvent()
		{
			Debug.Log("Select Menu Info Clicked");
			selectMenuManager.SetMenuState(SelectMenuManager.SelectMenuState.INFO);
			Repaint();
		}

		public void TaskClickEvent()
		{
			Debug.Log("Select Menu Task Clicked");
			selectMenuManager.SetMenuState(SelectMenuManager.SelectMenuState.TASKS);
			Repaint();
		}

		public void Initialize (SelectMenuManager selectMenuManager) {
			this.selectMenuManager = selectMenuManager;
		}

	}
}