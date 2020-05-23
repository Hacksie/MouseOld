using UnityEngine;

namespace HackedDesign {
	public class LevelCompleteManager : MonoBehaviour 
	{
		Story.ActionManager actionManager = null;

        public void Initialize(Story.ActionManager actionManager) => this.actionManager = actionManager;

        public void Next() => actionManager.Invoke(GameManager.Instance.GameState.CurrentLevel.template.exitAction);
    }
}

