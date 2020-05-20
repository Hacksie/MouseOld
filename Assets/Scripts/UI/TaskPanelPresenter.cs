using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackedDesign.Story;

namespace HackedDesign.UI
{

        public class TaskPanelPresenter : AbstractPresenter
        {

            public GameObject taskButtonParent;
            public GameObject taskButtonPrefab;
            public UnityEngine.UI.Text taskDescription;

            private bool dirty = true;

            SelectMenuManager selectMenuManager;

            public void Initialize(SelectMenuManager selectMenuManager)
            {
                this.selectMenuManager = selectMenuManager;
            }

            public override void Repaint()
            {
                if (GameManager.Instance.state.state == GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.TASKS)
                {
                    Show();
                    if (dirty)
                    {
                        dirty = false;
                        RepaintTasks();
                    }                    
                }
                else
                {
                    dirty = true;
                    Hide();
                    
                }
            }

            private void RepaintTasks()
            {
                for (int i = 0; i < taskButtonParent.transform.childCount; i++)
                {
                    Destroy(taskButtonParent.transform.GetChild(i).gameObject);
                }

                for (int i = 0; i < GameManager.Instance.state.taskList.Count; i++)
                {

                    var go = Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goTaskItem = go.GetComponent<TaskListItem>();
                    goTaskItem.task = GameManager.Instance.state.taskList[i];
                    goTaskItem.Repaint();
                }

                RepaintTaskDescription(GameManager.Instance.state.selectedTask);
            }

            public void RepaintTaskDescription(Task selectedTask)
            {
                if (GameManager.Instance.state.selectedTask == null)
                {
                    taskDescription.text = "";
                }
                else
                {
                    taskDescription.text = GameManager.Instance.state.selectedTask.description;
                    taskDescription.text += "\n";
                    if (GameManager.Instance.state.selectedTask.objectives != null)
                    {
                        Logger.Log(this, "Objectives count - " + GameManager.Instance.state.selectedTask.objectives.Count);
                        foreach (TaskObjective objective in GameManager.Instance.state.selectedTask.objectives)
                        {
                            taskDescription.text += "\n[" + (objective.completed ? "*" : " ") + "] " + objective.objective + (objective.optional ? "(*)" : "");
                            taskDescription.text += "\n     <color='#999999'>" + objective.description + "</color>";
                        }
                    }
                }
            }

            public void ClickEvent()
            {
                Logger.Log(this, "Clicked");
            }
        }
    
}