using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelGenerator : MonoBehaviour {

			const string DEFAULT_ROOM_START = "wdww_entry";

			const string TOPLEFT = "tl";
			const string TOPRIGHT = "tr";
			const string BOTTOMLEFT = "bl";
			const string BOTTOMRIGHT = "br";

			public LevelGenTemplate[] levelGenTemplates;

			public Level GenerateLevel (string template) {
				return GenerateLevel (template, 0, 0, 0, 0, 0, 0);
			}

			public Level GenerateLevel (string template, int length, int height, int width, int difficulty, int enemies, int cameras) {
				Debug.Log ("Generating Level");

				if (string.IsNullOrEmpty (template)) {
					Debug.LogError ("No level template set");
					return null;
				}

				if (string.IsNullOrEmpty (name)) {
					Debug.LogError ("No level name set");
					return null;
				}

				Debug.Log (template);

				var genTemplate = GetLevelGenTemplate (template);

				if (length > 0) {
					genTemplate.levelLength = length;
				}

				if (height > 0) {
					genTemplate.levelHeight = height;
				}

				if (width > 0) {
					genTemplate.levelWidth = width;
				}

				genTemplate.enemies = enemies;
				genTemplate.traps = cameras;

				if (genTemplate == null) {
					Debug.LogError ("No level gen template found: " + template);
					return null;
				}

				Level level;

				if (genTemplate.isRandom) {
					//int seed = UnityEngine.Random.seed;
					level = GenerateRandomLevel (genTemplate);
				} else {
					level = GenerateFixedLevel (genTemplate);
				}

				
				GenerateEntities (level);
				GenerateEnemySpawns (level);
				GenerateTrapSpawns(level);

				level.Print ();

				return level;

			}

			protected Level GenerateFixedLevel (LevelGenTemplate genTemplate) {
				var level = new Level (genTemplate);
				for (int y = 0; y < genTemplate.mapWallsRows.Count; y++) {
					var columns = genTemplate.mapWallsRows[y].Split (',');

					for (int x = 0; x < columns.Length; x++) {
						Debug.Log (columns[x]);
						var room = RoomFromString (columns[x]);
						Debug.Log (room.AsPrintableString ());
						level.proxyLevel[x, y] = room;
						if (room != null && room.isEntry) {
							level.spawn = new Vector2Int (x, y);
						}
					}
				}

				return level;
			}

			protected Level GenerateRandomLevel (LevelGenTemplate genTemplate) {

				var level = new Level (genTemplate);
				var position = GenerateStartingLocation (level);

				if (level.length > 1) {
					GenerateMainChain (new Vector2Int (position.x, position.y - 1), position, level, level.length - 1);
				}

				GenerateAuxRooms (level);

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
				level.proxyLevel[position.x, position.y] = GenerateEntryRoom (level);
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

					level.proxyLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<RoomSide> () { RoomSide.Wall }, true, level); // Place a new tile here
					level.proxyLevel[newLocation.x, newLocation.y].isEnd = true;
					return true;
				}

				level.proxyLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<RoomSide> () { RoomSide.Open, RoomSide.Door }, true, level); // Place a new tile here 

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
				ProxyRoom room = level.proxyLevel[pos.x, pos.y];

				List<Vector2Int> results = new List<Vector2Int> ();

				if (room.left == RoomSide.Door || room.left == RoomSide.Open) {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasRoom (leftPos, level)) {
						results.Add (leftPos);
					}
				}

				if (room.top == RoomSide.Door || room.top == RoomSide.Open) {
					var upPos = new Vector2Int (pos.x, pos.y - 1);
					if (!PositionHasRoom (upPos, level)) {
						results.Add (upPos);
					}
				}

				if (room.bottom == RoomSide.Door || room.bottom == RoomSide.Open) {
					var bottomPos = new Vector2Int (pos.x, pos.y + 1);
					if (!PositionHasRoom (bottomPos, level)) {
						results.Add (bottomPos);
					}
				}

				if (room.right == RoomSide.Door || room.right == RoomSide.Open) {
					var rightPos = new Vector2Int (pos.x + 1, pos.y);
					if (!PositionHasRoom (rightPos, level)) {
						results.Add (rightPos);
					}
				}

				return results;
			}

			ProxyRoom GenerateEntryRoom (Level level) {

				string start = string.IsNullOrEmpty (level.template.startingRoomString) ? level.template.startingRoomString : DEFAULT_ROOM_START;

				ProxyRoom res = RoomFromString (start);
				res.isEntry = true;
				res.isMainChain = true;
				res.isNearEntry = true;

				return res;
			}

			ProxyRoom RoomFromString (string str) {

				if (string.IsNullOrWhiteSpace (str)) {
					return null;
				}

				ProxyRoom response = new ProxyRoom ();
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
					response.isEnd = splitString[1] == "end";
				}

				return response;
			}

			RoomSide SideFromChar (char ch) {

				return (RoomSide) Enum.ToObject (typeof (RoomSide), ch);
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
									level.proxyLevel[location.x, location.y] = GenerateRoom (location, new List<RoomSide> () {
										RoomSide.Open, RoomSide.Door, RoomSide.Wall, RoomSide.Wall, RoomSide.Wall, RoomSide.Wall
									}, false, level);
								}
							}
						}
					}
				}
			}

			ProxyRoom GenerateRoom (Vector2Int location, List<RoomSide> freeChoiceSides, bool isMainChain, Level level) {
				// Get Top Side
				List<RoomSide> tops = PossibleTopSides (location, freeChoiceSides, level);
				List<RoomSide> lefts = PossibleLeftSides (location, freeChoiceSides, level);
				List<RoomSide> bottoms = PossibleBottomSides (location, freeChoiceSides, level);
				List<RoomSide> rights = PossibleRightSides (location, freeChoiceSides, level);

				tops.Randomize ();
				lefts.Randomize ();
				bottoms.Randomize ();
				rights.Randomize ();

				return new ProxyRoom () {
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

					// Check for a room
					if (level.proxyLevel[surround[i].x, surround[i].y] == null)
						continue;

					if (level.proxyLevel[surround[i].x, surround[i].y].isEntry)
						return true;

				}

				return false;

			}

			List<RoomSide> PossibleTopSides (Vector2Int pos, List<RoomSide> freeChoice, Level level) {
				List<RoomSide> sides = new List<RoomSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y <= 0) {
					sides.Add (RoomSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyRoom room = level.proxyLevel[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (room == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the top
				sides.Add (room.bottom);
				return sides;
			}

			List<RoomSide> PossibleBottomSides (Vector2Int pos, List<RoomSide> freeChoice, Level level) {
				List<RoomSide> sides = new List<RoomSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y >= (level.template.levelHeight - 1)) {
					sides.Add (RoomSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyRoom room = level.proxyLevel[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (room == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the bottom
				sides.Add (room.top);
				return sides;
			}

			List<RoomSide> PossibleLeftSides (Vector2Int pos, List<RoomSide> freeChoice, Level level) {
				List<RoomSide> sides = new List<RoomSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x <= 0) {
					sides.Add (RoomSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyRoom room = level.proxyLevel[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (room == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the left
				sides.Add (room.right);
				return sides;
			}

			List<RoomSide> PossibleRightSides (Vector2Int position, List<RoomSide> freeChoice, Level level) {
				List<RoomSide> sides = new List<RoomSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (position.x >= (level.template.levelWidth - 1)) {
					sides.Add (RoomSide.Wall);
					return sides;
				}

				// Get what's at the position 
				ProxyRoom room = level.proxyLevel[position.x + 1, position.y];

				// If there's nothing then we're free to do anything
				if (room == null) {
					return freeChoice;
				}

				// Otherwise, match what's currently on the right
				sides.Add (room.left);
				return sides;
			}

			bool PositionHasRoom (Vector2Int pos, Level level) {
				if (pos.x >= level.template.levelWidth || pos.y >= level.template.levelHeight || pos.x < 0 || pos.y < 0) {
					return true; // If we go outside the level, pretend we already put a room here
				}
				return (!(level.proxyLevel[pos.x, pos.y] == null));
			}

			void GenerateEnemySpawns (Level level) {
				List<Vector2Int> candidates = new List<Vector2Int> ();

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						if (level.proxyLevel[j, i] != null && !level.proxyLevel[j, i].isNearEntry) {
							//Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 2, 0);
							candidates.Add (new Vector2Int (j, i));
						}
					}
				}

				candidates.Randomize ();
				level.enemySpawnLocationList = candidates.Take (level.template.enemies).ToList ();
				level.enemySpawnLocationList.Randomize ();
			}

			void GenerateTrapSpawns (Level level) {
				List<Vector2Int> candidates = new List<Vector2Int> ();

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						if (level.proxyLevel[j, i] != null && !level.proxyLevel[j, i].isNearEntry) {
							//Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((levelGenTemplate.levelHeight - 1) * 4) + 2, 0);
							candidates.Add (new Vector2Int (j, i));
						}
					}
				}

				candidates.Randomize ();
				level.trapSpawnLocationList = candidates.Take (level.template.traps).ToList ();
			}

			void GenerateEntities (Level level) {
				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

						if (level.proxyLevel[j, i] != null) {

							GenerateRoomEntities (level.proxyLevel[j, i], RoomObjectType.Walls, level.template, false);
							if (level.proxyLevel[j, i].isEntry) {
								GenerateRoomEntities (level.proxyLevel[j, i], RoomObjectType.Entry, level.template, false);
							} else if (level.proxyLevel[j, i].isEnd) {
								GenerateRoomEntities (level.proxyLevel[j, i], RoomObjectType.End, level.template, false);
							} else {

								GenerateRoomEntities (level.proxyLevel[j, i], RoomObjectType.Random, level.template, true);

							}

						}
					}
				}
			}

			void GenerateRoomEntities (ProxyRoom proxyRoom, RoomObjectType type, LevelGenTemplate template, bool allowTraps) {
				string roomString = proxyRoom.AsPrintableString ();
				List<GameObject> goBLList;
				List<GameObject> goBRList;
				List<GameObject> goTLList;
				List<GameObject> goTRList;
				// TL
				// if (allowTraps && UnityEngine.Random.Range (0, 100) < template.traps) {

				// 	goTLList = FindRoomObject (TOPLEFT, roomString.Substring (0, 1), roomString.Substring (1, 1), RoomObjectType.Trap, template).ToList ();
				// 	goTLList.Randomize ();

				// 	if (goTLList.FirstOrDefault () != null) {
				// 		proxyRoom.topLeft.Add (
				// 			new Corner () {
				// 				type = RoomObjectType.Trap,
				// 					name = goTLList[0].name,
				// 					isTrap = true
				// 			});
				// 	}

				// } else {

					goTLList = FindRoomObject (TOPLEFT, roomString.Substring (0, 1), roomString.Substring (1, 1), type, template).ToList ();
					goTLList.Randomize ();

					if (goTLList.FirstOrDefault () != null) {
						proxyRoom.topLeft.Add (
							new Corner () {
								type = type,
									name = goTLList[0].name,
									isTrap = false
							});
					}
				//}

				// TR
				// if (allowTraps && UnityEngine.Random.Range (0, 100) < template.traps) {
				// 	goTRList = FindRoomObject (TOPRIGHT, roomString.Substring (3, 1), roomString.Substring (1, 1), RoomObjectType.Trap, template).ToList ();
				// 	goTRList.Randomize ();

				// 	if (goTRList.FirstOrDefault () != null) {
				// 		proxyRoom.topRight.Add (
				// 			new Corner () {
				// 				type = RoomObjectType.Trap,
				// 					name = goTRList[0].name,
				// 					isTrap = true
				// 			});
				// 	}

				// } else {
					goTRList = FindRoomObject (TOPRIGHT, roomString.Substring (3, 1), roomString.Substring (1, 1), type, template).ToList ();
					goTRList.Randomize ();

					if (goTRList.FirstOrDefault () != null) {
						proxyRoom.topRight.Add (
							new Corner () {
								type = type,
									name = goTRList[0].name,
									isTrap = false
							});
					}

				//}

				// BL
				// if (allowTraps && UnityEngine.Random.Range (0, 100) < template.traps) {
				// 	goBLList = FindRoomObject (BOTTOMLEFT, roomString.Substring (0, 1), roomString.Substring (2, 1), RoomObjectType.Trap, template).ToList ();
				// 	goBLList.Randomize ();

				// 	if (goBLList.FirstOrDefault () != null) {
				// 		proxyRoom.bottomLeft.Add (
				// 			new Corner () {
				// 				type = RoomObjectType.Trap,
				// 					name = goBLList[0].name,
				// 					isTrap = true
				// 			});
				// 	}

				// } else {
					goBLList = FindRoomObject (BOTTOMLEFT, roomString.Substring (0, 1), roomString.Substring (2, 1), type, template).ToList ();
					goBLList.Randomize ();

					if (goBLList.FirstOrDefault () != null) {
						proxyRoom.bottomLeft.Add (
							new Corner () {
								type = type,
									name = goBLList[0].name,
									isTrap = false
							});
					}

				//}

				// BR
				// if (allowTraps && UnityEngine.Random.Range (0, 100) < template.traps) {
				// 	goBRList = FindRoomObject (BOTTOMRIGHT, roomString.Substring (3, 1), roomString.Substring (2, 1), RoomObjectType.Trap, template).ToList ();
				// 	goBRList.Randomize ();

				// 	if (goBRList.FirstOrDefault () != null) {

				// 		proxyRoom.bottomRight.Add (
				// 			new Corner () {
				// 				type = RoomObjectType.Trap,
				// 					name = goBRList[0].name,
				// 					isTrap = true
				// 			});
				// 	}
				// } else {
					goBRList = FindRoomObject (BOTTOMRIGHT, roomString.Substring (3, 1), roomString.Substring (2, 1), type, template).ToList ();
					goBRList.Randomize ();

					if (goBRList.FirstOrDefault () != null) {

						proxyRoom.bottomRight.Add (
							new Corner () {
								type = type,
									name = goBRList[0].name,
									isTrap = false
							});
					}

				//}

			}

			IEnumerable<GameObject> FindRoomObject (string corner, string wall1, string wall2, RoomObjectType type, LevelGenTemplate levelGenTemplate) {

				IEnumerable<GameObject> results = null;

				switch (type) {
					case RoomObjectType.Walls:
						results = levelGenTemplate.levelElements.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						break;

					case RoomObjectType.Entry:
						results = levelGenTemplate.startProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// if (results.Count () == 0) {
						// 	results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// }

						break;

					case RoomObjectType.End:
						results = levelGenTemplate.endProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// if (results.Count () == 0) {
						// 	results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// }

						break;

					case RoomObjectType.Trap:
						results = levelGenTemplate.trapProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// if (results.Count () == 0) {
						// 	results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						// }

						break;

					case RoomObjectType.Random:

						results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));

						break;

				}

				return results;

			}

			bool MatchSpriteName (string name, string corner, string wall1, string wall2) {
				string[] nameSplit = name.ToLower ().Split ('_');

				if (nameSplit.Length != 4) {
					Debug.Log ("Invalid sprite name");
					return false;
				}

				string open = "oaxy";
				string door = "daxz";
				string wall = "wayz";
				string exit = "edaxy";

				string first = nameSplit[3].Substring (0, 1);
				string second = nameSplit[3].Substring (1, 1);

				return (nameSplit[2] == corner.ToLower () &&
					((wall1.ToLower () == "o" && open.IndexOf (first) >= 0) ||
						(wall1.ToLower () == "d" && door.IndexOf (first) >= 0) ||
						(wall1.ToLower () == "w" && wall.IndexOf (first) >= 0) ||
						(wall1.ToLower () == "e" && exit.IndexOf (first) >= 0)) &&
					((wall2.ToLower () == "o" && open.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "d" && door.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "w" && wall.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "e" && exit.IndexOf (second) >= 0))
				);
			}

		}
	}

}