using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Dialogue {
        public class DialoguePanelPresenter : MonoBehaviour {

            IDialogueManager dialogueManager;
            public Text speaker;
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

            public void Repaint () {
                //FIXME: Don't repaint if nothing's changed
                Dialogue currentDialogue = dialogueManager.GetCurrentDialogue ();

                if(currentDialogue == null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }

                this.gameObject.SetActive(true);

                //Debug.Log(currentDialogue.speaker.corp.color.ToString());

                text.text = currentDialogue.text;
                //speaker.text = currentNarration.speaker.fullName + " / <color=cyan>\"" + currentNarration.speaker.handle + "\"</color> / <color="+ currentNarration.speaker.corp.color.ToString() + ">" + currentNarration.speaker.corp.name + "</color> / " + currentNarration.speaker.serial;
                //avatar.sprite = currentNarration.speaker.avatar;
                //actionButtonImage.sprite = currentDialogue.button;
                //.text = currentNarration.button;

            }    
        }
    }
}