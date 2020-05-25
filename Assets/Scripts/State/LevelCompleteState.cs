using UnityEngine;

namespace HackedDesign
{
    public class LevelCompleteState : IState
    {
        private UI.LevelCompletePresenter levelCompletePresenter;

        public LevelCompleteState(UI.LevelCompletePresenter levelCompletePresenter) => this.levelCompletePresenter = levelCompletePresenter;

        public void Begin()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.levelCompletePresenter.Show();
            this.levelCompletePresenter.Repaint();
            
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void End() => this.levelCompletePresenter.Hide();

        public void Interact()
        {
            
        }

        public void Hack()
        {
            
        }

        public void Dash()
        {
            
        }

        public void Overload()
        {
            
        }

        public void Start()
        {
            
        }

        public void Select()
        {
            
        }
    }
}