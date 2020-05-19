using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign.Story
{

    public class ActionManager : MonoBehaviour
    {
        public static ActionManager instance;
        public Entities.EntityManager entityManager;

        private Dictionary<string, ILevelActions> actions = new Dictionary<string, ILevelActions>();

        [SerializeField]
        private float timeOut = 10.0f;

        public List<ActionMessage> console = new List<ActionMessage>();

        ActionManager()
        {
            instance = this;
        }

        public void Initialize(Entities.EntityManager entityManager)
        {
            this.entityManager = entityManager;
            actions.Clear();
            actions.Add("Global", new GlobalActions());
            actions.Add("Bootstrap", new BootstrapActions());
            actions.Add("Olivia's Room", new PreludeActions());
            actions.Add("Arisana Bar", new PreludeBarActions());
        }


        public void AddActionMessage(string message)
        {
            console.Add(new ActionMessage()
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
                if ((Time.time - console[0].time) > timeOut)
                {
                    console.RemoveAt(0);
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
                Logger.LogError(this,"Cannot invoke an action if no level is loaded");
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
                Logger.LogError(this, "Cannot invoke action: ", actionName, " in current level: ", CoreGame.Instance.state.currentLevel.template.name);
            }
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
