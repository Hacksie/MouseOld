using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class TaskListItem : MonoBehaviour
    {
        public Story.Task task;

        [Header("Reference GameObjects")]
        [SerializeField] private UnityEngine.UI.Text label = null;

        void Awake()
        {
            //text = GetComponent<UnityEngine.UI.Text>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Repaint()
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