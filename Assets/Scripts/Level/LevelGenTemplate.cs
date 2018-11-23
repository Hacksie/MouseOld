using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HackedDesign
{
    namespace Level
    {
		[CreateAssetMenu (fileName = "LevelTemplate", menuName = "Mouse/Level/Level Template")]
        public class LevelGenTemplate : ScriptableObject
        {
			public int levelLength = 7;
			public int levelWidth = 10;
			public int levelHeight = 10;

			public bool generateDoors = true;

			public string levelNameTemplate;

			public Floor floor;
			public LevelElements levelElements;                        
        }
    }
}