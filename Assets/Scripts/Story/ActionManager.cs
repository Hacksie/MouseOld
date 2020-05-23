using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign.Story
{

    public class ActionManager : MonoBehaviour
    {
        public static ActionManager instance;

        private Dictionary<string, ILevelActions> actions = new Dictionary<string, ILevelActions>();

        [SerializeField]
        private float timeOut = 10.0f;

        public List<ActionMessage> console = new List<ActionMessage>();

        ActionManager() => instance = this;

        public void Initialize(Dialogue.NarrationManager narrationManager)
        {
            actions.Clear();
            actions.Add("Global", new GlobalActions());
            actions.Add("Bootstrap", new BootstrapActions(narrationManager));
            actions.Add("Olivia's Room", new PreludeActions());
            actions.Add("Arisana Bar", new PreludeBarActions());
        }

        public void AddActionMessage(string message) => console.Add(new ActionMessage()
        {
            time = Time.time,
            message = message
        });

        public void UpdateBehaviour()
        {
            if (console.Count > 0 && (Time.time - console[0].time) > timeOut)
            {
                console.RemoveAt(0);
            }
        }

        public void Invoke(string actionName)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            GameManager.Instance.SaveGame();

            if (GameManager.Instance.GameState.CurrentLevel == null)
            {
                Logger.LogError(this, "Cannot invoke an action if no level is loaded");
            }

            bool handled = false;

            if (actions.ContainsKey(GameManager.Instance.GameState.CurrentLevel.template.name))
            {
                handled = actions[GameManager.Instance.GameState.CurrentLevel.template.name].Invoke(actionName);
            }
            if (!handled)
            {
                handled = actions["Global"].Invoke(actionName);
            }

            if (!handled)
            {
                Logger.LogError(this, "Cannot invoke action: ", actionName, " in current level: ", GameManager.Instance.GameState.CurrentLevel.template.name);
            }
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
