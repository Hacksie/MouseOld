using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Entity
    {
        public class EntityManager : MonoBehaviour
        {
            [Header("Prefabs")]
            public List<GameObject> enemies;
            public List<GameObject> traps;  
            public List<GameObject> npcs;          
        }
    }
}