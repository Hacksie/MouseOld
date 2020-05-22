using UnityEngine;

namespace HackedDesign
{
    public class MissionCompleteState : IState
    {
        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            GameManager.Instance.missionCompletePanel.Show();
            GameManager.Instance.missionCompletePanel.Repaint();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void End()
        {
            GameManager.Instance.missionCompletePanel.Hide();
        }
    }
}