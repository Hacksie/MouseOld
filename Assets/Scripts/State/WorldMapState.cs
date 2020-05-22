using UnityEngine;

namespace HackedDesign
{
    public class WorldMapState : IState
    {
        private UI.WorldMapPresenter worldPanel;

        public WorldMapState(UI.WorldMapPresenter worldPanel)
        {
            this.worldPanel = worldPanel;
        }

        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.worldPanel.Show();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            this.worldPanel.Repaint();
        }

        public void End()
        {
            this.worldPanel.Hide();
        }
    }
}