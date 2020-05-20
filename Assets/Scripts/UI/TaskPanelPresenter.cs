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
                if (GameManager.Instance.GameState.PlayState == PlayStateEnum.SelectMenu && selectMenuManager.MenuState == SelectMenuState.Tasks)
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

                foreach (var task in GameManager.Instance.GameState.TaskList)
                {

                    var go = Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                    var goTaskItem = go.GetComponent<TaskListItem>();
                    goTaskItem.task = task;
                    goTaskItem.Repaint();
                }

                RepaintTaskDescription(GameManager.Instance.GameState.selectedTask);
            }

            public void RepaintTaskDescription(Task selectedTask)
            {
                if (GameManager.Instance.GameState.selectedTask == null)
                {
                    taskDescription.text = "";
                }
                else
                {
                    taskDescription.text = GameManager.Instance.GameState.selectedTask.description;
                    taskDescription.text += "\n";
                    if (GameManager.Instance.GameState.selectedTask.objectives != null)
                    {
                        Logger.Log(this, "Objectives count - " + GameManager.Instance.GameState.selectedTask.objectives.Count);
                        foreach (TaskObjective objective in GameManager.Instance.GameState.selectedTask.objectives)
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