using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class TaskRepository : MonoBehaviour
        {
            public List<Task> taskList = new List<Task>();

            public static TaskRepository Instance { get; private set; }

            public string tasksResource = @"Tasks/";

            public TaskRepository()
            {
                Instance = this;
            }

            public void Start()
            {
                LoadTaskDefinitions();
            }

            public void LoadTaskDefinitions()
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

            public Task GetTaskInstanceFromDefinition(string id)
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


            public List<Task> GetTaskDefinitions()
            {
                return taskList;
            }

            public void AddTask(string id)
            {
                if (!GameManager.Instance.Data.TaskList.ContainsKey(id))
                {
                    var task = GetTaskInstanceFromDefinition(id);
                    GameManager.Instance.Data.TaskList.Add(id, task);
                }

            }

            public bool HasTask(string id)
            {
                return GameManager.Instance.Data.TaskList.ContainsKey(id);
            }

            public Task GetTask(string id)
            {
                if (HasTask(id))
                {
                    return GameManager.Instance.Data.TaskList[id];
                }   
                return null;             
            }

            public void SelectCurrentTask(string id)
            {
                GameManager.Instance.Data.selectedTask = GameManager.Instance.Data.TaskList.FirstOrDefault(t => t.Value.id == id).Value;
            }

            public void CompleteTaskObjective(string id, string objectiveid)
            {
                Logger.Log(this, "Complete task", id, objectiveid);
                var task = GameManager.Instance.Data.TaskList[id];

                var objective = task.objectives.FirstOrDefault(o => o.objective == objectiveid);
                objective.completed = true;
            }


        }
    }
}