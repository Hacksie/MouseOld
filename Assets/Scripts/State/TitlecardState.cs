using UnityEngine;

namespace HackedDesign
{
    public class TitlecardState : IState
    {
        private UI.TitlecardPresenter titlecardPresenter;

        public TitlecardState(UI.TitlecardPresenter titlecardPresenter) => this.titlecardPresenter = titlecardPresenter;

        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.titlecardPresenter.Show();
            this.titlecardPresenter.Repaint();
        }

        public void Update()
        {
            
        }

        public void LateUpdate()
        {
         
        }

        public void End() => this.titlecardPresenter.Hide();

        public bool PlayerActionAllowed => false;
    }
}