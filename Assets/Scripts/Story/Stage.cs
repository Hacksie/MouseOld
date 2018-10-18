using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Story {
        [CreateAssetMenu (fileName = "Stage", menuName = "Mouse/Story/Stage")]
        public class Stage : StoryEvent { 
            [TextArea]
            public string description;
        }
    }
}