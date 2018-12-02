using System;
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
			private int seed = 1;

			public LevelGenTemplate[] levelGenTemplates;
			public List<GameObject> securityGuardEasyPrefabs;

			public DynamicNavigation2D navigation2D;

			public void Initialize (GameObject parent) {
				this.parent = parent;
			}

			// Template -> Generate -> GeneratedLevel
			public int GenerateLevel (string name, string template) {
				Debug.Log ("Generating Level");

				if (string.IsNullOrEmpty (template)) {
					Debug.LogError ("No level template set");
					return 0;
				}

				if (string.IsNullOrEmpty (name)) {
					Debug.LogError ("No level name set");
					return 0;
				}

				LevelGenTemplate levelGenTemplate = GetLevelGenTemplate (template);

				if (levelGenTemplate == null) {
					Debug.LogError ("No level gen template found");
					return 0;
				}

				PlaceholderChunk[, ] placeholderLevel = new PlaceholderChunk[levelGenTemplate.levelWidth, levelGenTemplate.levelHeight];
				Debug.Log ("Random seed is " + UnityEngine.Random.seed);

				//UnityEngine.Random.InitState (seed); // Psuedo random seed gives predictable results, so we can save the seed and recreate the level

				// Seems like a sensible limit
				if (levelGenTemplate.levelLength > Mathf.Sqrt (levelGenTemplate.levelLength * levelGenTemplate.levelWidth)) {
					levelGenTemplate.levelLength = (int) Mathf.Sqrt (levelGenTemplate.levelLength * levelGenTemplate.levelWidth);
				}

				if (levelGenTemplate.levelLength < 1) {
					return -1;
				}

				var position = GenerateStartingLocation (placeholderLevel, levelGenTemplate);
				if (levelGenTemplate.levelLength > 1) {

					GenerateMainChain (new Vector2Int (position.x, position.y - 1), position, levelGenTemplate.levelLength - 1, placeholderLevel, levelGenTemplate);
				}

				GenerateAuxRooms (placeholderLevel, levelGenTemplate);

				PopulateLevelTilemap (placeholderLevel, levelGenTemplate);
				
				navigation2D.BakeNavMesh2D();

				//PopulateLevelDoors (placeholderLevel, levelGenTemplate);
				PopulateSecurityGuards (placeholderLevel, levelGenTemplate);
				PrintLevelDebug (placeholderLevel, levelGenTemplate);

				

				

				return seed;

			}

			LevelGenTemplate GetLevelGenTemplate (string template) {
				return levelGenTemplates.FirstOrDefault (t => t.name == template);
			}

			Vector2Int GenerateStartingLocation (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				Debug.Log ("Generating Starting Location");

				// Starting at the bottom and going up means we should never create a chain that fails completely and roles all the way back to the entry
				// This is important!				
				// It also means the player starts at the bottom and plays upwards, which is ideal
				Vector2Int position = new Vector2Int ((levelGenTemplate.levelWidth - 1) / 2, (levelGenTemplate.levelHeight - 1));
				placeholderLevel[position.x, position.y] = GenerateEntryRoomChunk (levelGenTemplate);
				return position;
			}

			bool GenerateMainChain (Vector2Int newLocation, Vector2Int lastLocation, int lengthRemaining, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {

				if (lengthRemaining == 0) {
					return true;
				}

				Debug.Log ("Generating Main Chain");

				// The end room is considered special
				if (lengthRemaining == 1) {
					Debug.Log ("End of main chain");

					placeholderLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Wall }, true, placeholderLevel, levelGenTemplate); // Place a new tile here
					placeholderLevel[newLocation.x, newLocation.y].isEnd = true;
					return true;
				}

				placeholderLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door }, true, placeholderLevel, levelGenTemplate); // Place a new tile here 

				List<Vector2Int> directions = PossibleDirections (newLocation, placeholderLevel, levelGenTemplate);

				directions.Randomize ();

				bool result = false;

				// Iterate over potential directions from here
				for (int i = 0; i < directions.Count; i++) {

					result = GenerateMainChain (directions[i], newLocation, lengthRemaining - 1, placeholderLevel, levelGenTemplate);

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

			public void PopulateLevelTilemap (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				for (int k = 0; k < parent.transform.childCount; k++) {
					GameObject.Destroy (parent.transform.GetChild (k).gameObject);
				}

				for (int i = 0; i < levelGenTemplate.levelHeight; i++) {
					for (int j = 0; j < levelGenTemplate.levelWidth; j++) {
						Vector3 pos = new Vector3 (j * 4, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4), 0);

						if (placeholderLevel[j, i] != null) {

							if (levelGenTemplate.floor != null) {
								GameObject.Instantiate (levelGenTemplate.floor, pos, Quaternion.identity, parent.transform);
							}

							List<GameObject> goList = FindChunkObject (placeholderLevel[j, i], levelGenTemplate).ToList ();

							goList.Randomize ();

							if (goList.FirstOrDefault () != null) {

								GameObject.Instantiate (goList[0], pos, Quaternion.identity, parent.transform);
								GameObject.Instantiate (roomCentrePrefab, pos + new Vector3 (2, 2, 0), Quaternion.identity, parent.transform);

							}
						}
						// else {
						// 	GameObject gw = FindChunkObject (ChunkFromString ("wwww"), levelGenTemplate).FirstOrDefault();
						// 	if (gw != null) {
						// 		GameObject.Instantiate (gw, pos, Quaternion.identity, parent.transform);
						// 	}
						// }
					}
				}
			}

			public void PopulateLevelDoors (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				if (!levelGenTemplate.generateDoors) {
					Debug.Log ("Skipping doors");
					return;
				}

				for (int i = 0; i < levelGenTemplate.levelHeight; i++) {
					for (int j = 0; j < levelGenTemplate.levelWidth; j++) {
						PlaceholderChunk placeholder = placeholderLevel[j, i];

						if (placeholder != null) {
							if (placeholder.top == Chunk.ChunkSide.Door) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (doorewPrefab, pos, Quaternion.identity, parent.transform);
							}

							if (placeholder.left == Chunk.ChunkSide.Door) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (doornsPrefab, pos, Quaternion.identity, parent.transform);
							}

						}
					}
				}
			}

			public void PopulateSecurityGuards (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				List<Vector3> spawnLocationList = new List<Vector3> ();

				for (int i = 0; i < levelGenTemplate.levelHeight; i++) {
					for (int j = 0; j < levelGenTemplate.levelWidth; j++) {
						if (placeholderLevel[j, i] != null && !placeholderLevel[j, i].isNearEntry) {
							Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 2, 0);
							spawnLocationList.Add (pos);
						}
					}
				}

				spawnLocationList.Randomize ();

				if (securityGuardEasyPrefabs.Count <= 0) {
					return;
				}

				for (int i = 0; i < spawnLocationList.Count; i++) {
					if (i >= levelGenTemplate.securityGuards) {
						break;
					}

					securityGuardEasyPrefabs.Randomize ();
					GameObject sggo = securityGuardEasyPrefabs[0];
					var go = GameObject.Instantiate (sggo, spawnLocationList[i], Quaternion.identity, parent.transform);

					NavMeshAgent2D navMeshAgent = go.GetComponent<NavMeshAgent2D>();
					navMeshAgent.SetDestination(new Vector2(5,5));
				}
			}

			IEnumerable<GameObject> FindChunkObject (PlaceholderChunk chunk, LevelGenTemplate levelGenTemplate) {
				return levelGenTemplate.levelElements.Where (g => g != null && ChunkMatchesString (chunk, g.name));
			}

			bool ChunkMatchesString (PlaceholderChunk chunk, string str) {

				var goChunk = ChunkFromString (str);
				if (chunk.isEntry && goChunk.isEntry == chunk.isEntry) {
					return true;
				}

				return (
					goChunk.isEntry == chunk.isEntry &&
					goChunk.top == chunk.top &&
					goChunk.bottom == chunk.bottom &&
					goChunk.left == chunk.left &&
					goChunk.right == chunk.right
				);
			}

			PlaceholderChunk ChunkFromString (string str) {
				PlaceholderChunk response = new PlaceholderChunk ();
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

			void GenerateAuxRooms (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				Debug.Log ("Generating Aux Rooms");
				bool newRooms = true;

				// iterate through every position, checking for neighbours and creating rooms accordingly. 
				// Keep iterating until we stop creating rooms				
				while (newRooms) {
					newRooms = false;
					for (int i = 0; i < levelGenTemplate.levelHeight; i++) {
						for (int j = 0; j < levelGenTemplate.levelWidth; j++) {
							if ((placeholderLevel[j, i] != null)) {
								Vector2Int pos = new Vector2Int (j, i);
								List<Vector2Int> dirs = PossibleDirections (pos, placeholderLevel, levelGenTemplate);

								foreach (Vector2Int location in dirs) {
									newRooms = true;
									placeholderLevel[location.x, location.y] = GenerateRoom (location, new List<Chunk.ChunkSide> () {
										Chunk.ChunkSide.Open, Chunk.ChunkSide.Door, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall
									}, false, placeholderLevel, levelGenTemplate);
								}
							}
						}
					}
				}
			}

			PlaceholderChunk GenerateRoom (Vector2Int location, List<Chunk.ChunkSide> freeChoiceSides, bool isMainChain, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				// Get Top Side
				List<Chunk.ChunkSide> tops = PossibleTopSides (location, freeChoiceSides, placeholderLevel, levelGenTemplate);
				List<Chunk.ChunkSide> lefts = PossibleLeftSides (location, freeChoiceSides, placeholderLevel, levelGenTemplate);
				List<Chunk.ChunkSide> bottoms = PossibleBottomSides (location, freeChoiceSides, placeholderLevel, levelGenTemplate);
				List<Chunk.ChunkSide> rights = PossibleRightSides (location, freeChoiceSides, placeholderLevel, levelGenTemplate);

				tops.Randomize ();
				lefts.Randomize ();
				bottoms.Randomize ();
				rights.Randomize ();

				return new PlaceholderChunk () {
					isEntry = false,
						isEnd = false,
						isMainChain = isMainChain,
						top = tops[0],
						left = lefts[0],
						bottom = bottoms[0],
						right = rights[0],
						isNearEntry = IsNearEntry (location, placeholderLevel, levelGenTemplate)
				};

			}

			bool IsNearEntry (Vector2Int location, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
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

					if (surround[i].x >= levelGenTemplate.levelWidth)
						continue;

					if (surround[i].y >= levelGenTemplate.levelHeight)
						continue;

					// Check for a chunk
					if (placeholderLevel[surround[i].x, surround[i].y] == null)
						continue;

					if (placeholderLevel[surround[i].x, surround[i].y].isEntry)
						return true;

				}

				return false;

			}

			PlaceholderChunk GenerateEntryRoomChunk (LevelGenTemplate levelGenTemplate) {

				string start = string.IsNullOrEmpty (levelGenTemplate.startingChunkString) ? "wdww_entry" : levelGenTemplate.startingChunkString;

				//FIXME: Make this more robust
				if (!start.Contains ("_entry")) {
					start += "_entry";
				}

				PlaceholderChunk res = ChunkFromString (start);
				res.isMainChain = true;
				res.isNearEntry = true;

				return res;
			}

			List<Chunk.ChunkSide> PossibleTopSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y <= 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the top
				sides.Add (chunk.bottom);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleBottomSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y >= (levelGenTemplate.levelHeight - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the bottom
				sides.Add (chunk.top);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleLeftSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x <= 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the left
				sides.Add (chunk.right);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleRightSides (Vector2Int position, List<Chunk.ChunkSide> freeChoice, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (position.x >= (levelGenTemplate.levelWidth - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[position.x + 1, position.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the right
				sides.Add (chunk.left);
				return sides;
			}

			List<Vector2Int> PossibleDirections (Vector2Int pos, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y];

				List<Vector2Int> results = new List<Vector2Int> ();

				if (chunk.left == Chunk.ChunkSide.Door || chunk.left == Chunk.ChunkSide.Open) {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasChunk (leftPos, placeholderLevel, levelGenTemplate)) {
						results.Add (leftPos);
					}
				}

				if (chunk.top == Chunk.ChunkSide.Door || chunk.top == Chunk.ChunkSide.Open) {
					var upPos = new Vector2Int (pos.x, pos.y - 1);
					if (!PositionHasChunk (upPos, placeholderLevel, levelGenTemplate)) {
						results.Add (upPos);
					}
				}

				if (chunk.bottom == Chunk.ChunkSide.Door || chunk.bottom == Chunk.ChunkSide.Open) {
					var bottomPos = new Vector2Int (pos.x, pos.y + 1);
					if (!PositionHasChunk (bottomPos, placeholderLevel, levelGenTemplate)) {
						results.Add (bottomPos);
					}
				}

				if (chunk.right == Chunk.ChunkSide.Door || chunk.right == Chunk.ChunkSide.Open) {
					var rightPos = new Vector2Int (pos.x + 1, pos.y);
					if (!PositionHasChunk (rightPos, placeholderLevel, levelGenTemplate)) {
						results.Add (rightPos);
					}
				}

				return results;
			}

			bool PositionHasChunk (Vector2Int pos, PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				if (pos.x >= levelGenTemplate.levelWidth || pos.y >= levelGenTemplate.levelHeight || pos.x < 0 || pos.y < 0) {
					return true; // If we go outside the level, pretend we already put a chunk here
				}
				return (!(placeholderLevel[pos.x, pos.y] == null));
			}

			void PrintLevelDebug (PlaceholderChunk[, ] placeholderLevel, LevelGenTemplate levelGenTemplate) {
				Debug.Log ("Printing level");
				for (int i = 0; i < levelGenTemplate.levelHeight; i++) {
					string line = "";
					for (int j = 0; j < levelGenTemplate.levelWidth; j++) {
						if (placeholderLevel[j, i] != null) {

							if (placeholderLevel[j, i].isEntry) {
								line += "[" + placeholderLevel[j, i].AsPrintableString () + "]";

							} else if (placeholderLevel[j, i].isEnd) {
								line += "{" + placeholderLevel[j, i].AsPrintableString () + "}";

							} else if (placeholderLevel[j, i].isMainChain) {
								line += "<" + placeholderLevel[j, i].AsPrintableString () + ">";
							} else {

								line += "(" + placeholderLevel[j, i].AsPrintableString () + ")";
							}
						} else {
							line += "-####-";
						}
					}

					Debug.Log (line);
				}
			}
		}
	}
}