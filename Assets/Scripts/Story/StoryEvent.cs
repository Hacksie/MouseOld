using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class StoryEvent : ScriptableObject {

            public StoryEventState currentState = StoryEventState.UNSTARTED;
            //public StoryEvent nextStoryEvent;

            private List<StoryEventListener> listeners = new List<StoryEventListener> ();

            public void RegisterListener (StoryEventListener listener) {
                if (!listeners.Contains (listener)) {
                    listeners.Add (listener);
                } else {
                    Debug.LogWarning ("Listener already registered");
                }
            }

            public void UnregisterListener (StoryEventListener listener) {
                if (listeners.Contains (listener)) {
                    listeners.Remove (listener);
                }
            }

            public void Start () {
                currentState = StoryEventState.STARTED;

                for (int i = 0; i < listeners.Count; i++) {
                    Debug.Log ("Triggering story start " + listeners[i].name);
                    listeners[i].OnEventTrigger (currentState);
                }
            }

            public void Complete () {
                currentState = StoryEventState.COMPLETED;

                for (int i = 0; i < listeners.Count; i++) {
                    Debug.Log ("Triggering story complete " + listeners[i].name);
                    listeners[i].OnEventTrigger (currentState);
                }         
                       
                // if (nextStoryEvent != null) {
                //     nextStoryEvent.Start ();
                // }
            }

            // public void TriggerState (StoryEventState eventType) {
            //     StoryEventState currentState = eventType;

            //     if (listeners.ContainsKey (eventType)) {
            //         for (int i = 0; i < listeners[eventType].Count; i++) {
            //             Debug.Log("Triggering story start "+ listeners[eventType][i].name + " " + eventType.ToString());
            //             listeners[eventType][i].OnEventTrigger ();
            //         }
            //     }
            // }

            public enum StoryEventState {
                UNSTARTED,
                STARTED,
                COMPLETED
            }
        }
    }
}