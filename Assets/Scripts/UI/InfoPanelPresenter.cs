using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Story {
        public class InfoPanelPresenter : MonoBehaviour {

            public Transform categoriesParent;
            InfoManager infoManager;
            SelectMenuManager selectMenuManager;

            public void Initialize (InfoManager infoManager, SelectMenuManager selectMenuManager) {
                this.infoManager = infoManager;
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint () {
                if (CoreGame.instance.state.state == GameState.SELECTMENU && selectMenuManager.state == SelectMenuManager.SelectMenuState.INFO) {
                    if (!this.gameObject.activeInHierarchy) {
                        Show (true);
                    }
                } else if (this.gameObject.activeInHierarchy) {
                    Show (false);
                }

            }

            private void Show (bool flag) {
                Debug.Log ("Set Info Panel " + flag);

                this.gameObject.SetActive (flag);

                if (!flag) {
                    return;
                }

                RepaintCategories (infoManager.GetCategories ());
            }

            public void RepaintCategories (List<InfoCategory> categories) {
                if (categoriesParent == null) {
                    Debug.LogWarning ("No categories parent set");
                    return;
                }

                for (int i = 0; i < categoriesParent.childCount; i++) {
                    Transform cbt = categoriesParent.GetChild (i);
                    //Button b = cbt.GetComponent<Button> ();
                    Text t = cbt.GetComponentInChildren<Text> ();
                    //Map.SectorBehaviour sb = sbt.GetComponent<Map.SectorBehaviour> ();

                    if (categories.Count > i) {
                        cbt.gameObject.SetActive (true); // Just in case it isn't active
                        InfoCategory currentCategory = categories[i];

                        //sb.sector = currentSector;
                        t.text = currentCategory.name;

                    } else {
                        cbt.gameObject.SetActive (false);
                    }

                }

            }
        }
    }
}