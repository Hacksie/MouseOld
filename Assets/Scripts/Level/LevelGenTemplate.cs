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
			public string corp;
			public string building;
			public string level;

			public int levelLength = 7;
			public int levelWidth = 10;
			public int levelHeight = 10;
			public int enemies = 0;
			public int traps = 0;

			public bool generateDoors = true;

			public string startingRoomString = "wnww_entry";
			public string startingAction;
			public string exitAction;

			public string levelResource;

			public bool generateNavMesh = true;

			public bool hostile = true;

			public List<GameObject> floors;
			public List<GameObject> levelElements;     
			public List<GameObject> endProps;
			public List<GameObject> startProps;
			public List<GameObject> randomProps;
			public List<GameObject> trapProps;
			public List<GameObject> fixedProps;

			public List<string> easyEnemies;
			public List<string> mediumEnemies;
			public List<string> hardEnemies;
		                
        }
    }
}