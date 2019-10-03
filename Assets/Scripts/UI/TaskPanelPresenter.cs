using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class TaskPanelPresenter : MonoBehaviour
        {

            public GameObject taskButtonParent;
            public GameObject taskButtonPrefab;
            public UnityEngine.UI.Text taskDescription;

            SelectMenuManager selectMenuManager;

            public void Initialize(SelectMenuManager selectMenuManager)
            {
                this.selectMenuManager = selectMenuManager;
            }

            public void Repaint()
            {
                if (CoreGame.Instance.State.state == GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.TASKS)
                {
                    if (!this.gameObject.activeInHierarchy)
                    {
                        RepaintTasks();
                        Show(true);
                    }
                }
                else if (this.gameObject.activeInHierarchy)
                {
                    Show(false);
                }
            }

            private void Show(bool flag)
            {
                Debug.Log(this.name + ": set task panel " + flag);
                this.gameObject.SetActive(flag);
            }

            private void RepaintTasks()
            {
                for (int i = 0; i < taskButtonParent.transform.childCount; i++)
                {
                    GameObject.Destroy(taskButtonParent.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < CoreGame.Instance.State.taskList.Count; i++)
                {

                    var go = GameObject.Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goText = go.GetComponentInChildren<UnityEngine.UI.Text>();
                    goText.text = CoreGame.Instance.State.taskList[i].title;
                }

                RepaintTaskDescription(CoreGame.Instance.State.selectedTask);
            }

            public void RepaintTaskDescription(Task selectedTask)
            {

                if (CoreGame.Instance.State.selectedTask == null)
                {
                    taskDescription.text = "";

                }
                else
                {
                    taskDescription.text = CoreGame.Instance.State.selectedTask.description;
                    taskDescription.text += "\n";
                    foreach(TaskObjective objective in CoreGame.Instance.State.selectedTask.objectives)
                    {
                        taskDescription.text += "\n[" + (objective.completed ? "*" : " ") + "] " + objective.objective + (objective.optional ? "(*)" : "") ;
                    }
                }
            }

            public void ClickEvent()
            {
                Debug.Log(this.name + ": clicked");
            }
        }
    }
}