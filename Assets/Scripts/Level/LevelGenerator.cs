using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelGenerator : MonoBehaviour {

			public GameObject parent;

			public int levelLength = 7;
			public int levelWidth = 10;
			public int levelHeight = 10;
			public int seed = 1;

			public string levelName;
			public string floorName;

			public List<Floor> floors;
			public List<LevelElements> levelElements;

			public GeneratorChunk[, ] levelChunks;

			// Use this for initialization
			void Start () {

				//GenerateLevel ();
				//PrintLevelDebug ();

			}

			public void Initialize (GameObject parent) {
				this.parent = parent;
				GenerateLevel ();
				PopulateLevelChunks ();
				PrintLevelDebug ();
			}

			void GenerateLevel () {
				Debug.Log ("Generating Level");
				levelChunks = new GeneratorChunk[levelWidth, levelHeight];

				UnityEngine.Random.InitState (seed); // Psuedo random seed gives predictable results, so we can save the seed and recreate the level

				// Seems like a sensible limit
				if (levelLength < Mathf.Sqrt (levelLength * levelWidth)) {
					levelLength = (int) Mathf.Sqrt (levelLength * levelWidth);
				}

				if (levelLength < 0) {
					return;
				}

				var pos = GenerateStartingLocation ();
				if (levelLength > 0) {

					GenerateMainChain (new Vector2Int (pos.x, pos.y - 1), pos, levelLength);
					GenerateAuxRooms ();
				}

			}

			Vector2Int GenerateStartingLocation () {
				Debug.Log ("Generating Starting Location");

				// Starting at the bottom and going up means we should never create a chain that fails completely and roles all the way back to the entry
				// This is important!				
				// It also means the player starts at the bottom and plays upwards, which is ideal
				Vector2Int pos = new Vector2Int (UnityEngine.Random.Range (0, levelWidth), levelHeight - 1);
				levelChunks[pos.x, pos.y] = GenerateEntryRoomChunk ();
				return pos;
			}

			bool GenerateMainChain (Vector2Int newLocation, Vector2Int lastLocation, int lengthRemaining) {

				Debug.Log ("Generating Main Chain");
				if (lengthRemaining == 0) {
					return true;
				}

				// The end room is considered special
				if (lengthRemaining == 1) {
					Debug.Log ("End of main chain");

					levelChunks[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Wall }, true); // Place a new tile here
					levelChunks[newLocation.x, newLocation.y].isEnd = true;
					//PrintLevelDebug ();
					return true;
				}

				levelChunks[newLocation.x, newLocation.y] = GenerateRoom (newLocation, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door }, true); // Place a new tile here 

				//PrintLevelDebug ();
				List<Vector2Int> directions = PossibleDirections (newLocation);

				directions = RandomizeDirections (directions); // directions.OrderBy (a => Guid.NewGuid ()).ToList ();

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
					//level[newLocation.x, newLocation.y] = "####";
				}

				return result;
			}

			//FIXME: Probably shit
			List<Vector2Int> RandomizeDirections (List<Vector2Int> list) {
				Vector2Int temp;

				for (int i = 0; i < list.Count; i++) {
					int r = UnityEngine.Random.Range (i, list.Count);
					temp = list[r];
					list[r] = list[i];
					list[i] = temp;
				}

				return list;
			}

			void PopulateLevelChunks () {
				for (int i = 0; i < levelHeight; i++) {
					for (int j = 0; j < levelWidth; j++) {
						if (levelChunks[j, i] != null) {
							Chunk c = FindChunk (levelChunks[j, i]);
							if (c != null) {
								var f = FindFloor ();
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((levelHeight - 1) * 4), 0);

								GameObject.Instantiate (f.gameObject, pos, Quaternion.identity, parent.transform);

								GameObject.Instantiate (c.gameObject, pos, Quaternion.identity, parent.transform);
								Debug.Log (c.name);
								Debug.Log (j + ":" + j * 4);
								Debug.Log (i + ":" + (i * -4 + ((levelHeight - 1) * 4)));
							}

						}
					}
				}
			}

			Floor FindFloor () {
				return floors.FirstOrDefault (f => f.name == floorName);
			}

			Chunk FindChunk (GeneratorChunk chunk) {
				return levelElements.FirstOrDefault (l => l.name == levelName).chunks.FirstOrDefault (c => c.isEntry == chunk.isEntry);
			}

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
							if ((levelChunks[j, i] != null)) {
								Vector2Int pos = new Vector2Int (j, i);
								List<Vector2Int> dirs = PossibleDirections (pos);

								foreach (Vector2Int location in dirs) {
									newRooms = true;
									levelChunks[location.x, location.y] = GenerateRoom (location, new List<Chunk.ChunkSide> () { Chunk.ChunkSide.Open, Chunk.ChunkSide.Door, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall, Chunk.ChunkSide.Wall }, false);
								}
							}
						}
					}
				}
			}

			GeneratorChunk GenerateRoom (Vector2Int location, List<Chunk.ChunkSide> freeChoiceSides, bool isMainChain) {
				// Get Top Side
				List<Chunk.ChunkSide> tops = PossibleTopSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> lefts = PossibleLeftSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> bottoms = PossibleBottomSides (location, freeChoiceSides);
				List<Chunk.ChunkSide> rights = PossibleRightSides (location, freeChoiceSides);

				return new GeneratorChunk () { isEntry = false, isEnd = false, isMainChain = isMainChain, top = tops[0], left = lefts[0], bottom = bottoms[0], right = rights[0] };
			}

			GeneratorChunk GenerateEntryRoomChunk () {
				return new GeneratorChunk () { isEntry = true, isEnd = false, isMainChain = true, top = Chunk.ChunkSide.Door, left = Chunk.ChunkSide.Wall, bottom = Chunk.ChunkSide.Wall, right = Chunk.ChunkSide.Wall };
			}

			List<Chunk.ChunkSide> PossibleTopSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y == 0) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				GeneratorChunk chunk = levelChunks[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice = RandomizeSides (freeChoice); //freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
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
				GeneratorChunk chunk = levelChunks[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice = RandomizeSides (freeChoice); //freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them

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
				GeneratorChunk chunk = levelChunks[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice = RandomizeSides (freeChoice); // freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.right);
				return sides;
			}

			List<Chunk.ChunkSide> PossibleRightSides (Vector2Int pos, List<Chunk.ChunkSide> freeChoice) {
				List<Chunk.ChunkSide> sides = new List<Chunk.ChunkSide> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x == (levelWidth - 1)) {
					sides.Add (Chunk.ChunkSide.Wall);
					return sides;
				}

				// Get what's at the position 
				GeneratorChunk chunk = levelChunks[pos.x + 1, pos.y];

				// If there's nothing then we're free to do anything
				if (chunk == null) {
					freeChoice = RandomizeSides (freeChoice); // freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				sides.Add (chunk.left);
				return sides;
			}

			//FIXME: Probably shit
			List<Chunk.ChunkSide> RandomizeSides (List<Chunk.ChunkSide> list) {
				Chunk.ChunkSide temp;

				for (int i = 0; i < list.Count; i++) {
					int r = UnityEngine.Random.Range (i, list.Count);
					temp = list[r];
					list[r] = list[i];
					list[i] = temp;
				}

				return list;
			}

			List<Vector2Int> PossibleDirections (Vector2Int pos) {
				GeneratorChunk chunk = levelChunks[pos.x, pos.y];

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
				return (!(levelChunks[pos.x, pos.y] == null));
			}

			void PrintLevelDebug () {
				Debug.Log ("Printing level");
				for (int i = 0; i < levelHeight; i++) {
					string line = "";
					for (int j = 0; j < levelWidth; j++) {
						if (levelChunks[j, i] != null) {

							if (levelChunks[j, i].isEntry) {
								line += "[" + levelChunks[j, i].AsPrintableString () + "]";

							} else if (levelChunks[j, i].isEnd) {
								line += "{" + levelChunks[j, i].AsPrintableString () + "}";

							} else if (levelChunks[j, i].isMainChain) {
								line += "<" + levelChunks[j, i].AsPrintableString () + ">";
							} else {

								line += "(" + levelChunks[j, i].AsPrintableString () + ")";
							}
						} else {
							line += "-####-";
						}
					}

					Debug.Log (line);
				}
			}

			public class GeneratorChunk {
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