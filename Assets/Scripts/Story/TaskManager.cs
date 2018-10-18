using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class TaskManager : MonoBehaviour {
            public List<Task> taskList = new List<Task>();

            public Task selectedTask;

            public List<Task> GetTasks()
            {
                return taskList;
            }
        }
    }
}