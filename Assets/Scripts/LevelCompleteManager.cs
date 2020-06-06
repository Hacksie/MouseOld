namespace HackedDesign {
	public class LevelCompleteManager
	{
		Story.SceneManager actionManager = null;

        public void Initialize(Story.SceneManager actionManager) => this.actionManager = actionManager;

        public void Next() => actionManager.Invoke(GameManager.Instance.Data.CurrentLevel.template.exitAction);
    }
}

