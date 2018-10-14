using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Location", menuName = "Mouse/Map/Location")]
        public class Location : ScriptableObject {
            public string title;
            public string description;
            public Scene scene;
        }
    }
}