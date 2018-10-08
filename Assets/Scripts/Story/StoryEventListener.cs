using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign {
    namespace Story {
        public class StoryEventListener : MonoBehaviour {
            public StoryEvent storyEvent;
            public UnityEvent startedAction;
            public UnityEvent completedAction;

            private void OnEnable () {
                storyEvent.RegisterListener (this);
            }

            private void OnDisable () {
                storyEvent.UnregisterListener (this);
            }

            public void OnEventTrigger (StoryEvent.StoryEventState state) {
                switch (state) {
                    case StoryEvent.StoryEventState.STARTED:
                        if (startedAction != null) {
                            startedAction.Invoke ();
                        }
                        break;
                    case StoryEvent.StoryEventState.COMPLETED:
                        if (startedAction != null) {
                            completedAction.Invoke ();
                        }
                        break;
                }
            }
        }
    }
}