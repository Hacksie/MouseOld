using UnityEngine;

namespace HackedDesign {
	public class LevelCompleteManager : MonoBehaviour 
	{
		Story.SceneManager actionManager = null;

        public void Initialize(Story.SceneManager actionManager) => this.actionManager = actionManager;

        public void Next() => actionManager.Invoke(GameManager.Instance.Data.CurrentLevel.template.exitAction);
    }
}

