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
            public Text speakerHandle;
            public Text speakerCorp;
            public Text speakerStatus;
            //public Image avatar;

            //public List<Character.Corp> corps = new List<Character.Corp>();

            
            public void Initialize (INarrationManager narrationManager) {
                this.narrationManager = narrationManager;

                if(text == null) Debug.LogError("Text is null");
                if(actionButton == null) Debug.LogError("Button is null");
                if(actionButtonImage == null) Debug.LogError("Button sprite is null");
            }  

            public void Show(bool flag) {
                Debug.Log("Show dialogue " + flag);
                Narration currentNarration = narrationManager.GetCurrentNarration ();              

                if(currentNarration == null)
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
                speaker.text = currentNarration.speaker.fullName;
                speakerHandle.text = currentNarration.speaker.handle;
                speakerCorp.text = currentNarration.speaker.corp.name;
                speakerCorp.color = currentNarration.speaker.corp.color;
                speakerStatus.text = currentNarration.speaker.status.ToString();

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