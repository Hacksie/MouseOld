using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController playerController;
        private Story.ActionManager actionManager;
        private UI.ActionConsolePresenter actionConsolePresenter;
        private UI.ActionPanelPresenter actionPanelPresenter;
        private UI.TimerPanelPresenter timerPanelPresenter;
        private UI.MinimapPresenter minimapPresenter;

        public PlayingState(PlayerController playerController, Story.ActionManager actionManager, UI.ActionConsolePresenter actionConsolePresenter, UI.ActionPanelPresenter actionPanelPresenter, UI.TimerPanelPresenter timerPanelPresenter, UI.MinimapPresenter minimapPresenter)
        {
            this.playerController = playerController;
            this.actionManager = actionManager;
            this.actionConsolePresenter = actionConsolePresenter;
            this.actionPanelPresenter = actionPanelPresenter;
            this.timerPanelPresenter = timerPanelPresenter;
            this.minimapPresenter = minimapPresenter;
        }

        public void Start()
        {
            Logger.Log("PlayingState", "Start");
            Time.timeScale = 1;
            Cursor.visible = true;
            this.actionConsolePresenter.Show();
            this.actionPanelPresenter.Show();
            this.timerPanelPresenter.Show();
            this.minimapPresenter.Show();
        }

        public void Update()
        {
            this.playerController.UpdateTransform();
            GameManager.Instance.GameState.entityList.ForEach(entity => entity.UpdateBehaviour());
            GameManager.Instance.GameState.CurrentLevel.timer.Update();
        }

        public void LateUpdate()
        {
            this.actionManager.UpdateBehaviour();
            this.playerController.Animate();
            AnimateDoors();
            AnimateEntity();

            this.actionConsolePresenter.Repaint();
            this.actionPanelPresenter.Repaint();
            this.timerPanelPresenter.Repaint();
            this.minimapPresenter.Repaint();
        }

        public void End()
        {
            this.actionConsolePresenter.Hide();
            this.actionPanelPresenter.Hide();
            this.timerPanelPresenter.Hide();
            this.minimapPresenter.Hide();
            Logger.Log("PlayingState", "End");
        }

        private void AnimateDoors() => GameManager.Instance.GameState.doorList.ForEach(door => door.UpdateAnimation());

        private void AnimateEntity() => GameManager.Instance.GameState.entityList.ForEach(entity => entity.Animate());

        public bool PlayerActionAllowed => true;
    }
}