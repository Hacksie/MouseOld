using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MissionCompleteManager : MonoBehaviour 
	{
		Story.ActionManager actionManager = null;

        public void Initialize(Story.ActionManager actionManager) => this.actionManager = actionManager;

        public void ResumeEvent() => actionManager.Invoke(GameManager.Instance.GameState.CurrentLevel.template.exitAction);
    }
}

