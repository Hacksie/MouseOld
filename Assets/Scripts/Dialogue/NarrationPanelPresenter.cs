using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Dialogue {
        public class NarrationPanelPresenter : MonoBehaviour {

            INarrationManager narrationManager;
            public Text text; 
            public Button actionButton;
            public Image actionButtonImage;
            public Text speaker;
            //public Image avatar;

            //public List<Character.Corp> corps = new List<Character.Corp>();

            
            public void Initialize (INarrationManager narrationManager) {
                this.narrationManager = narrationManager;

                if(text == null) Debug.LogError("Text is null");
                if(actionButton == null) Debug.LogError("Button is null");
                if(actionButtonImage == null) Debug.LogError("Button sprite is null");
            }  

            public void Repaint () {
                //FIXME: Don't repaint if nothing's changed
                Narration currentNarration = narrationManager.GetCurrentNarration ();

                

                if(currentNarration == null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }

                this.gameObject.SetActive(true);


                //Debug.Log(currentDialogue.speaker.corp.color.ToString());
                speaker.text = currentNarration.speaker.fullName + " / \"" + currentNarration.speaker.handle + "\"";

                text.text = currentNarration.text;
                //speaker.text = currentNarration.speaker.fullName + " / <color=cyan>\"" + currentNarration.speaker.handle + "\"</color> / <color="+ currentNarration.speaker.corp.color.ToString() + ">" + currentNarration.speaker.corp.name + "</color> / " + currentNarration.speaker.serial;
                //avatar.sprite = currentNarration.speaker.avatar;
                actionButtonImage.sprite = currentNarration.button;
                EventSystem.current.SetSelectedGameObject(actionButton.gameObject);
                //.text = currentNarration.button;

            }    

            
        }
    }
}