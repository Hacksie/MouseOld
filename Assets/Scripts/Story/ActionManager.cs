using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class ActionManager : MonoBehaviour {
            public static ActionManager instance;

            ActionManager () {
                instance = this;
            }

            //Twas the night before christmas, when all through the city, all the creatures were stirring. Even a Mouse... God please kill that alarm. I'm getting too old for this. If I had my way, I'd leave the creatures to their stirring and go back to sleep. No chance for that tonight.

            //Good morning Miss Ives, it's currently 2:03am and I've just started a pot of coffee for you. It's currently raining outside and there is a message waiting for you on your terminal. 

            public void Invoke (string eventName) {
                switch (eventName) {
                    case "Prelude1":
                        Debug.Log ("Invoke Prelude1");
                        Dialogue.NarrationManager.instance.ShowNarration ("Prelude1");
                        break;
                    case "Prelude2":
                        Debug.Log ("Invoke Prelude2");
                        Dialogue.NarrationManager.instance.ShowNarration ("Prelude2");
                        break;
                    case "PreludeLaptop":
                        PreludeLaptop ();
                        break;
                    case "PreludeExit":
                        PreludeExit();
                        break;
                }
            }

            public void PreludeLaptop () {

                if (!CoreGame.instance.state.taskList.Exists (t => t.title == "Milk Run")) {
                    Story.Task t = (Story.Task)ScriptableObject.CreateInstance(typeof(Story.Task));
                    
                    t.title = "Milk Run";
                    t.description = "Milk Run Description";

                    CoreGame.instance.state.taskList.Add (t);
                    CoreGame.instance.state.selectedTask = t;
                }
            }

            public void PreludeExit () {
                if (CoreGame.instance.state.taskList.Exists (t => t.title == "Milk Run"))
                {
                    Debug.Log ("Can exit");
                }
                    else 
                    {
                        Debug.Log ("Can't exit, haven't received mission");
                    }
            }

        }
    }
}
