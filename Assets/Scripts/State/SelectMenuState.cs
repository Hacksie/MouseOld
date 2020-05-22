using UnityEngine;

namespace HackedDesign
{
    public class SelectMenuState : IState
    {
        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            GameManager.Instance.selectMenuPanel.Show();
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
            GameManager.Instance.selectMenuPanel.Hide();
        }
    }
}