using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Building", menuName = "Mouse/Map/Building")]
        public class Building : ScriptableObject {
            public bool available;
            //public bool current;
            public bool target;
            public string title;
            [TextArea]
            public string description;
            public List<Location> locations;
            public Vector2 mapPosition;
        }
    }
}