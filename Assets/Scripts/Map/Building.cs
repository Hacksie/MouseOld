using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Building", menuName = "Mouse/Map/Building")]
        public class Building : ScriptableObject {
            public bool available;
            public bool current;
            public bool target;
            public string title;
            public string description;
            public Location[] locations;
            public Vector2 mapPosition;
        }
    }
}