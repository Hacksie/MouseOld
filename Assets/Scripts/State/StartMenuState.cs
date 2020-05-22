using UnityEngine;

namespace HackedDesign
{
    public class StartMenuState : IState
    {
        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            GameManager.Instance.startMenuPanel.Show();
        }

        public void Update()
        {
            
        }

        public void LateUpdate()
        {
            
            //GameManager.Instance.mainMenu.Repaint();
        }

        public void End()
        {
            GameManager.Instance.startMenuPanel.Hide();
        }
    }
}