using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class TaskPanelPresenter : MonoBehaviour {

            public GameObject taskButtonParent;
            public GameObject taskButtonPrefab;
            public UnityEngine.UI.Text taskDescription;
            TaskManager taskManager;
            SelectMenuManager selectMenuManager;

            public void Initialize (TaskManager taskManager, SelectMenuManager selectMenuManager) {
                this.taskManager = taskManager;
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint () {
                if (CoreGame.Instance.CoreState.state == GameState.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.TASKS) {
                    if (!this.gameObject.activeInHierarchy) {
                        RepaintTasks();
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

            private void RepaintTasks()
            {
                for (int i = 0; i < taskButtonParent.transform.childCount; i++) {
                    GameObject.Destroy (taskButtonParent.transform.GetChild (i).gameObject);
                }
                for(int i = 0; i< taskManager.taskList.Count; i++)
                {              
                    
                    var go = GameObject.Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goText = go.GetComponentInChildren<UnityEngine.UI.Text>();
                    goText.text = taskManager.taskList[i].title;
                 }
            }

            public void RepaintTaskDescription(Task selectedTask)
            {
                if(taskManager.selectedTask == null)
                {
                    taskDescription.text = "";

                }
                else
                {
                    taskDescription.text = taskManager.selectedTask.description;
                }
            }

            public void ClickEvent()
            {
                Debug.Log(this.name + ": clicked");
            }
        }
    }
}