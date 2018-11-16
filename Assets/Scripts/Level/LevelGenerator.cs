using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelGenerator : MonoBehaviour {

			private GameObject parent;

			private int levelLength = 7;
			private int levelWidth = 10;
			private int levelHeight = 10;
			private int seed = 1;

			private string levelNameTemplate;
			private string floorName;

			public Floor floor;
			public LevelElements levelElements;

			public PlaceholderChunk[, ] placeholderLevel;

			public LevelGenTemplate[] levelGenTemplates;

			// Use this for initialization
			void Start () {

			}

			public void Initialize (GameObject parent) {
				this.parent = parent;

				//GenerateLevel ();
				//PopulateLevelChunks ();
				//PrintLevelDebug ();
			}

			// Template -> Generate -> GeneratedLevel
			public int GenerateLevel (string name, string template) {
				Debug.Log ("Generating Level");

				LevelGenTemplate levelGenTemplate = GetLevelGenTemplate (template);

				this.levelLength = levelGenTemplate.levelLength;
				this.levelWidth = levelGenTemplate.levelWidth;
				this.levelHeight = levelGenTemplate.levelHeight;
				this.levelNameTemplate = levelGenTemplate.levelNameTemplate;
				//this.floorName = levelGenTemplate.floorName;
				this.floor = levelGenTemplate.floor;
				this.levelElements = levelGenTemplate.levelElements;

				placeholderLevel = new PlaceholderChunk[levelWidth, levelHeight];

				//UnityEngine.Random.InitState (seed); // Psuedo random seed gives predictable results, so we can save the seed and recreate the level

				// Seems like a sensible limit
				if (levelLength < Mathf.Sqrt (levelLength * levelWidth)) {
					levelLength = (int) Mathf.Sqrt (levelLength * levelWidth);
				}

				if (levelLength < 1) {
					return -1;
				}

				var position = GenerateStartingLocation ();
				if (levelLength > 1) {

					GenerateMainChain (new Vector2Int (position.x, position.y - 1), position, levelLength - 1);
					GenerateAuxRooms ();
				}

				PopulateLevelTilemap ();
				PrintLevelDebug ();

				return seed;

			}

			LevelGenTemplate GetLevelGenTemplate (string template) {
				return levelGenTemplates.FirstOrDefault (t => t.name == template);
			}

			Vector2Int GenerateStartingLocation () {
				Debug.Log ("Generating Starting Location");

				// Starting at the bottom and going up means we should never create a chain that fails completely and roles all the way back to the entry
				// This is important!				
				// It also means the player starts at the bottom and plays upwards, which is ideal
				Vector2Int position = new Vector2Int (UnityEngine.Random.Range (0, levelWidth), levelHeight - 1);
				placeholderLevel[position.x, position.y] = GenerateEntryRoomChunk ();
				return position;
			}

			bool GenerateMainChain (Vector2Int newLocation, Vector2Int lastLocation, int lengthRemaining) {

				if (lengthRemaining == 0) {
					return true;
				}

				Debug.Log ("Generating Main Chain");

				// The end room is considered special
				if (lengthRemaining == 1) {
					Debug.Log ("End of main chain");

					placeholderLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Wall }, true); // Place a new tile here
					placeholderLevel[newLocation.x, newLocation.y].isEnd = true;
					return true;
				}

				placeholderLevel[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door }, true); // Place a new tile here 

				List<Vector2Int> directions = PossibleDirections (newLocation);

				directions.Randomize ();

				bool result = false;

				// Iterate over potential directions from here
				for (int i = 0; i < directions.Count; i++) {

					result = GenerateMainChain (directions[i], newLocation, lengthRemaining - 1);

					if (result) // If the chain is okay, don't need to try any new directions. We could probably return true here
					{
						break;
					}
				}

				// If we didn't complete the chain, abandon this location
				// Fixme: we probably have to change a side because of this
				if (!result) {
					Debug.Log ("Abandoning chain, role back one step");
				}

				return result;
			}

			public void PopulateLevelTilemap () {
				for (int kk = 0; kk < parent.transform.childCount; kk++) {
					GameObject.Destroy (parent.transform.GetChild (kk).gameObject);
				}

				for (int i = 0; i < levelHeight; i++) {
					for (int j = 0; j < levelWidth; j++) {
						Vector3 pos = new Vector3 (j * 4, i * -4 + ((levelHeight - 1) * 4), 0);

						if (placeholderLevel[j, i] != null) {
							
							if (floor != null) {
								GameObject.Instantiate (floor.gameObject, pos, Quaternion.identity, parent.transform);
							}
							GameObject go = FindChunkObject (placeholderLevel[j, i]);
							if (go != null) {
								
								var x = GameObject.Instantiate (go, pos, Quaternion.identity, parent.transform);
							} 
						} else {
							GameObject gw = FindChunkObject (ChunkFromString ("wwww"));
							if (gw != null) {
								GameObject.Instantiate (gw, pos, Quaternion.identity, parent.transform);
							}
						}
					}
				}
			}

			GameObject FindChunkObject (PlaceholderChunk chunk) {
				return levelElements.chunkObjects.FirstOrDefault (g => g != null && ChunkMatchesString (chunk, g.name));
			}

			bool ChunkMatchesString (PlaceholderChunk chunk, string s) {
				var goChunk = ChunkFromString (s);
				return (
					goChunk.isEntry == chunk.isEntry &&
					goChunk.top == chunk.top &&
					goChunk.bottom == chunk.bottom &&
					goChunk.left == chunk.left &&
					goChunk.right == chunk.right
				);
			}

			PlaceholderChunk ChunkFromString (string s) {
				PlaceholderChunk response = new PlaceholderChunk ();
				string[] sp = s.Split ('_');

				if (sp.Length < 1) {
					return null;
				}

				if (sp.Length > 0) {

					response.left = SideFromChar (sp[0][0]);
					response.top = SideFromChar (sp[0][1]);
					response.bottom = SideFromChar (sp[0][2]);
					response.right = SideFromChar (sp[0][3]);
				}

				if (sp.Length > 1) {
					response.isEntry = sp[1] == "entry";
					response.isEnd = sp[1] == "exit";
				}

				return response;
			}

			Chunk.ChunkSide SideFromChar (char c) {
				switch (c) {
					case 'w':
						return Chunk.ChunkSide.Wall;
					case 'd':
						return Chunk.ChunkSide.Door;
					case 'o':
						return Chunk.ChunkSide.Open;
				}
				return Chunk.ChunkSide.Wall;
			}

			// Chunk FindChunk (PlaceholderChunk chunk) {
			// 	return levelElements.chunks.FirstOrDefault (c => c != null &&
			// 		c.isEntry == chunk.isEntry &&
			// 		c.top == chunk.top &&
			// 		c.bottom == chunk.bottom &&
			// 		c.left == chunk.left &&
			// 		c.right == chunk.right);
			// }

			void GenerateAuxRooms () {
				Debug.Log ("Generating Aux Rooms");
				bool newRooms = true;

				// iterate through every position, checking for neighbours and creating rooms accordingly. 
				// Keep iterating until we stop creating rooms				
				while (newRooms) {
					//PrintLevelDebug ();
					//Debug.Log ("Iteration");
					newRooms = false;
					for (int i = 0; i < levelHeight; i++) {
						for (int j = 0; j < levelWidth; j++) {
							if ((placeholderLevel[j, i] != null)) {
								Vector2Int pos = new Vector2Int (j, i);
								List<Vector2Int> dirs = PossibleDirections (pos);

								foreach (Vector2Int location in dirs) {
									newRooms = true;
									placeholderLevel[location.x, location.y] = GenerateRoom (location, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall }, false);
								}
							}
						}
					}
				}
			}

			PlaceholderChunk GenerateRoom (Vector2Int location, List<Chunk.ChunkSide> freeChoiceSides, bool isMainChain) {
				// Get Top Side
				List<Chunk.ChunkSide> tops = PossibleTopSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> lefts = PossibleLeftSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> bottoms = PossibleBottomSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> rights = PossibleRightSides (location, freeChoiceSides);

				return new PlaceholderChunk () { isEntry = false, isEnd = false, isMainChain = isMainChain, top = tops[0], left = lefts[0], bottom = bottoms[0], right = rights[0] };
			}

			PlaceholderChunk GenerateEntryRoomChunk () {
				return new PlaceholderChunk () { isEntry = true, isEnd = false, isMainChain = true, top = Chunk.ChunkSide.Door, left = Chunk.ChunkSide.Wall, bottom = Chunk.ChunkSide.Wall, right = Chunk.ChunkSide.Wall };
			}

			List<Chunk.ChunkSide> PossibleTopSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y == 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice.Randomize ();
					//freeChoice = RandomizeSides (freeChoice); //freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.bottom);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleBottomSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y == (levelHeight - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice.Randomize ();
					//freeChoice = RandomizeSides (freeChoice); //freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them

					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.top);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleLeftSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x == 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice.Randomize ();
					//freeChoice = RandomizeSides (freeChoice); // freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.right);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleRightSides (Vector2Int position, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (position.x == (levelWidth - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				PlaceholderChunk chunk = placeholderLevel[position.x + 1, position.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice.Randomize ();
					//freeChoice = RandomizeSides (freeChoice); // freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				sides.Add (chunk.left);
				return sides;
			}

			List<Vector2Int> PossibleDirections (Vector2Int pos) {
				PlaceholderChunk chunk = placeholderLevel[pos.x, pos.y];

				List<Vector2Int> results = new List<Vector2Int> ();

				if (chunk.left == Chunk.ChunkSide.Door || chunk.left == Chunk.ChunkSide.Open) {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasChunk (leftPos)) {
						results.Add (leftPos);
					}
				}

				if (chunk.top == Chunk.ChunkSide.Door || chunk.top == Chunk.ChunkSide.Open) {
					var upPos = new Vector2Int (pos.x, pos.y - 1);
					if (!PositionHasChunk (upPos)) {
						results.Add (upPos);
					}
				}

				if (chunk.bottom == Chunk.ChunkSide.Door || chunk.bottom == Chunk.ChunkSide.Open) {
					var bottomPos = new Vector2Int (pos.x, pos.y + 1);
					if (!PositionHasChunk (bottomPos)) {
						results.Add (bottomPos);
					}
				}

				if (chunk.right == Chunk.ChunkSide.Door || chunk.right == Chunk.ChunkSide.Open) {
					var rightPos = new Vector2Int (pos.x + 1, pos.y);
					if (!PositionHasChunk (rightPos)) {
						results.Add (rightPos);
					}
				}

				return results;
			}

			bool PositionHasChunk (Vector2Int pos) {
				if (pos.x >= levelWidth || pos.y >= levelHeight || pos.x < 0 || pos.y < 0) {
					return true; // If we go outside the level, pretend we already put a chunk here
				}
				return (!(placeholderLevel[pos.x, pos.y] == null));
			}

			void PrintLevelDebug () {
				Debug.Log ("Printing level");
				for (int i = 0; i < levelHeight; i++) {
					string line = "";
					for (int j = 0; j < levelWidth; j++) {
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

			public class PlaceholderChunk {
				public bool isEntry = false;
				public bool isEnd = false;
				public bool isMainChain = false;

				public Chunk.ChunkSide top;
				public Chunk.ChunkSide left;
				public Chunk.ChunkSide bottom;
				public Chunk.ChunkSide right;

				public string AsPrintableString () {
					string s = "";

					switch (left) {
						case Chunk.ChunkSide.Wall:
							s += "W";
							break;
						case Chunk.ChunkSide.Door:
							s += "D";
							break;
						case Chunk.ChunkSide.Open:
							s += "O";
							break;
					}

					switch (top) {
						case Chunk.ChunkSide.Wall:
							s += "W";
							break;
						case Chunk.ChunkSide.Door:
							s += "D";
							break;
						case Chunk.ChunkSide.Open:
							s += "O";
							break;
					}

					switch (bottom) {
						case Chunk.ChunkSide.Wall:
							s += "W";
							break;
						case Chunk.ChunkSide.Door:
							s += "D";
							break;
						case Chunk.ChunkSide.Open:
							s += "O";
							break;
					}

					switch (right) {
						case Chunk.ChunkSide.Wall:
							s += "W";
							break;
						case Chunk.ChunkSide.Door:
							s += "D";
							break;
						case Chunk.ChunkSide.Open:
							s += "O";
							break;
					}

					return s;
				}
			}
		}
	}
}