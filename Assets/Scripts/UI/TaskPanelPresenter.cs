using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackedDesign.Story;

namespace HackedDesign.UI
{
    public class TaskPanelPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text taskRequesterText;
        [SerializeField] private UnityEngine.UI.Text taskCorpText;
        [SerializeField] private UnityEngine.UI.Text taskTitleText;
        [SerializeField] private UnityEngine.UI.Text taskRewardText;
        [SerializeField] private UnityEngine.UI.Text taskDescription;
        [SerializeField] private UnityEngine.UI.Image RequesterAvatar;
        public GameObject taskButtonParent;
        public GameObject taskButtonPrefab;


        private bool dirty = true;

        SelectMenuManager selectMenuManager;

        public void Initialize(SelectMenuManager selectMenuManager)
        {
            this.selectMenuManager = selectMenuManager;
        }

        public override void Repaint()
        {
            if (selectMenuManager.MenuState == SelectMenuSubState.Tasks)
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
                taskRequesterText.text = "";
                taskTitleText.text = "";
                taskCorpText.text = "";
                taskDescription.text = "";
                taskRewardText.text = "";
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