using UnityEngine;

namespace HackedDesign {
    namespace Story {
        public class StoryEventTransition : MonoBehaviour {

            public Story.StoryEvent currentStoryEvent;
            public Story.StoryEvent nextStoryEvent;

			public void Invoke ()
			{
				//Debug.Log ("Invoking  story trigger " + currentStoryEvent.name + " -> " + nextStoryEvent.name );
                if (currentStoryEvent != null && currentStoryEvent.currentState == StoryEvent.StoryEventState.STARTED) {
                    currentStoryEvent.Complete ();
                }

                if (nextStoryEvent != null) {
                    nextStoryEvent.Start ();
                }
			}
        }
    }
}