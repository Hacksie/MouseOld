using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        [CreateAssetMenu (fileName = "NarrationActionStoryTransition", menuName = "Mouse/Dialogue/NarrationActionStoryTransition")]
        public class NarrationActionStoryTransition : NarrationAction {
            public Story.StoryEvent storyTransition;

            public override void Invoke () {
                Debug.Log ("Narration Action Invoke");
                if (storyTransition != null) {
                    storyTransition.Complete ();
                }
                //storyEvent.Trigger(storyEventState);
            }
        }
    }
}