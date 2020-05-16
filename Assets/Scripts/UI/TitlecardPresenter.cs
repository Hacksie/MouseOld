using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HackedDesign
{
    public class TitlecardPresenter : MonoBehaviour
    {
        [Header("Referenced GameObjects")]
        public Text titleText;
        public Button continueButton;
        [Header("Settings")]
        public string[] titleStrings;
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
                if (!gameObject.activeInHierarchy)
                {
                    Show(true);
                    EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
                }
                
            }
            else if (gameObject.activeInHierarchy)
            {
                Show(false);
            }
        }

        public void ClickEvent()
        {
            actionManager.Invoke(nextActions[CoreGame.Instance.state.story.act]);
        }        

        private void Show(bool flag)
        {
            gameObject.SetActive(flag);
            titleText.text = titleStrings[CoreGame.Instance.state.story.act];
        }   
    }
}