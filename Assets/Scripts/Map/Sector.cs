using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign {
    namespace Map {
        [CreateAssetMenu (fileName = "Sector", menuName = "Mouse/Map/Sector")]
        public class Sector : ScriptableObject {
            public bool available;
            //public bool current;
            public bool target;
            public string title;
            [TextArea]
            public string description;
            public List<Building> buildings;
            public Vector2 mapPosition;
        }
    }
}