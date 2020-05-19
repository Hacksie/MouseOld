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
                if (CoreGame.Instance.state.state == GameState.GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.TASKS)
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

                for (int i = 0; i < CoreGame.Instance.state.taskList.Count; i++)
                {

                    var go = Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goTaskItem = go.GetComponent<TaskListItem>();
                    goTaskItem.task = CoreGame.Instance.state.taskList[i];
                    goTaskItem.Repaint();
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
                        Logger.Log(this, "Objectives count - " + CoreGame.Instance.state.selectedTask.objectives.Count);
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
                Logger.Log(this, "Clicked");
            }
        }
    
}