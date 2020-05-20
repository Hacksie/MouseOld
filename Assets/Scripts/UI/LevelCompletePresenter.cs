
namespace HackedDesign.UI
{
    public class LevelCompletePresenter : AbstractPresenter
    {
        LevelCompleteManager levelCompleteManager;

        public override void Repaint()
        {
            if (GameManager.Instance.state.state == GameStateEnum.LEVELCOMPLETE)
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
            GameManager.Instance.SetPlaying();
        }

        public void OkEvent()
        {
            levelCompleteManager.NextLevel();
        }

    }
}