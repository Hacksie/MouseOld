using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackedDesign.Story;

namespace HackedDesign.UI
{
    public class TaskPanelPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text taskRequesterText = null;
        [SerializeField] private UnityEngine.UI.Text taskCorpText = null;
        [SerializeField] private UnityEngine.UI.Text taskTitleText = null;
        [SerializeField] private UnityEngine.UI.Text taskRewardText = null;
        [SerializeField] private UnityEngine.UI.Text taskDescription = null;
        [SerializeField] private UnityEngine.UI.Image requesterAvatar = null;
        [SerializeField] private GameObject taskButtonParent = null;
        [SerializeField] private GameObject taskButtonPrefab = null;


        SelectMenuManager selectMenuManager;

        public void Initialize(SelectMenuManager selectMenuManager)
        {
            this.selectMenuManager = selectMenuManager;
        }

        public override void Repaint()
        {
            RepaintTasks();
        }

        private void RepaintTasks()
        {
            for (int i = 0; i < taskButtonParent.transform.childCount; i++)
            {
                Destroy(taskButtonParent.transform.GetChild(i).gameObject);
            }

            foreach (var task in GameManager.Instance.Data.TaskList)
            {
                var go = Instantiate(taskButtonPrefab, Vector3.zero, Quaternion.identity, taskButtonParent.transform);
                var goTaskItem = go.GetComponent<TaskListItem>();
                goTaskItem.task = task.Value;
                goTaskItem.Repaint();
            }
            RepaintTaskDescription(GameManager.Instance.Data.selectedTask);
        }

        public void RepaintTaskDescription(Task selectedTask)
        {
            if (GameManager.Instance.Data.selectedTask == null)
            {
                taskRequesterText.text = "";
                taskTitleText.text = "";
                taskCorpText.text = "";
                taskDescription.text = "";
                taskRewardText.text = "";
            }
            else
            {
                var requester = Story.InfoRepository.Instance.GetCharacter(selectedTask.giver);
                if(requester != null)
                {
                    requesterAvatar.sprite = requester.avatar;
                    taskRequesterText.text = requester.handle;
                    taskCorpText.text = requester.corp;
                }

                taskRewardText.text = selectedTask.reward;
                taskTitleText.text = selectedTask.title;
                taskDescription.text = selectedTask.description;
                taskDescription.text += "\n";
                if (selectedTask.objectives != null)
                {
                    foreach (TaskObjective objective in selectedTask.objectives)
                    {
                        taskDescription.text += "\n[" + (objective.completed ? "*" : " ") + "] " + objective.objective + (objective.optional ? "(*)" : "");
                        taskDescription.text += "\n<color='#999999'>" + objective.description + "</color>";
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