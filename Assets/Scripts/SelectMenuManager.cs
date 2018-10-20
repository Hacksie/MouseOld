using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class SelectMenuManager : MonoBehaviour {

		public SelectMenuState state = SelectMenuState.INFO;

		public void SetMenuState(SelectMenuState state)
		{
			this.state = state;
		}

		public SelectMenuState GetMenuState()
		{
			return state;
		}

		public void ResumeEvent () {
			CoreGame.instance.SetResume ();
		}

		public enum SelectMenuState {
			INFO,
			TASKS,
			STASH,
			PSYCH
		}
	}
}