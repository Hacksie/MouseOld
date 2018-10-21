using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Location", menuName = "Mouse/Map/Location")]
        public class Location : ScriptableObject {
            public string title;
            [TextArea]
            public string description;
            public string scene;
            //public bool current;
            public bool available;
        }
    }
}