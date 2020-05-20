using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class TitlecardPresenter : AbstractPresenter
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

        public override void Repaint()
        {
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.Titlecard)
            {
                Show();
                titleText.text = titleStrings[GameManager.Instance.GameState.Story.act];
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            }
            else
            {
                Hide();
            }
        }

        public void ClickEvent()
        {
            actionManager.Invoke(nextActions[GameManager.Instance.GameState.Story.act]);
        }
    }
}