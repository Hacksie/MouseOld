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
			public int securityGuards = 0;

			public bool generateDoors = true;

			public string startingChunkString = "wdww";

			public string levelNameTemplate;

			public bool isRandom;

			public GameObject floor;
			//public LevelElements levelElements;   
			public List<GameObject> levelElements;     
			public List<GameObject> endProps;
			public List<GameObject> startProps;
			public List<GameObject> randomProps;
			                
        }
    }
}