using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class TaskDefinitionManager : MonoBehaviour
        {
            public List<Task> taskList = new List<Task>();

            public static TaskDefinitionManager instance;

            public TaskDefinitionManager()
            {
                instance = this;
            }

            //public Task selectedTask;

            public Task GetTaskDefinition(string name)
            {
                return taskList.FirstOrDefault(t => t.name == name);
            }

            public Task GetTaskInstance(string name)
            {
                var t = GetTaskDefinition(name);

                if (!t)
                    return null;

                Story.Task instance = (Story.Task)ScriptableObject.CreateInstance(typeof(Story.Task));
                instance.title = t.title;
                instance.description = t.description;
                instance.completed = t.completed;
                return instance;
            }


            public List<Task> GetTasks()
            {
                return taskList;
            }
        }
    }
}