using UnityEngine;

namespace HackedDesign
{
    public class LevelCompleteState : IState
    {
        public void Start()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            GameManager.Instance.levelCompletePresenter.Show();
            
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            //GameManager.Instance.levelCompletePresenter.Repaint();
        }

        public void End()
        {
            GameManager.Instance.levelCompletePresenter.Hide();
        }
    }
}