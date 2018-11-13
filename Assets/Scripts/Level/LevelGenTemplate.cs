using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign
{
    namespace Level
    {
        public class LevelGenTemplate : ScriptableObject
        {
			public int levelLength = 7;
			public int levelWidth = 10;
			public int levelHeight = 10;

			public string levelName;
			public string floorName;

			public List<Floor> floors;
			public List<LevelElements> levelElements;                        
        }
    }
}