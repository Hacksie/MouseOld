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
                if (CoreGame.Instance.state.state == GameState.GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.TASKS)
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
                    Destroy(taskButtonParent.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < CoreGame.Instance.state.taskList.Count; i++)
                {

                    var go = Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goTaskItem = go.GetComponent<TaskListItem>();
                    goTaskItem.task = CoreGame.Instance.state.taskList[i];
                    goTaskItem.Repaint();
                    //var goText = go.GetComponentInChildren<UnityEngine.UI.Text>();
                    //goText.text = CoreGame.Instance.state.taskList[i].title;
                }

                RepaintTaskDescription(CoreGame.Instance.state.selectedTask);
            }

            public void RepaintTaskDescription(Task selectedTask)
            {

                if (CoreGame.Instance.state.selectedTask == null)
                {
                    taskDescription.text = "";
                }
                else
                {
                    taskDescription.text = CoreGame.Instance.state.selectedTask.description;
                    taskDescription.text += "\n";
                    if (CoreGame.Instance.state.selectedTask.objectives != null)
                    {
                        Logger.Log(this.name, "objectives count - " + CoreGame.Instance.state.selectedTask.objectives.Count);
                        foreach (TaskObjective objective in CoreGame.Instance.state.selectedTask.objectives)
                        {
                            taskDescription.text += "\n[" + (objective.completed ? "*" : " ") + "] " + objective.objective + (objective.optional ? "(*)" : "");
                            taskDescription.text += "\n     <color='#999999'>" + objective.description + "</color>";
                        }
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