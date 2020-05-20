using UnityEngine;

namespace HackedDesign.UI
{
    public class StartMenuPanelPresenter : AbstractPresenter
    {
        GameState state;
        StartMenuManager startMenuManager = null;

        public override void Repaint()
        {
            if (state.state == GameStateEnum.STARTMENU)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Initialize(GameState state, StartMenuManager startMenuManager)
        {
            this.state = state;
            this.startMenuManager = startMenuManager;
        }

    }
}