using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Dialogue {
        public class DialoguePanelPresenter : MonoBehaviour {

            IDialogueManager dialogueManager;
            public Text speaker;
            public Text speakerHandle;
            public Text speakerCorp;
            public Text speakerStatus;
            public Text text; 
            public Button response1Button;
            public Button response2Button;
            public Button response3Button;
            public Button response4Button;
            public Text response1ButtonText;
            public Text response2ButtonText;
            public Text response3ButtonText;
            public Text response4ButtonText;            
            
            //public Text speaker;
            //public Image avatar;

            //public List<Character.Corp> corps = new List<Character.Corp>();

            
            public void Initialize (IDialogueManager dialogueManager) {
                this.dialogueManager = dialogueManager;
            }  

            public void Repaint()
            {
                if (CoreGame.instance.state.state == GameState.DIALOGUE && !this.gameObject.activeInHierarchy) {
                    Show (true);
                } else {
                    Show (false);
                }                
            }

            private void Show(bool flag) {
                Debug.Log("Show dialogue " + flag);
                Dialogue currentDialogue = dialogueManager.GetCurrentDialogue ();

                if(currentDialogue == null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }

                this.gameObject.SetActive(flag);

                if(!flag)
                {
                    return;
                }


                //Debug.Log(currentDialogue.speaker.corp.color.ToString());

                text.text = currentDialogue.text;
                speaker.text = currentDialogue.speaker.fullName;
                speakerHandle.text = currentDialogue.speaker.handle;
                speakerCorp.text = currentDialogue.speaker.corp.name;
                speakerCorp.color = currentDialogue.speaker.corp.color;
                speakerStatus.text = currentDialogue.speaker.status.ToString();                

                EventSystem.current.SetSelectedGameObject(null);
                if(currentDialogue.button1text != "")
                {
                    response1Button.gameObject.SetActive(true);
                    response1ButtonText.text = currentDialogue.button1text;
                    EventSystem.current.SetSelectedGameObject(response1Button.gameObject);
                }
                else
                {
                    response1Button.gameObject.SetActive(false);
                }

                if(currentDialogue.button2text != "")
                {
                    response2Button.gameObject.SetActive(true);
                    response2ButtonText.text = currentDialogue.button2text;
                }
                else
                {
                    response2Button.gameObject.SetActive(false);
                }

                if(currentDialogue.button3text != "")
                {
                    response3Button.gameObject.SetActive(true);
                    response3ButtonText.text = currentDialogue.button3text;
                }
                else
                {
                    response3Button.gameObject.SetActive(false);
                }

                if(currentDialogue.button4text != "")
                {
                    response4Button.gameObject.SetActive(true);
                    response4ButtonText.text = currentDialogue.button4text;
                }
                else
                {
                    response4Button.gameObject.SetActive(false);
                }


                //avatar.sprite = currentNarration.speaker.avatar;
                //actionButtonImage.sprite = currentDialogue.button;
                //.text = currentNarration.button;

            }    
        }
    }
}