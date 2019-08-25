using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class ActionManager : MonoBehaviour
        {
            public static ActionManager instance;

            [SerializeField]
            private float timeOut = 5.0f;

            public Queue<ActionMessage> console = new Queue<ActionMessage>();

            ActionManager()
            {
                instance = this;
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

            public void Invoke(string eventName)
            {
                switch (eventName)
                {
                    case "Prelude1":
                        Debug.Log(this.name + ": invoke Prelude1");
                        InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                        InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Kari"));
                        Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                        break;
                    case "Prelude2":
                        Debug.Log(this.name + ": invoke Prelude2");
                        Dialogue.NarrationManager.instance.ShowNarration("Prelude2");
                        break;
                    case "PreludeLaptop":
                        PreludeLaptop();
                        break;
                    case "PreludeExit":
                        PreludeExit();
                        break;
                }
            }

            public void PreludeLaptop()
            {

                if (!CoreGame.Instance.CoreState.taskList.Exists(t => t.title == "Milk Run"))
                {
                    Story.Task t = (Story.Task)ScriptableObject.CreateInstance(typeof(Story.Task));

                    t.title = "Milk Run";
                    t.description = "Milk Run Description";

                    CoreGame.Instance.CoreState.taskList.Add(t);
                    CoreGame.Instance.CoreState.selectedTask = t;
                }
            }

            public void PreludeExit()
            {
                if (CoreGame.Instance.CoreState.taskList.Exists(t => t.title == "Milk Run"))
                {
                    Debug.Log("Can exit");
                }
                else
                {
                    Debug.Log("Can't exit, haven't received mission");
                }
            }

        }

        public class ActionMessage
        {
            public float time;
            public string message;
        }
    }
}
