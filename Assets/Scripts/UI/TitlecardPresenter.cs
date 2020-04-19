using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    public class TitlecardPresenter : MonoBehaviour
    {
        public string[] titleStrings;
        public Text titleText;
        public string[] nextActions;

        private Story.ActionManager actionManager;

        public void Initialize(Story.ActionManager actionManager)
        {
            this.actionManager = actionManager;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.TITLECARD)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    Show(true);
                }
                
            }
            else if (this.gameObject.activeInHierarchy)
            {
                Show(false);
            }
        }

        public void ClickEvent()
        {
            actionManager.Invoke(this.nextActions[CoreGame.Instance.state.story.act]);
        }        

        private void Show(bool flag)
        {
            this.gameObject.SetActive(flag);
            this.titleText.text = titleStrings[CoreGame.Instance.state.story.act];
        }   


    }
}