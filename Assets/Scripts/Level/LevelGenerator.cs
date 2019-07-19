﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelGenerator : MonoBehaviour {

			private GameObject parent;
			public GameObject doorewPrefab;
			public GameObject doornsPrefab;
			public GameObject roomCentrePrefab;
			public GameObject npcParent;
			public GameObject spawnPrefab;

			public LevelGenTemplate[] levelGenTemplates;
			public List<GameObject> securityGuardEasyPrefabs;

			public DynamicNavigation2D navigation2D;

			public void Initialize (GameObject parent) {
				this.parent = parent;
			}

	
			// Template -> Generate -> GeneratedLevel
			public Level GenerateRandomLevel (string name, string template, int seed) {
				Debug.Log ("Generating Level");

				if (string.IsNullOrEmpty (template)) {
					Debug.LogError ("No level template set");
					return null;
				}

				if (string.IsNullOrEmpty (name)) {
					Debug.LogError ("No level name set");
					return null;
				}

				var genTemplate = GetLevelGenTemplate (template);

				if (genTemplate == null) {
					Debug.LogError ("No level gen template found");
					return null;
				}

				var level = new Level (genTemplate);
				var position = GenerateStartingLocation (level);

				if (level.length > 1) {
					GenerateMainChain (new Vector2Int (position.x, position.y - 1), position, level, level.length - 1);
				}

				GenerateAuxRooms (level);

				PopulateLevelTilemap (level);

				navigation2D.BakeNavMesh2D ();

				//PopulateLevelDoors (level);
				PopulateSecurityGuards (level);
				level.Print();

				return level;

			}

			LevelGenTemplate GetLevelGenTemplate (string template) {
				var genTemplate = levelGenTemplates.FirstOrDefault (t => t.name == template);

				return genTemplate;
			}

			Vector2Int GenerateStartingLocation (Level level) {
				Debug.Log ("Generating Starting Location");

				// Starting at the bottom and going up means we should never create a chain that fails completely and rolls all the way back to the entry
				// This is important!				
				// It also means the player starts at the bottom and plays upwards, which is ideal
				Vector2Int position = new Vector2Int ((level.template.levelWidth - 1) / 2, (level.template.levelHeight - 1));
				level.proxyLevel[position.x, position.y] = GenerateEntryRoomChunk (level);
				level.spawn = position;
				return position;
			}

			bool GenerateMainChain (Vector2Int newLocation, Vector2Int lastLocation, Level level, int lengthRemaining) {
				if (lengthRemaining == 0) {
					return true;
				}

				Debug.Log ("Generating Main Chain");

				// The end room is considered special
				if (lengthRemaining == 1) {
					Debug.Log ("End of main chain");

					level.proxyLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Wall }, true, level); // Place a new tile here
					level.proxyLevel[newLocation.x, newLocation.y].isEnd = true;
					return true;
				}

				level.proxyLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door }, true, level); // Place a new tile here 

				List<Vector2Int> directions = PossibleBuildDirections (newLocation, level);

				directions.Randomize ();

				bool result = false;

				// Iterate over potential directions from here
				for (int i = 0; i < directions.Count; i++) {

					result = GenerateMainChain (directions[i], newLocation, level, lengthRemaining - 1);

					if (result) // If the chain is okay, don't need to try any new directions. We could probably return true here
					{
						break;
					}
				}

				// If we didn't complete the chain, abandon this location
				// Fixme: we probably have to change a side because of this
				if (!result) {
					Debug.Log ("Abandoning chain, roll back one step");
				}

				return result;
			}

			List<Vector2Int> PossibleBuildDirections (Vector2Int pos, Level level) {
				ProxyChunk chunk = level.proxyLevel[pos.x, pos.y];

				List<Vector2Int> results = new List<Vector2Int> ();

				if (chunk.left == Chunk.ChunkSide.Door || chunk.left == Chunk.ChunkSide.Open) {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasChunk (leftPos, level)) {
						results.Add (leftPos);
					}
				}

				if (chunk.top == Chunk.ChunkSide.Door || chunk.top == Chunk.ChunkSide.Open) {
					var upPos = new Vector2Int (pos.x, pos.y - 1);
					if (!PositionHasChunk (upPos, level)) {
						results.Add (upPos);
					}
				}

				if (chunk.bottom == Chunk.ChunkSide.Door || chunk.bottom == Chunk.ChunkSide.Open) {
					var bottomPos = new Vector2Int (pos.x, pos.y + 1);
					if (!PositionHasChunk (bottomPos, level)) {
						results.Add (bottomPos);
					}
				}

				if (chunk.right == Chunk.ChunkSide.Door || chunk.right == Chunk.ChunkSide.Open) {
					var rightPos = new Vector2Int (pos.x + 1, pos.y);
					if (!PositionHasChunk (rightPos, level)) {
						results.Add (rightPos);
					}
				}

				return results;
			}

			ProxyChunk GenerateEntryRoomChunk (Level level) {

				string start = string.IsNullOrEmpty (level.template.startingChunkString) ? "wdww_entry" : level.template.startingChunkString;

				//FIXME: Make this more robust
				if (!start.Contains ("_entry")) {
					start += "_entry";
				}

				ProxyChunk res = ChunkFromString (start);
				res.isMainChain = true;
				res.isNearEntry = true;

				return res;
			}

			public void PopulateLevelTilemap (Level level) {
				for (int k = 0; k < parent.transform.childCount; k++) {
					GameObject.Destroy (parent.transform.GetChild (k).gameObject);
				}

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

						if (level.proxyLevel[j, i] != null) {

							if (level.template.floor != null) {
								GameObject.Instantiate (level.template.floor, pos, Quaternion.identity, parent.transform);
							}

							PopulateRoom(level.proxyLevel[j, i], pos, parent.transform, level.template);

							// List<GameObject> goList = FindChunkObject (level.proxyLevel[j, i], level.template).ToList ();

							// goList.Randomize ();

							// if (goList.FirstOrDefault () != null) {

							// 	GameObject.Instantiate (goList[0], pos, Quaternion.identity, parent.transform);
							// 	GameObject.Instantiate (roomCentrePrefab, pos + new Vector3 (2, 2, 0), Quaternion.identity, parent.transform);

							// }
						}
					}
				}
			}

			public void PopulateRoom(ProxyChunk proxyChunk, Vector3 pos, Transform parent, LevelGenTemplate template)
			{
				string chunkString = proxyChunk.AsPrintableString();
				
				// TL
				List<GameObject> goTLList = FindChunkObject("TL", chunkString.Substring(0,1), chunkString.Substring(1,1), template).ToList();
				
				goTLList.Randomize ();

				if (goTLList.FirstOrDefault () != null) {
					GameObject.Instantiate (goTLList[0], pos, Quaternion.identity, parent.transform);
				}				

				// TR

				List<GameObject> goTRList = FindChunkObject("TR", chunkString.Substring(3,1), chunkString.Substring(1,1), template).ToList();
				goTRList.Randomize ();
				

				if (goTRList.FirstOrDefault () != null) {
					GameObject.Instantiate (goTRList[0], pos, Quaternion.identity, parent.transform);
				}					

				// BL

				List<GameObject> goBList = FindChunkObject("BL", chunkString.Substring(0,1), chunkString.Substring(2,1), template).ToList();
				goBList.Randomize ();

				if (goBList.FirstOrDefault () != null) {
					GameObject.Instantiate (goBList[0], pos, Quaternion.identity, parent.transform);
				}				

				// BR

				List<GameObject> goBRist = FindChunkObject("BR", chunkString.Substring(3,1), chunkString.Substring(2,1), template).ToList();
				goBRist.Randomize ();

				if (goBRist.FirstOrDefault () != null) {
					GameObject.Instantiate (goBRist[0], pos, Quaternion.identity, parent.transform);
				}
			}

			IEnumerable<GameObject> FindChunkObject (string corner, string wall1, string wall2, LevelGenTemplate levelGenTemplate) {
				return levelGenTemplate.levelElements.Where (g => g != null && g.name.Substring(0,2).ToUpper() == corner.ToUpper() && g.name.Substring(3,1).ToUpper() == wall1.ToUpper() && g.name.Substring(4,1).ToUpper() == wall2.ToUpper());
			}			

			public void PopulateLevelDoors (Level level) {
				if (!level.template.generateDoors) {
					Debug.Log ("Skipping doors");
					return;
				}

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						ProxyChunk placeholder = level.proxyLevel[j, i];

						if (placeholder != null) {
							if (placeholder.top == Chunk.ChunkSide.Door) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (doorewPrefab, pos, Quaternion.identity, parent.transform);
							}

							if (placeholder.left == Chunk.ChunkSide.Door) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (doornsPrefab, pos, Quaternion.identity, parent.transform);
							}

						}
					}
				}
			}

			public void PopulateSecurityGuards (Level level) {
				List<Vector2Int> spawnLocationList = new List<Vector2Int> ();

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						if (level.proxyLevel[j, i] != null && !level.proxyLevel[j, i].isNearEntry) {
							//Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 2, 0);
							spawnLocationList.Add (new Vector2Int (j, i));
						}
					}
				}

				spawnLocationList.Randomize ();

				if (securityGuardEasyPrefabs.Count <= 0) {
					return;
				}

				for (int i = 0; i < spawnLocationList.Count; i++) {
					if (i >= level.template.securityGuards) {
						break;
					}

					securityGuardEasyPrefabs.Randomize ();
					GameObject sggo = securityGuardEasyPrefabs[0];
					var go = GameObject.Instantiate (sggo, level.ConvertLevelPosToWorld (spawnLocationList[i]), Quaternion.identity, npcParent.transform);

					//NavMeshAgent2D navMeshAgent = go.GetComponent<NavMeshAgent2D> ();
					// BaseNPCController npcController = go.GetComponent<BaseNPCController> ();

					// var relativeList = ConstructRandomPatrolPath (spawnLocationList[i], npcController.patrolPathLength, level.placeholderLevel);
					// npcController.patrolPath = relativeList.ConvertAll<Vector3> (e => ConvertLevelPosToWorld (e, level.template));
				}
			}

			// IEnumerable<GameObject> FindChunkObject (ProxyChunk chunk, LevelGenTemplate levelGenTemplate) {
			// 	return levelGenTemplate.levelElements.Where (g => g != null && ChunkMatchesString (chunk, g.name));
			// }

			// bool ChunkMatchesString (ProxyChunk chunk, string str) {

			// 	var goChunk = ChunkFromString (str);
			// 	if (chunk.isEntry && goChunk.isEntry == chunk.isEntry) {
			// 		return true;
			// 	}

			// 	return (
			// 		goChunk.isEntry == chunk.isEntry &&
			// 		goChunk.top == chunk.top &&
			// 		goChunk.bottom == chunk.bottom &&
			// 		goChunk.left == chunk.left &&
			// 		goChunk.right == chunk.right
			// 	);
			// }

			ProxyChunk ChunkFromString (string str) {
				ProxyChunk response = new ProxyChunk ();
				string[] splitString = str.Split ('_');

				if (splitString.Length < 1) {
					return null;
				}

				if (splitString.Length > 0) {

					response.left = SideFromChar (splitString[0][0]);
					response.top = SideFromChar (splitString[0][1]);
					response.bottom = SideFromChar (splitString[0][2]);
					response.right = SideFromChar (splitString[0][3]);
				}

				if (splitString.Length > 1) {
					response.isEntry = splitString[1] == "entry";
					response.isEnd = splitString[1] == "exit";
				}

				return response;
			}

			Chunk.ChunkSide SideFromChar (char ch) {
				switch (ch) {
					case 'w':
						return Chunk.ChunkSide.Wall;
					case 'd':
						return Chunk.ChunkSide.Door;
					case 'o':
						return Chunk.ChunkSide.Open;
				}
				return Chunk.ChunkSide.Wall;
			}

			void GenerateAuxRooms (Level level) {
				Debug.Log ("Generating Aux Rooms");
				bool newRooms = true;

				// iterate through every position, checking for neighbours and creating rooms accordingly. 
				// Keep iterating until we stop creating rooms				
				while (newRooms) {
					newRooms = false;
					for (int i = 0; i < level.template.levelHeight; i++) {
						for (int j = 0; j < level.template.levelWidth; j++) {
							if ((level.proxyLevel[j, i] != null)) {
								Vector2Int pos = new Vector2Int (j, i);
								List<Vector2Int> dirs = PossibleBuildDirections (pos, level);

								foreach (Vector2Int location in dirs) {
									newRooms = true;
									level.proxyLevel[location.x, location.y] = GenerateRoom (location, new List<Chunk.ChunkSide> () {
										Chunk.ChunkSide.Open, Chunk.ChunkSide.Door, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall
									}, false, level);
								}
							}
						}
					}
				}
			}

			ProxyChunk GenerateRoom (Vector2Int location, List<Chunk.ChunkSide> freeChoiceSides, bool isMainChain, Level level) 
			{
				// Get Top Side
				List<Chunk.ChunkSide> tops = PossibleTopSides (location, freeChoiceSides, level);
				List<Chunk.ChunkSide> lefts = PossibleLeftSides (location, freeChoiceSides, level);
				List<Chunk.ChunkSide> bottoms = PossibleBottomSides (location, freeChoiceSides, level);
				List<Chunk.ChunkSide> rights = PossibleRightSides (location, freeChoiceSides, level);

				tops.Randomize ();
				lefts.Randomize ();
				bottoms.Randomize ();
				rights.Randomize ();

				return new ProxyChunk () {
					isEntry = false,
						isEnd = false,
						isMainChain = isMainChain,
						top = tops[0],
						left = lefts[0],
						bottom = bottoms[0],
						right = rights[0],
						isNearEntry = IsNearEntry (location, level)
				};

			}

			bool IsNearEntry (Vector2Int location, Level level) {
				Vector2Int[] surround = new Vector2Int[9];
				surround[0] = location + new Vector2Int (-1, -1);
				surround[1] = location + new Vector2Int (0, -1);
				surround[2] = location + new Vector2Int (1, -1);
				surround[3] = location + new Vector2Int (-1, 0);
				surround[4] = location + new Vector2Int (0, 0);
				surround[5] = location + new Vector2Int (1, 0);
				surround[6] = location + new Vector2Int (-1, 1);
				surround[7] = location + new Vector2Int (0, 1);
				surround[8] = location + new Vector2Int (1, 1);

				for (int i = 0; i < 9; i++) {

					// Check bounds
					if (surround[i].x < 0)
						continue;

					if (surround[i].y < 0)
						continue;

					if (surround[i].x >= level.template.levelWidth)
						continue;

					if (surround[i].y >= level.template.levelHeight)
						continue;

					// Check for a chunk
					if (level.proxyLevel[surround[i].x, surround[i].y] == null)
						continue;

					if (level.proxyLevel[surround[i].x, surround[i].y].isEntry)
						return true;

				}

				return false;

			}

			List<Chunk.ChunkSide> PossibleTopSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, Level level) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y <= 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyChunk chunk = level.proxyLevel[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the top
				sides.Add (chunk.bottom);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleBottomSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, Level level) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y >= (level.template.levelHeight - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyChunk chunk = level.proxyLevel[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the bottom
				sides.Add (chunk.top);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleLeftSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, Level level) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x <= 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyChunk chunk = level.proxyLevel[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the left
				sides.Add (chunk.right);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleRightSides (Vector2Int position, List<Chunk.ChunkSide> freeChoice, Level level) 
			{
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (position.x >= (level.template.levelWidth - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyChunk chunk = level.proxyLevel[position.x + 1, position.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the right
				sides.Add (chunk.left);
				return sides;
			}

			bool PositionHasChunk (Vector2Int pos, Level level) {
				if (pos.x >= level.template.levelWidth || pos.y >= level.template.levelHeight || pos.x < 0 || pos.y < 0) {
					return true; // If we go outside the level, pretend we already put a chunk here
				}
				return (!(level.proxyLevel[pos.x, pos.y] == null));
			}
		}
	}
}