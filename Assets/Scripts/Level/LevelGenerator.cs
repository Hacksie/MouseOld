using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelGenerator : MonoBehaviour {

			public int levelLength = 7;
			public int levelWidth = 10;
			public int levelHeight = 10;

			public string[, ] level;

			// Use this for initialization
			void Start () {
				level = new string[levelWidth, levelHeight];
				var pos = GenerateStartingLocation ();

				PrintLevelDebug ();

			}

			Vector2Int GenerateStartingLocation () {
				Vector2Int pos = new Vector2Int (0, levelHeight - 1);

				pos.x = UnityEngine.Random.Range (0, levelWidth);

				// Chunk entry = ScriptableObject.Instantiate<Chunk>();

				// entry.bottom = Chunk.ChunkSide.Wall;
				// entry.left = Chunk.ChunkSide.Wall;
				// entry.right = Chunk.ChunkSide.Wall;
				// entry.top = Chunk.ChunkSide.Door;

				// Starting at the bottom and going up means we should never create a chain that fails completely and roles all the way back to the entry
				// This is important!

				level[pos.x, pos.y] = "1211";

				Debug.Log (pos);

				GenerateMainChain (new Vector2Int (pos.x, pos.y - 1), pos, levelLength);
				GenerateAuxRooms();

				return pos;
			}

			void GenerateAuxRooms()
			{
				// iterate through every position, checking for neighbours and creating rooms accordingly. 
				// Keep iterating until we stop creating rooms

			}

			bool HasNeighbours(Vector2Int location) 
			{
				
				return false;
			}

			bool GenerateMainChain (Vector2Int newLocation, Vector2Int lastLocation, int lengthRemaining) {

				Debug.Log ("Generating main chain");

				// The end room is considered special
				if (lengthRemaining == 0) {
					Debug.Log ("End of main chain");

					List<string> endFreeChoice = new List<string> () { "1" }; // Don't expect me to honor the order

					List<string> endTops = PossibleTopSides (newLocation, endFreeChoice);
					List<string> endLefts = PossibleLeftSides (newLocation, endFreeChoice);
					List<string> endBottoms = PossibleBottomSides (newLocation, endFreeChoice);
					List<string> endRights = PossibleRightSides (newLocation, endFreeChoice);

					string endRoom = endLefts[0]  + endTops[0] + endBottoms[0] + endRights[0];

					level[newLocation.x, newLocation.y] = endRoom;
					PrintLevelDebug ();
					return true;
				}

				List<string> mainChainFreeChoice = new List<string> () { "3", "2" }; // Don't expect me to honor the order

				// Get Top Side
				List<string> tops = PossibleTopSides (newLocation, mainChainFreeChoice);
				List<string> lefts = PossibleLeftSides (newLocation, mainChainFreeChoice);
				List<string> bottoms = PossibleBottomSides (newLocation, mainChainFreeChoice);
				List<string> rights = PossibleRightSides (newLocation, mainChainFreeChoice);

				string room = lefts[0] + tops[0] + bottoms[0] + rights[0];

				level[newLocation.x, newLocation.y] = room; // Try and place a new tile here 

				PrintLevelDebug ();
				List<Vector2Int> directions = PossibleDirections (newLocation);

				directions = directions.OrderBy (a => Guid.NewGuid ()).ToList ();

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
					level[newLocation.x, newLocation.y] = "####";
				}

				return result;
			}

			List<string> PossibleTopSides (Vector2Int pos, List<string> freeChoice) {
				List<string> sides = new List<string> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y == 0) {
					sides.Add ("1");
					return sides;
				}

				// Get what's at the position 
				string chunk = level[pos.x, pos.y - 1];

				// If there's nothing then we're free to do anything
				if (string.IsNullOrEmpty (chunk)) {
					freeChoice = freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.Substring (2, 1));
				return sides;
			}

			List<string> PossibleBottomSides (Vector2Int pos, List<string> freeChoice) {
				List<string> sides = new List<string> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.y == (levelHeight - 1)) {
					sides.Add ("1");
					return sides;
				}

				// Get what's at the position 
				string chunk = level[pos.x, pos.y + 1];

				// If there's nothing then we're free to do anything
				if (string.IsNullOrEmpty (chunk)) {
					freeChoice = freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the top side
				sides.Add (chunk.Substring (1, 1));
				return sides;
			}

			List<string> PossibleLeftSides (Vector2Int pos, List<string> freeChoice) {
				List<string> sides = new List<string> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x == 0) {
					sides.Add ("1");
					return sides;
				}

				// Get what's at the position 
				string chunk = level[pos.x - 1, pos.y];

				// If there's nothing then we're free to do anything
				if (string.IsNullOrEmpty (chunk)) {
					freeChoice = freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.Substring (3, 1));
				return sides;
			}

			List<string> PossibleRightSides (Vector2Int pos, List<string> freeChoice) {
				List<string> sides = new List<string> ();

				// If the side would lead out of the level, the side has to be wall
				if (pos.x == (levelWidth - 1)) {
					sides.Add ("1");
					return sides;
				}

				// Get what's at the position 
				string chunk = level[pos.x + 1, pos.y];

				// If there's nothing then we're free to do anything
				if (string.IsNullOrEmpty (chunk)) {
					freeChoice = freeChoice.OrderBy (a => Guid.NewGuid ()).ToList (); // Randomize them
					return freeChoice;
				}

				// Otherwise, match what's currently in the bottom side
				sides.Add (chunk.Substring (0, 1));
				return sides;
			}

			List<Vector2Int> PossibleDirections (Vector2Int pos) {

				string s = level[pos.x, pos.y];
				List<Vector2Int> results = new List<Vector2Int> ();

				if (s[0] == '2' || s[0] == '3') {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasChunk (leftPos)) {
						//Debug.Log ("We can move left");
						results.Add (leftPos);
					}

				}

				if (s[1] == '2' || s[1] == '3') {
					var upPos = new Vector2Int (pos.x, pos.y - 1);
					if (!PositionHasChunk (upPos)) {
						//Debug.Log ("We can move up");
						results.Add (upPos);
					}
				}

				if (s[2] == '2' || s[2] == '3') {
					var downPos = new Vector2Int (pos.x, pos.y + 1);
					if (!PositionHasChunk (downPos)) {
						//Debug.Log ("We can move down");
						results.Add (downPos);
					}
				}

				if (s[3] == '2' || s[3] == '3') {
					var rightPos = new Vector2Int (pos.x - 1, pos.y);
					if (!PositionHasChunk (rightPos)) {
						//Debug.Log ("We can move right");
						results.Add (rightPos);
					}
				}

				//1234

				return results;

			}

			bool PositionHasChunk (Vector2Int pos) {
				//Debug.Log("Checking if pos " + pos + " has chunk");
				if (pos.x >= levelWidth || pos.y >= levelHeight || pos.x < 0 || pos.y < 0) {
					//Debug.Log("We crossed outside the level checking for new chunk locations");
					return true; // If we go outside the level, pretend we already put a chunk here
				}

				//Debug.Log (level[pos.x, pos.y]);
				return (!string.IsNullOrEmpty (level[pos.x, pos.y]));
			}

			// Update is called once per frame
			void Update () {

			}

			void PrintLevelDebug () {
				Debug.Log("Printing level");
				for (int i = 0; i < levelHeight; i++) {
					string line = "";
					//Debug.Log ("i" + i);
					for (int j = 0; j < levelWidth; j++) {
						//Debug.Log ("j " + j);
						if (!string.IsNullOrEmpty (level[j, i])) {
							line += "[" + level[j, i] + "]";
						} else {
							line += "[0000]";
						}

					}

					Debug.Log (line);
				}
			}
		}
	}
}