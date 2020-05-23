using UnityEngine;

namespace HackedDesign
{
    public class LevelCompleteState : IState
    {
        private UI.LevelCompletePresenter levelCompletePresenter;

        public LevelCompleteState(UI.LevelCompletePresenter levelCompletePresenter) => this.levelCompletePresenter = levelCompletePresenter;

        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.levelCompletePresenter.Show();
            
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void End() => this.levelCompletePresenter.Hide();

        public bool PlayerActionAllowed => false;
    }
}