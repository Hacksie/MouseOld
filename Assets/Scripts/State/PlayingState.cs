using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController playerController;
        private Story.SceneManager actionManager;
        private UI.ActionConsolePresenter actionConsolePresenter;
        private UI.ActionPanelPresenter actionPanelPresenter;
        private UI.TimerPanelPresenter timerPanelPresenter;
        private UI.MinimapPresenter minimapPresenter;

        public PlayingState(PlayerController playerController, Story.SceneManager actionManager, UI.ActionConsolePresenter actionConsolePresenter, UI.ActionPanelPresenter actionPanelPresenter, UI.TimerPanelPresenter timerPanelPresenter, UI.MinimapPresenter minimapPresenter)
        {
            this.playerController = playerController;
            this.actionManager = actionManager;
            this.actionConsolePresenter = actionConsolePresenter;
            this.actionPanelPresenter = actionPanelPresenter;
            this.timerPanelPresenter = timerPanelPresenter;
            this.minimapPresenter = minimapPresenter;
        }

        public void Begin()
        {
            Logger.Log("PlayingState", "Start");
            Time.timeScale = 1;
            Cursor.visible = true;
            GameManager.Instance.SetLight(GameManager.Instance.Data.CurrentLevel.template.startingLight);
            this.actionConsolePresenter.Show();
            this.actionPanelPresenter.Show();
            this.timerPanelPresenter.Show();
            this.minimapPresenter.Show();
        }

        public void Update()
        {
            this.playerController.UpdateBehaviour();
            GameManager.Instance.Data.entityList.ForEach(entity => entity.UpdateBehaviour());
            GameManager.Instance.Data.CurrentLevel.timer.Update();
        }

        public void LateUpdate()
        {
            this.actionManager.UpdateBehaviour();
            this.playerController.Animate();
            AnimateDoors();
            //AnimateEntities();

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
        }

        private void AnimateDoors() => GameManager.Instance.Data.doorList.ForEach(door => door.UpdateAnimation());

        //private void AnimateEntities() => GameManager.Instance.Data.entityList.ForEach(entity => entity.Animate());

        public void Interact()
        {
            this.playerController.Interact();
        }

        public void Hack()
        {
            this.playerController.Hack();
        }

        public void Dash()
        {
            this.playerController.Dash();
        }

        public void Overload()
        {
            this.playerController.Overload();
        }

        public void Start()
        {
            GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {
            GameManager.Instance.SetSelectMenu();
        }
    }
}