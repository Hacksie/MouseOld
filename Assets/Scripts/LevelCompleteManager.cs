using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class LevelCompleteManager : MonoBehaviour 
	{
		Story.ActionManager actionManager = null;

		public void Initialize(Story.ActionManager actionManager)
		{
			this.actionManager = actionManager;
		}

		public void ResumeEvent () {
			//FIXME: This would return to hub room
			actionManager.Invoke(CoreGame.Instance.state.currentLevel.template.exitAction);
			//CoreGame.Instance.EndGame ();
		}
	}
}

