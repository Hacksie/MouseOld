using UnityEngine;

namespace HackedDesign
{
    public class SelectMenuState : IState
    {
        private UI.SelectMenuPanelPresenter selectMenuPanelPresenter;
        private SelectMenuManager selectMenuManager;

        public SelectMenuState(UI.SelectMenuPanelPresenter selectMenuPanelPresenter, SelectMenuManager selectMenuManager) : this(selectMenuPanelPresenter, selectMenuManager, SelectMenuSubState.Info)
        {
        }

        public SelectMenuState(UI.SelectMenuPanelPresenter selectMenuPanelPresenter, SelectMenuManager selectMenuManager, SelectMenuSubState subState)
        {
            this.selectMenuPanelPresenter = selectMenuPanelPresenter;
            this.selectMenuManager = selectMenuManager;
            this.selectMenuManager.MenuState = subState;

        }

        public void Begin()
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
            this.selectMenuPanelPresenter.Repaint();
        }

        public void End() => this.selectMenuPanelPresenter.Hide();

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
            // Hide panels
            GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {
            // Hide panels
            GameManager.Instance.SetPlaying();
        }
    }
}