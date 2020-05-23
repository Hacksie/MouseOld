using UnityEngine;

namespace HackedDesign
{
    public class StartMenuState : IState
    {
        private UI.StartMenuPanelPresenter startMenuPanelPresenter;

        public StartMenuState(UI.StartMenuPanelPresenter startMenuPanelPresenter) => this.startMenuPanelPresenter = startMenuPanelPresenter;

        public void Start()
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

        public bool PlayerActionAllowed => false;
    }
}