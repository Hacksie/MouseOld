using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class TaskDefinitionRepository : MonoBehaviour
        {
            public List<Task> taskList = new List<Task>();

            public static TaskDefinitionRepository Instance { get; private set; }

            public string tasksResource = @"Tasks/";

            public TaskDefinitionRepository()
            {
                Instance = this;
            }

            public void Start()
            {
                LoadTasks();
            }

            public void LoadTasks()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(tasksResource);

                foreach (var file in jsonTextFiles)
                {
                    
                    var tasksHolder = JsonUtility.FromJson<TasksHolder>(file.text);
                    Logger.Log(name, tasksHolder.tasks.Count.ToString(), " Tasks added");
                    taskList.AddRange(tasksHolder.tasks);                  
                }
            }

            public Task GetTaskDefinition(string id)
            {
                return taskList.FirstOrDefault(t => t.id == id);
            }

            public Task GetTaskInstance(string id)
            {
                var t = GetTaskDefinition(id);

                if (t == null)
                {
                    Logger.LogError(name, id, " task definition not found");
                    return null;
                }

                Logger.Log(name, "get task instance ", t.id);

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
        }
    }
}