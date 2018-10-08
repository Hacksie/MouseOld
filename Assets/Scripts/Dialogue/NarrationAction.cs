using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

    namespace Dialogue {
        public abstract class NarrationAction : ScriptableObject {

            public abstract void Invoke () ;
            
        }
    }
}