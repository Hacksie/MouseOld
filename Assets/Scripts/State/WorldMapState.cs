using UnityEngine;

namespace HackedDesign
{
    public class WorldMapState : IState
    {
        private UI.WorldMapPresenter worldPanel;

        public WorldMapState(UI.WorldMapPresenter worldPanel) => this.worldPanel = worldPanel;

        public void Begin()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.worldPanel.Show();
            this.worldPanel.Repaint();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void End() => this.worldPanel.Hide();

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