using UnityEngine;

namespace HackedDesign
{
    public class StartMenuState : IState
    {
        private UI.StartMenuPanelPresenter startMenuPanelPresenter;

        public StartMenuState(UI.StartMenuPanelPresenter startMenuPanelPresenter) => this.startMenuPanelPresenter = startMenuPanelPresenter;

        public void Begin()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.startMenuPanelPresenter.Show();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {

            //GameManager.Instance.mainMenu.Repaint();
        }

        public void End() => this.startMenuPanelPresenter.Hide();

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
            GameManager.Instance.SetPlaying();
        }

        public void Select()
        {
            GameManager.Instance.SetSelectMenu();
        }
    }
}