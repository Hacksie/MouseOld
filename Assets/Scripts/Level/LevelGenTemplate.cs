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
			public int enemies = 0;
			public int traps = 0;

			public bool generateDoors = true;

			public string startingRoomString = "weww";
			public string startingAction;
			public string exitAction;

			public bool isRandom;
			public bool generateNavMesh = true;

			public GameObject floor;
			//public LevelElements levelElements;   
			public List<GameObject> levelElements;     
			public List<GameObject> endProps;
			public List<GameObject> startProps;
			public List<GameObject> randomProps;
			public List<GameObject> trapProps;

			public List<string> mapWallsRows;
			public List<string> mapPropsRows;
			                
        }
    }
}