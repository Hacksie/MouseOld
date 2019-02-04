using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class TaskPanelPresenter : MonoBehaviour {

            TaskManager taskManager;

            public void Initialize (TaskManager taskManager) {
                this.taskManager = taskManager;
            }

            public void Show(bool flag)
            {
                Debug.Log("Set task panel " + flag);
                this.gameObject.SetActive(flag);
            }
        }
    }
}