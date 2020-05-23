using UnityEngine;

namespace HackedDesign
{
    public class SelectMenuState : IState
    {
        private UI.SelectMenuPanelPresenter selectMenuPanelPresenter;

        public SelectMenuState(UI.SelectMenuPanelPresenter selectMenuPanelPresenter)
        {
            this.selectMenuPanelPresenter = selectMenuPanelPresenter;
        }

        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.selectMenuPanelPresenter.Show();
        }

        public void Update()
        {
            
        }

        public void LateUpdate()
        {
            
        }

        public void End() => this.selectMenuPanelPresenter.Hide();

        public bool PlayerActionAllowed => false;
    }
}