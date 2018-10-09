using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        public abstract class DialogueAction : ScriptableObject {

            public abstract void Invoke () ;
        }
    }
}