using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController playerController;
        private Story.ActionManager actionManager;

        public PlayingState(PlayerController playerController, Story.ActionManager actionManager)
        {
            this.playerController = playerController;
            this.actionManager = actionManager;
        }

        public void Start()
        {
            Time.timeScale = 1;
            Cursor.visible = true;
            GameManager.Instance.actionConsolePanel.Show();
            GameManager.Instance.actionPanel.Show();
            GameManager.Instance.timerPanel.Show();
            GameManager.Instance.minimapPanel.Show();
            Logger.Log("PlayingState", "Start");
        }

        public void Update()
        {
            this.playerController.UpdateTransform();

            foreach (var entity in GameManager.Instance.GameState.entityList)
            {
                entity.UpdateBehaviour();
            }

            //PlayingEntityUpdate();
            //PlayingTriggerUpdate();
            GameManager.Instance.GameState.CurrentLevel.timer.Update();
        }

        public void LateUpdate()
        {
            this.actionManager.UpdateBehaviour();


            this.playerController.Animate();
            AnimateDoors();
            AnimateEntity();


            GameManager.Instance.actionConsolePanel.Repaint();
            GameManager.Instance.actionPanel.Repaint();
            GameManager.Instance.timerPanel.Repaint();
            GameManager.Instance.minimapPanel.Repaint();
        }

        public void End()
        {
            GameManager.Instance.actionConsolePanel.Hide();
            GameManager.Instance.actionPanel.Hide();
            GameManager.Instance.timerPanel.Hide();
            GameManager.Instance.minimapPanel.Hide();
            Logger.Log("PlayingState", "End");
        }

        private void AnimateDoors()
        {
            foreach (var door in GameManager.Instance.GameState.doorList)
            {
                door.UpdateAnimation();
            }
        }

        private void AnimateEntity()
        {
            foreach (var entity in GameManager.Instance.GameState.entityList)
            {
                entity.Animate();
            }
        }
    }
}