using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        public abstract class NarrationAction : ScriptableObject {

            public abstract void Invoke () ;
        }
    }
}