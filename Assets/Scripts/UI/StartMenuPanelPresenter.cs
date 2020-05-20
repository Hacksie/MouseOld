using UnityEngine;

namespace HackedDesign.UI
{
    public class StartMenuPanelPresenter : AbstractPresenter
    {
        StartMenuManager startMenuManager = null;

        public override void Repaint()
        {
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.StartMenu)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Initialize(StartMenuManager startMenuManager)
        {
            this.startMenuManager = startMenuManager;
        }

    }
}