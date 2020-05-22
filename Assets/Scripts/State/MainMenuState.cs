
using UnityEngine;

namespace HackedDesign
{
    public class MainMenuState : IState
    {
        private UI.MainMenuPresenter mainMenu;

        public MainMenuState(UI.MainMenuPresenter mainMenu)
        {
            this.mainMenu = mainMenu;
        }

        public void Start()
        {
            this.mainMenu.Show();
            Cursor.visible = true;
        }

        public void Update()
        {
            
        }

        public void LateUpdate()
        {
            this.mainMenu.Repaint();
        }

        public void End()
        {
            this.mainMenu.Hide();
        }
    }
}