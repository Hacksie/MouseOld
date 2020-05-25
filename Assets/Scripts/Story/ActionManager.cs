using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign.Story
{

    public class ActionManager : MonoBehaviour
    {
        [SerializeField] private float timeOut = 10.0f;

        public static ActionManager Instance { get; private set;}

        public List<ActionMessage> console = new List<ActionMessage>();

        public ILevelActions CurrentStoryActions { get; private set;}

        ActionManager() => Instance = this;

        public void Initialize(Dialogue.NarrationManager narrationManager)
        {
            CurrentStoryActions = new PreludeActions();
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

            var handled = CurrentStoryActions.Invoke(actionName);

            if (!handled)
            {
                Logger.LogWarning(this, "Cannot invoke action: ", actionName, " in current state");
            }
        }
    }

    public class ActionMessage
    {
        public float time;
        public string message;
    }

}
