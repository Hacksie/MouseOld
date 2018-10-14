using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Sector", menuName = "Mouse/Map/Sector")]
        public class Sector : ScriptableObject {
            public bool available;
            public bool current;
            public bool target;
            public string title;
            public string description;
            public Building[] buildings;
            public Vector2 mapPosition;
        }
    }
}