using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign.Story
{

    public class ActionManager : MonoBehaviour
    {
        public static ActionManager instance;
        public Entities.EntityManager entityManager;
        public TaskDefinitionManager taskManager;

        private Dictionary<string, ILevelActions> actions = new Dictionary<string, ILevelActions>();

        [SerializeField]
        private float timeOut = 10.0f;

        public Queue<ActionMessage> console = new Queue<ActionMessage>();

        ActionManager()
        {
            instance = this;
        }

        public void Initialize(Entities.EntityManager entityManager, TaskDefinitionManager taskManager)
        {
            this.entityManager = entityManager;
            this.taskManager = taskManager;
            actions.Clear();
            actions.Add("Global", new GlobalActions());
            actions.Add("Bootstrap", new BootstrapActions());
            actions.Add("Olivia's Room", new PreludeActions());
            actions.Add("Arisana Bar", new PreludeBarActions());
        }


        public void AddActionMessage(string message)
        {
            console.Enqueue(new ActionMessage()
            {
                time = Time.time,
                message = message
            });
        }

        public void UpdateBehaviour()
        {
            // Pop items off the console one frame at a time
            if (console.Count > 0)
            {
                if ((Time.time - console.Peek().time) > timeOut)
                {
                    console.Dequeue();
                }
            }
        }

        public void Invoke(string actionName)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            CoreGame.Instance.SaveGame();

            if (CoreGame.Instance.state.currentLevel == null)
            {
                Debug.LogError(this.name + ": cannot invoke an action if no level is loaded");
            }

            bool handled = false;

            if (actions.ContainsKey(CoreGame.Instance.state.currentLevel.template.name))
            {
                handled = actions[CoreGame.Instance.state.currentLevel.template.name].Invoke(actionName);
            }
            if(!handled) {
                handled = actions["Global"].Invoke(actionName);
            }

            if(!handled)
            {
                Debug.LogError(this.name + ": cannot invoke action: " + actionName + " in current level: " + CoreGame.Instance.state.currentLevel.template.name);
            }
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
