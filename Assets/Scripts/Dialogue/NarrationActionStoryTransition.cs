using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        /// <summary>
        /// Performs a story transition on clicking a narration action button
        /// </summary>
        [CreateAssetMenu (fileName = "NarrationActionStoryTransition", menuName = "Mouse/Dialogue/NarrationActionStoryTransition")]
        public class NarrationActionStoryTransition : NarrationAction {
            public Story.StoryEvent currentStoryEvent;
            public Story.StoryEvent nextStoryEvent;

            public override void Invoke () {
                Debug.Log ("Narration Action Invoke");
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