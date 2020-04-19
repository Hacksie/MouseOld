using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign
{
    namespace Dialogue
    {
        public class NarrationPanelPresenter : MonoBehaviour
        {

            public Narration currentNarration;

            public Text text;
            public Button actionButton;
            //public Image actionButtonImage;
            public Text handleText;
            public Text shortNameText;
            public Text categoryText;
            public Text corpText;
            public Image avatarSprite;
            /*
            public Image avatarHairSprite;
            public Image avatarEyesSprite;
            public Image avatarPantsSprite;
            public Image avatarShirtSprite;
            public Image avatarShoesSprite;*/

            private Story.InfoManager infoManager;
            private INarrationManager narrationManager;


            public void Initialize(INarrationManager narrationManager, Story.InfoManager info)
            {
                this.narrationManager = narrationManager;
                this.infoManager = info;

                if (text == null) Debug.LogError("Text is null");
                if (actionButton == null) Debug.LogError("Button is null");
            }

            public void Repaint()
            {
                if (CoreGame.Instance.state.state == GameState.GameStateEnum.NARRATION)
                {
                    if (!this.gameObject.activeInHierarchy || currentNarration != narrationManager.GetCurrentNarration())
                    {
                        Show(true);
                    }
                }
                else if (this.gameObject.activeInHierarchy)
                {
                    Show(false);
                }

            }

            private void Show(bool flag)
            {
                Debug.Log(this.name + ": show narration " + flag);
                currentNarration = narrationManager.GetCurrentNarration();

                if (currentNarration == null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }

                this.gameObject.SetActive(flag);

                if (!flag)
                {
                    return;
                }

                var speaker = infoManager.GetCharacter(currentNarration.speaker);
                var corp = infoManager.GetCorp(speaker.corp);
                handleText.text = speaker.handle;
                shortNameText.text = speaker.name;

                switch (currentNarration.speakerEmotion)
                {
                    case "tired":
                        avatarSprite.sprite = speaker.avatarTired;
                        break;
                    case "thinking":
                        avatarSprite.sprite = speaker.avatarThinking;
                        break;
                    case "happy":
                        avatarSprite.sprite = speaker.avatarHappy;
                        break;
                    case "angry":
                        avatarSprite.sprite = speaker.avatarAngry;
                        break;
                    default:
                        avatarSprite.sprite = speaker.avatar;
                        break;
                }

                corpText.text = "<color=\"" + corp.color + "\">" + corp.name + "</color>";
                text.text = currentNarration.text;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(actionButton.gameObject);
            }
        }
    }
}