using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Dialogue {
        public class NarrationPanelPresenter : MonoBehaviour {

            public Narration currentNarration;
            INarrationManager narrationManager;
            public Text text;
            public Button actionButton;
            public Image actionButtonImage;
            public Text speaker;
            public Text speakerHandle;
            public Text speakerCorp;
            public Text speakerStatus;
            public Image avatarSprite;
            //public Image avatar;

            //public List<Character.Corp> corps = new List<Character.Corp>();

            public void Initialize (INarrationManager narrationManager) {
                this.narrationManager = narrationManager;

                if (text == null) Debug.LogError ("Text is null");
                if (actionButton == null) Debug.LogError ("Button is null");
                if (actionButtonImage == null) Debug.LogError ("Button sprite is null");
            }

            public void Repaint () {
                if (CoreGame.Instance.CoreState.state == GameState.NARRATION) {
                    if (!this.gameObject.activeInHierarchy || currentNarration != narrationManager.GetCurrentNarration ()) {
                        Show (true);
                    }
                } else if (this.gameObject.activeInHierarchy) {
                    Show (false);
                }

            }

            private void Show (bool flag) {
                Debug.Log (this.name + ": show narration " + flag);
                currentNarration = narrationManager.GetCurrentNarration ();

                if (currentNarration == null) {
                    this.gameObject.SetActive (false);
                    return;
                }

                this.gameObject.SetActive (flag);

                if (!flag) {
                    return;
                }

                speaker.text = currentNarration.speaker.fullName;
                speakerHandle.text = currentNarration.speaker.handle;
                speakerCorp.text = currentNarration.speaker.corp.name;
                speakerCorp.color = currentNarration.speaker.corp.color;
                speakerStatus.text = currentNarration.speaker.@class.ToString ();
                if (currentNarration.speaker.avatar != null) {
                    avatarSprite.sprite = currentNarration.speaker.avatar;
                }

                text.text = currentNarration.text;
                //speaker.text = currentNarration.speaker.fullName + " / <color=cyan>\"" + currentNarration.speaker.handle + "\"</color> / <color="+ currentNarration.speaker.corp.color.ToString() + ">" + currentNarration.speaker.corp.name + "</color> / " + currentNarration.speaker.serial;
                //avatar.sprite = currentNarration.speaker.avatar;
                actionButtonImage.sprite = currentNarration.button;
                EventSystem.current.SetSelectedGameObject (actionButton.gameObject);
                //.text = currentNarration.button;

            }
        }
    }
}