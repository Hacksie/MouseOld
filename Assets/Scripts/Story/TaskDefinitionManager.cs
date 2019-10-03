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

                Task instance = (Task)ScriptableObject.CreateInstance(typeof(Task));
                instance.title = t.title;
                instance.description = t.description;
                instance.completed = t.completed;
                instance.objectives = new List<TaskObjective>();
                foreach(TaskObjective objective in t.objectives)
                {
                    TaskObjective o = (TaskObjective)ScriptableObject.CreateInstance(typeof(TaskObjective));
                    o.objective = objective.objective;
                    o.completed = objective.completed;
                    o.optional = objective.optional;
                    instance.objectives.Add(o);
                }
                return instance;
            }


            public List<Task> GetTasks()
            {
                return taskList;
            }
        }
    }
}