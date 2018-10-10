using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        /// <summary>
        /// Performs a story transition on clicking a Dialogue action button
        /// </summary>
        [CreateAssetMenu (fileName = "DialogueActionStoryTransition", menuName = "Mouse/Dialogue/DialogueActionStoryTransition")]
        public class DialogueActionStoryTransition : DialogueAction {
            public Story.StoryEvent currentStoryEvent;
            public Story.StoryEvent nextStoryEvent;

            public override void Invoke () {
                Debug.Log ("Dialogue Action Invoke");
                if (currentStoryEvent != null) {
                    currentStoryEvent.Complete ();
                }

                if (nextStoryEvent != null) {
                    nextStoryEvent.Start ();
                }
            }
        }
    }
}