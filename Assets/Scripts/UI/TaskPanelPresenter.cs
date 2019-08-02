using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class TaskPanelPresenter : MonoBehaviour {

            TaskManager taskManager;
            SelectMenuManager selectMenuManager;

            public void Initialize (TaskManager taskManager, SelectMenuManager selectMenuManager) {
                this.taskManager = taskManager;
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint () {
                if (CoreGame.instance.state.state == GameState.SELECTMENU && selectMenuManager.state == SelectMenuManager.SelectMenuState.TASKS) {
                    if (!this.gameObject.activeInHierarchy) {
                        Show (true);
                    }
                } else if(this.gameObject.activeInHierarchy) {
                    Show (false);
                }

            }

            private void Show (bool flag) {
                Debug.Log ("Set task panel " + flag);
                this.gameObject.SetActive (flag);
            }
        }
    }
}