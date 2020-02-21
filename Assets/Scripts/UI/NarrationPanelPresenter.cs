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
            public Image avatarBodySprite;
            public Image avatarHairSprite;
            public Image avatarEyesSprite;
            public Image avatarPantsSprite;
            public Image avatarShirtSprite;
            public Image avatarShoesSprite;

            private Story.InfoManager infoManager;
            private INarrationManager narrationManager;
            private CharacterSpriteManager characterSpriteManager;


            public void Initialize(INarrationManager narrationManager, Story.InfoManager info, CharacterSpriteManager characterSpriteManager)
            {
                this.narrationManager = narrationManager;
                this.infoManager = info;
                this.characterSpriteManager = characterSpriteManager;

                if (text == null) Debug.LogError("Text is null");
                if (actionButton == null) Debug.LogError("Button is null");
            }

            public void Repaint()
            {
                if (CoreGame.Instance.state.state == State.GameStateEnum.NARRATION)
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
                categoryText.text = speaker.category;
                corpText.text = "<color=\"" + corp.color + "\">" + corp.name + "</color>";
                //CharacterSpriteManager.BodyTypes body = characterSpriteManager.GetBody(speaker.id);
                var bodySprites = characterSpriteManager.GetSkin(speaker.id);
                var hairSprites = characterSpriteManager.GetHair(speaker.id);
                var eyesSprites = characterSpriteManager.GetEyes(speaker.id);
                var shirtSprites = characterSpriteManager.GetShirt(speaker.id);
                var pantsSprites = characterSpriteManager.GetPants(speaker.id);
                var shoesSprites = characterSpriteManager.GetShoes(speaker.id);
                var hairColor = characterSpriteManager.GetHairColor(speaker.id);
                var shirtColor = characterSpriteManager.GetShirtColor(speaker.id);
                var pantsColor = characterSpriteManager.GetPantsColor(speaker.id);
                var shoesColor = characterSpriteManager.GetShoesColor(speaker.id);

                avatarBodySprite.gameObject.SetActive(bodySprites != null);
                avatarHairSprite.gameObject.SetActive(hairSprites != null);
                avatarEyesSprite.gameObject.SetActive(eyesSprites != null);
                avatarShirtSprite.gameObject.SetActive(shirtSprites != null);
                avatarPantsSprite.gameObject.SetActive(pantsSprites != null);
                avatarShoesSprite.gameObject.SetActive(shoesSprites != null);

                var offset = characterSpriteManager.GetSpriteOffset(speaker.id);


                avatarBodySprite.sprite = bodySprites != null ? bodySprites[offset] : null;
                avatarHairSprite.sprite = hairSprites != null ? hairSprites[offset] : null;
                avatarEyesSprite.sprite = eyesSprites != null ? eyesSprites[offset] : null;
                avatarShirtSprite.sprite = shirtSprites != null ? shirtSprites[offset] : null;
                avatarPantsSprite.sprite = pantsSprites != null ? pantsSprites[offset] : null;
                avatarShoesSprite.sprite = shoesSprites != null ? shoesSprites[offset] : null;

                avatarHairSprite.color = hairColor != null ? hairColor : Color.magenta;
                avatarShirtSprite.color = shirtColor != null ? shirtColor : Color.magenta;
                avatarPantsSprite.color = pantsColor != null ? pantsColor : Color.magenta;
                avatarShoesSprite.color = shoesColor != null ? shoesColor : Color.magenta;


                text.text = currentNarration.text;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(actionButton.gameObject);
            }
        }
    }
}