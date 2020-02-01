using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign.Story
{

    public class ActionManager : MonoBehaviour
    {
        public static ActionManager instance;
        public Entity.EntityManager entityManager;
        public TaskDefinitionManager taskManager;

        private Dictionary<string, ILevelActions> actions = new Dictionary<string, ILevelActions>();



        [SerializeField]
        private float timeOut = 5.0f;

        public Queue<ActionMessage> console = new Queue<ActionMessage>();

        ActionManager()
        {
            instance = this;
        }

        public void Initialize(Entity.EntityManager entityManager, TaskDefinitionManager taskManager)
        {
            this.entityManager = entityManager;
            this.taskManager = taskManager;

            actions.Add("Victoria's Room", new PreludeActions());
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

        //Twas the night before christmas, when all through the city, all the creatures were stirring. Even a Mouse... God please kill that alarm. I'm getting too old for this. If I had my way, I'd leave the creatures to their stirring and go back to sleep. No chance for that tonight.

        //Good morning Miss Ives, it's currently 2:03am and I've just started a pot of coffee for you. It's currently raining outside and there is a message waiting for you on your terminal. 

        public void Invoke(string actionName)
        {
            if(string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }
            
            CoreGame.Instance.SaveGame();
            
            if (CoreGame.Instance.State.currentLevel == null)
            {
                Debug.LogError(this.name + ": cannot invoke an action if no level is loaded");
            }

            if (actions.ContainsKey(CoreGame.Instance.State.currentLevel.template.name))
            {
                actions[CoreGame.Instance.State.currentLevel.template.name].Invoke(actionName);
            }
            else {
                Debug.LogError(this.name + ": cannot invoke action: " + actionName + " in current level: " + CoreGame.Instance.State.currentLevel.template.name);
            }
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
