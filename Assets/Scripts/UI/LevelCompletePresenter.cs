
namespace HackedDesign.UI
{
    public class LevelCompletePresenter : AbstractPresenter
    {
        LevelCompleteManager levelCompleteManager;

        public override void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.LEVELCOMPLETE)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Initialize(LevelCompleteManager levelCompleteManager)
        {
            this.levelCompleteManager = levelCompleteManager;
        }

        public void CancelEvent()
        {
            CoreGame.Instance.SetPlaying();
        }

        public void OkEvent()
        {
            levelCompleteManager.NextLevel();
        }

    }
}