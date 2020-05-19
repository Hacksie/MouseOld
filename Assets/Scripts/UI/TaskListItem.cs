using UnityEngine;

namespace HackedDesign.UI
{
    public class TaskListItem : AbstractPresenter
    {
        public Story.Task task;

        [Header("Reference GameObjects")]
        [SerializeField] private UnityEngine.UI.Text label = null;

        public override void Repaint()
        {
            if (task != null)
            {
                label.text = task.title;
            }
            else
            {
                label.text = "<invalid>";
                Logger.LogError(name, "no task set");
            }
        }

        public void Click()
        {
            Logger.Log(name, "Task List Item clicked");
        }
    }
}