using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign {
    
        public class StashPanelPresenter : MonoBehaviour {

            // public Transform infoCategoriesParent;
            // public Transform infoEntitiesParent;
            // public Transform infoDescriptionParent;
            // InfoManager infoManager;
            SelectMenuManager selectMenuManager;

            public void Start () {
                // if (infoCategoriesParent == null) {
                //     Debug.LogError ("No infoCategoriesParent set");
                //     return;
                // }

                // if (infoEntitiesParent == null) {
                //     Debug.LogError ("No infoEntitiesParent set");
                //     return;
                // }

                // if (infoDescriptionParent == null) {
                //     Debug.LogError ("No infoDescriptionParent set");
                //     return;
                // }
            }

            public void Initialize (SelectMenuManager selectMenuManager) {
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint () {
                if (CoreGame.Instance.state.state == GameState.GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.STASH) {
                    if (!this.gameObject.activeInHierarchy) {
                        Show (true);
                    }
                } else if (this.gameObject.activeInHierarchy) {
                    Show (false);
                }

            }

            private void Show (bool flag) {
                Debug.Log ("Set Stash Panel " + flag);

                this.gameObject.SetActive (flag);

                if (!flag) {
                    return;
                }

                RepaintStash ();
            }

            public void RepaintStash () {

                EventSystem.current.SetSelectedGameObject (null);

            }     
        }
    //}
}