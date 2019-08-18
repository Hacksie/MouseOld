using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Level
    {
		[System.Serializable]
        public class Level
        {
            public LevelGenTemplate template;
            public ProxyRow[] map;
            //public ProxyRoom[,] proxyLevel;
            public int length;
            public Vector2Int spawn;
            public List<Vector2Int> enemySpawnLocationList;
            public List<Vector2Int> trapSpawnLocationList;

            //public Vector2Int? alert;

            public Level(LevelGenTemplate template)
            {
                this.template = template;
                this.length = CapLevelLength(template.levelLength, template.levelWidth, template.levelHeight);
                Debug.Log("Level length: " + length);


				
                map = new ProxyRow[template.levelHeight];
                for (int row = 0; row < template.levelHeight; row++)
                {
					map[row] = new ProxyRow();
					map[row].rooms = new ProxyRoom[template.levelWidth];

					for(int col = 0; col < template.levelWidth; col++)
					{
					 	map[row].rooms[col] = null;
					}
                }
                //proxyLevel = new ProxyRoom[template.levelWidth, template.levelHeight];
            }

            int CapLevelLength(int levelLength, int levelWidth, int levelHeight)
            {
                // Seems like a sensible limit
                if (levelLength > Mathf.Sqrt(levelHeight * levelWidth))
                {
                    return (int)Mathf.Sqrt(levelHeight * levelWidth);
                }

                if (levelLength < 0)
                {
                    return 0;
                }

                return levelLength;
            }

            public List<Vector2Int> MovementDirections(Vector2Int pos, bool entryAllowed, bool endAllowed)
            {
                var results = PossibleMovementDirections(pos);
                return results.TakeWhile(r => (!map[r.y].rooms[r.x].isEnd && !map[r.y].rooms[r.x].isEntry) || (map[r.y].rooms[r.x].isEnd && endAllowed) || (map[r.y].rooms[r.x].isEntry && entryAllowed)).ToList();
            }

            public List<Vector2Int> PossibleMovementDirections(Vector2Int pos)
            {
                ProxyRoom room = map[pos.y].rooms[pos.x];

                List<Vector2Int> results = new List<Vector2Int>();

                if (room.left == ProxyRoom.DOOR || room.left == ProxyRoom.OPEN)
                {
                    var leftPos = new Vector2Int(pos.x - 1, pos.y);

                    results.Add(leftPos);
                }

                if (room.top == ProxyRoom.DOOR || room.top == ProxyRoom.OPEN)
                {
                    var upPos = new Vector2Int(pos.x, pos.y - 1);

                    results.Add(upPos);

                }

                if (room.bottom == ProxyRoom.DOOR || room.bottom == ProxyRoom.OPEN)
                {
                    var bottomPos = new Vector2Int(pos.x, pos.y + 1);

                    results.Add(bottomPos);

                }

                if (room.right == ProxyRoom.DOOR || room.right == ProxyRoom.OPEN)
                {
                    var rightPos = new Vector2Int(pos.x + 1, pos.y);

                    results.Add(rightPos);

                }
                return results;

            }

            public List<Vector2Int> ConstructRandomPatrolPath(Vector2Int pos, int length)
            {
                if (length <= 1)
                {
                    return new List<Vector2Int>() { pos };
                }

                List<Vector2Int> dirs = PossibleMovementDirections(pos);
                dirs.Randomize();

                if (dirs.Count > 0)
                {

                    var x = new List<Vector2Int>() { pos };
                    x.AddRange(ConstructRandomPatrolPath(dirs[0], length - 1));
                    return x;
                }
                else
                {
                    return new List<Vector2Int>() { pos };
                }
            }

            public Vector3 ConvertLevelPosToWorld(Vector2Int pos)
            {
                return new Vector3(pos.x * 4 + 2, pos.y * -4 + ((template.levelHeight - 1) * 4) + 2);
            }

            public Vector2Int ConvertWorldToLevelPos(Vector3 pos)
            {

                //i * -4 + ((level.template.levelHeight - 1) * 4)

                return new Vector2Int((int)((pos.x) / 4), (int)((template.levelHeight) - (pos.y / 4)));
            }


            public void Print()
            {
                Debug.Log("Printing level");
                for (int i = 0; i < map.Count(); i++)
                {
                    string line = "";
                    for (int j = 0; j < map[i].rooms.Count(); j++)
                    {
                        if (map[i].rooms[j] != null)
                        {

                            if (map[i].rooms[j].isEntry)
                            {
                                line += "[" + map[i].rooms[j].AsPrintableString() + "]";

                            }
                            else if (map[i].rooms[j].isEnd)
                            {
                                line += "{" + map[i].rooms[j].AsPrintableString() + "}";

                            }
                            else if (map[i].rooms[j].isMainChain)
                            {
                                line += "<" + map[i].rooms[j].AsPrintableString() + ">";
                            }
                            else
                            {

                                line += "(" + map[i].rooms[j].AsPrintableString() + ")";
                            }
                        }
                        else
                        {
                            line += "-####-";
                        }
                    }

                    Debug.Log(line);
                }
            }
        }
    }
}