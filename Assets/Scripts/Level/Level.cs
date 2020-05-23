using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HackedDesign.Level
{
    [System.Serializable]
    public class Level
    {
        public LevelGenTemplate template;
        public ProxyRow[] map;
        //public ProxyRoom[,] proxyLevel;
        public int length;
        public Spawn playerSpawn;
        public List<Spawn> npcSpawnLocationList;
        public List<Spawn> enemySpawnLocationList;
        public List<Spawn> trapSpawnLocationList;
        public Timer timer = new Timer();
        public bool completed = false;
        public float startTime = 0;
        public int infoCollected = 0;
        public int alertLevel = 0;
        public int maxInfo = 10;
        public int completeCredits = 500;
        public int creditsCollected = 0;

        public Level(LevelGenTemplate template)
        {
            this.template = template;
            this.length = CapLevelLength(template.levelLength, template.levelWidth, template.levelHeight);

            map = new ProxyRow[template.levelHeight];
            for (int row = 0; row < template.levelHeight; row++)
            {
                map[row] = new ProxyRow();
                map[row].rooms = new ProxyRoom[template.levelWidth];

                for (int col = 0; col < template.levelWidth; col++)
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

            if (room.left == ProxyRoom.Door || room.left == ProxyRoom.Open)
            {
                var leftPos = new Vector2Int(pos.x - 1, pos.y);

                results.Add(leftPos);
            }

            if (room.top == ProxyRoom.Door || room.top == ProxyRoom.Open)
            {
                var upPos = new Vector2Int(pos.x, pos.y - 1);

                results.Add(upPos);

            }

            if (room.bottom == ProxyRoom.Door || room.bottom == ProxyRoom.Open)
            {
                var bottomPos = new Vector2Int(pos.x, pos.y + 1);

                results.Add(bottomPos);

            }

            if (room.right == ProxyRoom.Door || room.right == ProxyRoom.Open)
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

        public Vector2 ConvertLevelPosToWorld(Vector2Int pos)
        {
            return new Vector2(pos.x * template.spanHorizontal + (template.spanHorizontal / 2), pos.y * -template.spanVertical + ((template.levelHeight - 1) * template.spanVertical) + (template.spanVertical / 2));
        }

        public Vector2Int ConvertWorldToLevelPos(Vector2 pos)
        {
            return new Vector2Int((int)((pos.x) / template.spanHorizontal), (int)((template.levelHeight) - (pos.y / template.spanVertical)));
        }


        public void Print()
        {
            Logger.Log("Level", "Printing level");
            for (int i = 0; i < map.Count(); i++)
            {
                string line = "";
                for (int j = 0; j < map[i].rooms.Count(); j++)
                {
                    if (map[i].rooms[j] != null)
                    {

                        if (map[i].rooms[j].isEntry)
                        {
                            line += "[" + map[i].rooms[j].ToString() + "]";

                        }
                        else if (map[i].rooms[j].isEnd)
                        {
                            line += "{" + map[i].rooms[j].ToString() + "}";

                        }
                        else if (map[i].rooms[j].isMainChain)
                        {
                            line += "<" + map[i].rooms[j].ToString() + ">";
                        }
                        else
                        {

                            line += "-" + map[i].rooms[j].ToString() + "-";
                        }
                    }
                    else
                    {
                        line += "-####-";
                    }
                }

                Logger.Log("Level", line);
            }
        }
    }

    [Serializable]
    public struct Spawn
    {
        public const string ENTITY_TYPE_PLAYER = "player";
        public const string ENTITY_TYPE_NPC = "npc";
        public const string ENTITY_TYPE_ENEMY = "enemy";
        public const string ENTITY_TYPE_TRAP = "trap";

        public string type;
        public string name;
        public Vector2Int levelLocation;
        public Vector2 worldOffset;
        public int difficulty;
    }

}