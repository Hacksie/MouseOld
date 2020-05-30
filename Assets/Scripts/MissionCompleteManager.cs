using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MissionCompleteManager : MonoBehaviour 
	{
		Story.SceneManager actionManager = null;

        public void Initialize(Story.SceneManager actionManager) => this.actionManager = actionManager;

        public void ResumeEvent() => actionManager.Invoke(GameManager.Instance.Data.CurrentLevel.template.exitAction);
    }
}

