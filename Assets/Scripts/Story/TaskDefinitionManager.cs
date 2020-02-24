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

            public string tasksResource = @"Tasks/";

            public TaskDefinitionManager()
            {
                instance = this;
            }

            public void Initialize()
            {
                LoadTasks();
            }

            public void LoadTasks()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(tasksResource);

                foreach (var file in jsonTextFiles)
                {
                    Logger.Log(this.name, file.name);
                    Logger.Log(this.name, file.text);
                    
                    var tasksHolder = JsonUtility.FromJson<TasksHolder>(file.text);
                    Logger.Log(this.name, "tasks added - " + tasksHolder.tasks.Count);
                    Logger.Log(this.name, tasksHolder.tasks[0].id);

                    taskList.AddRange(tasksHolder.tasks);
                    
                }
            }


            //public Task selectedTask;

            public Task GetTaskDefinition(string id)
            {
                //return null;
                return taskList.FirstOrDefault(t => t.id == id);
            }

            public Task GetTaskInstance(string id)
            {
                var t = GetTaskDefinition(id);

                if (t == null)
                {
                    Logger.LogError(this.name, "task definition not found - " + id);
                    return null;
                }

                Logger.Log(this.name, "get task instance - " + t.id);

                Task instance = new Task
                {
                    id = t.id,
                    title = t.title,
                    completed = t.completed,
                    description = t.description,
                    giver = t.giver,
                    started = t.started
                };

                instance.objectives = new List<TaskObjective>(t.objectives.Count);

                foreach (var obj in t.objectives)
                {
                    instance.objectives.Add(new TaskObjective()
                    {
                        objective = obj.objective,
                        description = obj.description,
                        completed = obj.completed,
                        optional = obj.optional
                    });
                }

                return instance;
            }


            public List<Task> GetTasks()
            {
                return taskList;
            }

            private class TasksHolder
            {
                public List<Task> tasks;
            }
        }
    }
}