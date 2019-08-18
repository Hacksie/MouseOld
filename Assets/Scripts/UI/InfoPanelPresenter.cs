using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Story {
        public class InfoPanelPresenter : MonoBehaviour {

            public Transform infoCategoriesParent;
            public Transform infoEntitiesParent;
            public Transform infoDescriptionParent;
            InfoManager infoManager;
            SelectMenuManager selectMenuManager;

            public void Start () {
                if (infoCategoriesParent == null) {
                    Debug.LogError ("No infoCategoriesParent set");
                    return;
                }

                if (infoEntitiesParent == null) {
                    Debug.LogError ("No infoEntitiesParent set");
                    return;
                }

                if (infoDescriptionParent == null) {
                    Debug.LogError ("No infoDescriptionParent set");
                    return;
                }
            }

            public void Initialize (InfoManager infoManager, SelectMenuManager selectMenuManager) {
                this.infoManager = infoManager;
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint () {
                if (CoreGame.Instance.CoreState.state == GameState.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.INFO) {
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

                RepaintCategories ();
            }

            public void RepaintCategories () {

                EventSystem.current.SetSelectedGameObject (null);

                var categories = infoManager.categories.Where(e => e.available).ToList();

                for (int i = 0; i < infoCategoriesParent.childCount; i++) {
                    Transform cbt = infoCategoriesParent.GetChild (i);
                    Button b = cbt.GetComponent<Button> ();
                    Text t = cbt.GetComponentInChildren<Text> ();
                    //Map.SectorBehaviour sb = sbt.GetComponent<Map.SectorBehaviour> ();

                    if (categories.Count() > i) {
                        cbt.gameObject.SetActive (true); // Just in case it isn't active

                        cbt.name = categories[i].name;

                        // if(currentCategory == infoManager.selectedInfoCategory)
                        // {
                        //     b.Select();
                        // }

                        //sb.sector = currentSector;
                        t.text = categories[i].name;

                    } else {
                        cbt.gameObject.SetActive (false);
                    }

                }

                RepaintEntities ();

            }

            public void SelectCategory () {
                Debug.Log (EventSystem.current.currentSelectedGameObject.name);
                // This is flakey as fuck
                this.infoManager.selectedInfoCategory = EventSystem.current.currentSelectedGameObject.name;
                RepaintEntities ();
                //EventSystem.current.
            }

            public void SelectEntity () {
                Debug.Log (EventSystem.current.currentSelectedGameObject.name);
                // This is flakey as fuck
                this.infoManager.selectedInfoEntity = EventSystem.current.currentSelectedGameObject.name;
                RepaintDescription ();
            }

            public void RepaintEntities () {

                var entities = infoManager.entities.Where (e => e.parentInfoCategory == infoManager.selectedInfoCategory && e.available).ToList ();

                for (int i = 0; i < infoEntitiesParent.childCount; i++) {
                    Transform cbt = infoEntitiesParent.GetChild (i);
                    //Button b = cbt.GetComponent<Button> ();
                    Text t = cbt.GetComponentInChildren<Text> ();

                    if (entities.Count > i) {
                        cbt.gameObject.SetActive (true); // Just in case it isn't active
                        cbt.name = entities[i].name;
                        t.text = entities[i].name;

                    } else {
                        cbt.gameObject.SetActive (false);
                    }

                }

                RepaintDescription ();

            }

            public void RepaintDescription () {
                //Transform cbt = infoDescriptionParent.GetChild (i);
                //Button b = cbt.GetComponent<Button> ();

                Text t = infoDescriptionParent.GetComponentInChildren<Text> ();

                var entity = infoManager.entities.FirstOrDefault (e => e.parentInfoCategory == infoManager.selectedInfoCategory && e.name == infoManager.selectedInfoEntity);

                if (entity != null) {
                    
                    t.text = entity.description;
                } else 
                {
                    t.text = "";

                }

            }
        }
    }
}