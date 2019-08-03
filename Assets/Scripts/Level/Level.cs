using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class Level {
            public LevelGenTemplate template;
            public ProxyRoom[, ] proxyLevel;
            public int length;
			public Vector2Int spawn;
			public List<Vector2Int> enemySpawnLocationList;

            public Level(LevelGenTemplate template)
            {
                this.template = template;
                this.length = CapLevelLength(template.levelLength, template.levelWidth, template.levelHeight);
                Debug.Log("Level length: " + length);
                proxyLevel = new ProxyRoom[template.levelWidth, template.levelHeight];
            }

			int CapLevelLength (int levelLength, int levelWidth, int levelHeight) {
				// Seems like a sensible limit
				if (levelLength > Mathf.Sqrt (levelHeight * levelWidth)) {
					return (int) Mathf.Sqrt (levelHeight * levelWidth);
				}

				if (levelLength < 0) {
					return 0;
				}

				return levelLength;
			}           

			public List<Vector2Int> MovementDirections (Vector2Int pos, bool entryAllowed, bool endAllowed) {
				var results = PossibleMovementDirections(pos);
				return results.TakeWhile(r => (!proxyLevel[r.x, r.y].isEnd && !proxyLevel[r.x, r.y].isEntry) || (proxyLevel[r.x, r.y].isEnd && endAllowed) || (proxyLevel[r.x, r.y].isEntry && entryAllowed)).ToList();
			}			 

			public List<Vector2Int> PossibleMovementDirections (Vector2Int pos) {
				ProxyRoom room = proxyLevel[pos.x, pos.y];

				List<Vector2Int> results = new List<Vector2Int> ();



				if (room.left == RoomSide.Door || room.left == RoomSide.Open) {
					var leftPos = new Vector2Int (pos.x - 1, pos.y);

					results.Add (leftPos);
				}

				if (room.top == RoomSide.Door || room.top == RoomSide.Open) {
					var upPos = new Vector2Int (pos.x, pos.y - 1);

					results.Add (upPos);

				}

				if (room.bottom == RoomSide.Door || room.bottom == RoomSide.Open) {
					var bottomPos = new Vector2Int (pos.x, pos.y + 1);

					results.Add (bottomPos);

				}

				if (room.right == RoomSide.Door || room.right == RoomSide.Open) {
					var rightPos = new Vector2Int (pos.x + 1, pos.y);

					results.Add (rightPos);

				}
				return results;

			} 

			public List<Vector2Int> ConstructRandomPatrolPath (Vector2Int pos, int length) {
				if (length <= 1) {
					return new List<Vector2Int> () { pos };
				}

				List<Vector2Int> dirs = PossibleMovementDirections (pos);
				dirs.Randomize ();

				if (dirs.Count > 0) {

					var x = new List<Vector2Int> () { pos };
					x.AddRange (ConstructRandomPatrolPath (dirs[0], length - 1));
					return x;
				} else {
					return new List<Vector2Int> () { pos };
				}
			}     

			public Vector3 ConvertLevelPosToWorld (Vector2Int pos) {
				return new Vector3 (pos.x * 4 + 2, pos.y * -4 + ((template.levelHeight - 1) * 4) + 2);
			}  

			public Vector2Int ConvertWorldToLevelPos(Vector3 pos) {

				//i * -4 + ((level.template.levelHeight - 1) * 4)

				return new Vector2Int((int)((pos.x) / 4), (int)( (template.levelHeight)  -  (pos.y / 4))); 
			}

			public void Print () {
				Debug.Log ("Printing level");
				for (int i = 0; i < template.levelHeight; i++) {
					string line = "";
					for (int j = 0; j < template.levelWidth; j++) {
						if (proxyLevel[j, i] != null) {

							if (proxyLevel[j, i].isEntry) {
								line += "[" + proxyLevel[j, i].AsPrintableString () + "]";

							} else if (proxyLevel[j, i].isEnd) {
								line += "{" + proxyLevel[j, i].AsPrintableString () + "}";

							} else if (proxyLevel[j, i].isMainChain) {
								line += "<" + proxyLevel[j, i].AsPrintableString () + ">";
							} else {

								line += "(" + proxyLevel[j, i].AsPrintableString () + ")";
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