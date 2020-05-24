using UnityEngine;

namespace HackedDesign
{
    public class MissionCompleteState : IState
    {
        private UI.MissionCompletePresenter missionCompletePresenter;

        public MissionCompleteState(UI.MissionCompletePresenter missionCompletePresenter) => this.missionCompletePresenter = missionCompletePresenter;

        public void Begin()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.missionCompletePresenter.Show();
            this.missionCompletePresenter.Repaint();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void End() => this.missionCompletePresenter.Hide();

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