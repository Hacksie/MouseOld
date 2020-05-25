using UnityEngine;
using UnityEngine.EventSystems;
namespace HackedDesign.UI
{
    public class LevelCompletePresenter : AbstractPresenter
    {
        LevelCompleteManager levelCompleteManager = null;
        [SerializeField] UnityEngine.UI.Button defaultButton = null;

        public override void Repaint()
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
            
            //Logger.Log(this,"level complete repaint");
            /*
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.LevelComplete)
            {
                Show();
            }
            else
            {
                Hide();
            }*/
        }

        public void Initialize(LevelCompleteManager levelCompleteManager)
        {
            this.levelCompleteManager = levelCompleteManager;
        }

        public void CancelEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void OkEvent()
        {
            levelCompleteManager.Next();
        }

    }
}