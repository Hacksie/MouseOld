using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        private const string DEFAULT_ROOM_START = "wnew_entry";
        private const string TOPLEFT = "tl";
        private const string TOPRIGHT = "tr";
        private const string BOTTOMLEFT = "bl";
        private const string BOTTOMRIGHT = "br";

        [SerializeField]
        private Entities.EntityManager entityManager = null;

        [SerializeField]
        private LevelGenTemplate[] levelGenTemplates = null;

        public Level GenerateLevel(string template)
        {
            return GenerateLevel(template, 0, 0, 0, 0, 0, 0);
        }

        public Level GenerateLevel(string template, int difficulty, int enemies, int cameras)
        {
            return GenerateLevel(template, 0, 0, 0, difficulty, enemies, cameras);
        }

        public Level GenerateLevel(string template, int length, int height, int width, int difficulty, int enemies, int traps)
        {


            if (string.IsNullOrEmpty(template))
            {
                Logger.LogError(name, "no level template set");
                return null;
            }

            if (string.IsNullOrEmpty(name))
            {
                Logger.LogError(name, "no level name set");
                return null;
            }

            Logger.Log(name, "using template - " + template);

            var genTemplate = GetLevelGenTemplate(template);

            if (length > 0)
            {
                genTemplate.levelLength = length;
            }

            if (height > 0)
            {
                genTemplate.levelHeight = height;
            }

            if (width > 0)
            {
                genTemplate.levelWidth = width;
            }

            if (enemies > 0)
            {
                genTemplate.enemyCount = enemies;
            }

            if (traps > 0)
            {
                genTemplate.trapCount = traps;
            }

            if (genTemplate == null)
            {
                Logger.LogError(name, "no level gen template found - ", template);
                return null;
            }

            Logger.Log(name, "generating Level ", genTemplate.levelLength.ToString(), " x ", genTemplate.levelWidth.ToString(), " x ", genTemplate.levelHeight.ToString());

            Level level;

            if (!string.IsNullOrWhiteSpace(genTemplate.levelResource))
            {
                level = LoadLevelFromFile(genTemplate);
                GenerateElements(level, false);
            }
            else
            {
                level = GenerateRandomLevel(genTemplate);
                GenerateEnemySpawns(level);
                GenerateTrapSpawns(level);
                GenerateElements(level, true);

            }
            level.Print();
            return level;
        }

        protected Level LoadLevelFromFile(LevelGenTemplate genTemplate)
        {
            Level level = new Level(genTemplate);
            Logger.Log(this.name, "loading level from file: Levels/" + genTemplate.levelResource + @".json");
            var jsonTextFile = Resources.Load<TextAsset>(@"Levels/" + genTemplate.levelResource);
            if (jsonTextFile == null)
            {
                Debug.LogError(this.name + ": file not loaded");
                return null;
            }

            JsonUtility.FromJsonOverwrite(jsonTextFile.text, level);

            return level;

        }

        protected Level GenerateRandomLevel(LevelGenTemplate genTemplate)
        {
            var level = new Level(genTemplate);
            var position = GenerateStartingLocation(level);

            if (level.length > 1)
            {
                GenerateMainChain(new Vector2Int(position.x, position.y - 1), level, level.length - 1);
            }

            GenerateAuxRooms(level);

            return level;

        }

        LevelGenTemplate GetLevelGenTemplate(string template)
        {
            var genTemplate = levelGenTemplates.FirstOrDefault(t => t.name == template);

            return genTemplate;
        }

        Vector2Int GenerateStartingLocation(Level level)
        {
            Debug.Log(this.name + ": generating Starting Location");

            // Starting at the bottom and going up means we should never create a chain that fails completely and rolls all the way back to the entry
            // This is important!				
            // It also means the player starts at the bottom and plays upwards, which is ideal
            var position = new Vector2Int((level.template.levelWidth - 1) / 2, (level.template.levelHeight - 1));
            level.map[position.y].rooms[position.x] = GenerateEntryRoom(level);
            level.playerSpawn = new Spawn()
            {
                type = Spawn.ENTITY_TYPE_PLAYER,
                name = "Mouse",
                levelLocation = position
            };
            return position;
        }

        bool GenerateMainChain(Vector2Int newLocation, Level level, int lengthRemaining)
        {
            if (lengthRemaining == 0)
            {
                return true;
            }

            Debug.Log(this.name + ": generating main chain");

            // The end room is considered special
            if (lengthRemaining == 1)
            {
                Logger.Log(this.name, "end of main chain");

                level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.Wall }, true, level); // Place a new tile here
                level.map[newLocation.y].rooms[newLocation.x].isEnd = true;
                return true;
            }

            level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.Open, ProxyRoom.Door }, true, level); // Place a new tile here 

            List<Vector2Int> directions = PossibleBuildDirections(newLocation, level);

            directions.Randomize();

            bool result = false;

            // Iterate over potential directions from here
            for (int i = 0; i < directions.Count; i++)
            {

                result = GenerateMainChain(directions[i], level, lengthRemaining - 1);

                if (result) // If the chain is okay, don't need to try any new directions. We could probably return true here
                {
                    break;
                }
            }

            // If we didn't complete the chain, abandon this location
            // Fixme: we probably have to change a side because of this
            if (!result)
            {
                Debug.Log(this.name + ": abandoning chain, roll back one step");
            }

            return result;
        }

        List<Vector2Int> PossibleBuildDirections(Vector2Int pos, Level level)
        {
            ProxyRoom room = level.map[pos.y].rooms[pos.x];

            List<Vector2Int> results = new List<Vector2Int>();

            if (room.left == ProxyRoom.Door || room.left == ProxyRoom.Open)
            {
                var leftPos = new Vector2Int(pos.x - 1, pos.y);
                if (!PositionHasRoom(leftPos, level))
                {
                    results.Add(leftPos);
                }
            }

            if (room.top == ProxyRoom.Door || room.top == ProxyRoom.Open)
            {
                var upPos = new Vector2Int(pos.x, pos.y - 1);
                if (!PositionHasRoom(upPos, level))
                {
                    results.Add(upPos);
                }
            }

            if (room.bottom == ProxyRoom.Door || room.bottom == ProxyRoom.Open)
            {
                var bottomPos = new Vector2Int(pos.x, pos.y + 1);
                if (!PositionHasRoom(bottomPos, level))
                {
                    results.Add(bottomPos);
                }
            }

            if (room.right == ProxyRoom.Door || room.right == ProxyRoom.Open)
            {
                var rightPos = new Vector2Int(pos.x + 1, pos.y);
                if (!PositionHasRoom(rightPos, level))
                {
                    results.Add(rightPos);
                }
            }

            return results;
        }

        ProxyRoom GenerateEntryRoom(Level level)
        {

            string start = string.IsNullOrEmpty(level.template.startingRoomString) ? DEFAULT_ROOM_START : level.template.startingRoomString;

            ProxyRoom res = RoomFromString(start);
            res.isEntry = true;
            res.isMainChain = true;
            res.isNearEntry = true;

            return res;
        }

        ProxyRoom RoomFromString(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            ProxyRoom response = new ProxyRoom();
            string[] splitString = str.Split('_');

            if (splitString.Length < 1)
            {
                return null;
            }

            if (splitString.Length > 0)
            {

                response.left = splitString[0].Substring(0, 1);
                response.top = splitString[0].Substring(1, 1);
                response.bottom = splitString[0].Substring(2, 1);
                response.right = splitString[0].Substring(3, 1);
            }

            if (splitString.Length > 1)
            {
                response.isEntry = splitString[1] == "entry";
                response.isEnd = splitString[1] == "end";
            }

            return response;
        }

        void GenerateAuxRooms(Level level)
        {
            Debug.Log(this.name + ": generating Aux Rooms");
            bool newRooms = true;

            // iterate through every position, checking for neighbours and creating rooms accordingly. 
            // Keep iterating until we stop creating rooms				
            while (newRooms)
            {
                newRooms = false;
                for (int i = 0; i < level.template.levelHeight; i++)
                {
                    for (int j = 0; j < level.template.levelWidth; j++)
                    {
                        if ((level.map[i].rooms[j] != null))
                        {
                            Vector2Int pos = new Vector2Int(j, i);
                            List<Vector2Int> dirs = PossibleBuildDirections(pos, level);

                            foreach (Vector2Int location in dirs)
                            {
                                newRooms = true;
                                level.map[location.y].rooms[location.x] = GenerateRoom(location, new List<string>() {
                                        ProxyRoom.Open, ProxyRoom.Door, ProxyRoom.Wall, ProxyRoom.Wall,ProxyRoom.Wall

                                    }, false, level);
                            }
                        }
                    }
                }
            }
        }

        ProxyRoom GenerateRoom(Vector2Int location, List<string> freeChoiceSides, bool isMainChain, Level level)
        {
            // Get Top Side
            List<string> tops = PossibleTopSides(location, freeChoiceSides, level);
            List<string> lefts = PossibleLeftSides(location, freeChoiceSides, level);
            List<string> bottoms = PossibleBottomSides(location, freeChoiceSides, level);
            List<string> rights = PossibleRightSides(location, freeChoiceSides, level);

            tops.Randomize();
            lefts.Randomize();
            bottoms.Randomize();
            rights.Randomize();

            return new ProxyRoom()
            {
                isEntry = false,
                isEnd = false,
                isMainChain = isMainChain,
                top = tops[0],
                left = lefts[0],
                bottom = bottoms[0],
                right = rights[0],
                isNearEntry = IsNearEntry(location, level)
            };

        }

        bool IsNearEntry(Vector2Int location, Level level)
        {
            Vector2Int[] surround = new Vector2Int[9];
            surround[0] = location + new Vector2Int(-1, -1);
            surround[1] = location + new Vector2Int(0, -1);
            surround[2] = location + new Vector2Int(1, -1);
            surround[3] = location + new Vector2Int(-1, 0);
            surround[4] = location + new Vector2Int(0, 0);
            surround[5] = location + new Vector2Int(1, 0);
            surround[6] = location + new Vector2Int(-1, 1);
            surround[7] = location + new Vector2Int(0, 1);
            surround[8] = location + new Vector2Int(1, 1);

            for (int i = 0; i < 9; i++)
            {
                // Check bounds
                if (surround[i].x < 0 || surround[i].y < 0 || surround[i].x >= level.template.levelWidth || surround[i].y >= level.template.levelHeight || level.map[surround[i].y].rooms[surround[i].x] == null)
                    continue;

                if (level.map[surround[i].y].rooms[surround[i].x].isEntry)
                    return true;
            }

            return false;
        }

        List<string> PossibleTopSides(Vector2Int pos, List<string> freeChoice, Level level)
        {
            List<string> sides = new List<string>();

            // If the side would lead out of the level, the side has to be wall
            if (pos.y <= 0)
            {
                sides.Add(ProxyRoom.Wall);
                return sides;
            }

            // Get what's at the position 
            ProxyRoom room = level.map[pos.y - 1].rooms[pos.x];

            // If there's nothing then we're free to do anything
            if (room == null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the top
            sides.Add(room.bottom);
            return sides;
        }

        List<string> PossibleBottomSides(Vector2Int pos, List<string> freeChoice, Level level)
        {
            List<string> sides = new List<string>();

            // If the side would lead out of the level, the side has to be wall
            if (pos.y >= (level.template.levelHeight - 1))
            {
                sides.Add(ProxyRoom.Wall);
                return sides;
            }

            // Get what's at the position 
            ProxyRoom room = level.map[pos.y + 1].rooms[pos.x];

            // If there's nothing then we're free to do anything
            if (room == null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the bottom
            sides.Add(room.top);
            return sides;
        }

        List<string> PossibleLeftSides(Vector2Int pos, List<string> freeChoice, Level level)
        {
            List<string> sides = new List<string>();

            // If the side would lead out of the level, the side has to be wall
            if (pos.x <= 0)
            {
                sides.Add(ProxyRoom.Wall);
                return sides;
            }

            // Get what's at the position 
            ProxyRoom room = level.map[pos.y].rooms[pos.x - 1];

            // If there's nothing then we're free to do anything
            if (room == null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the left
            sides.Add(room.right);
            return sides;
        }

        List<string> PossibleRightSides(Vector2Int position, List<string> freeChoice, Level level)
        {
            List<String> sides = new List<string>();

            // If the side would lead out of the level, the side has to be wall
            if (position.x >= (level.template.levelWidth - 1))
            {
                sides.Add(ProxyRoom.Wall);
                return sides;
            }

            // Get what's at the position 
            ProxyRoom room = level.map[position.y].rooms[position.x + 1];

            // If there's nothing then we're free to do anything
            if (room == null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the right
            sides.Add(room.left);
            return sides;
        }

        bool PositionHasRoom(Vector2Int pos, Level level)
        {
            if (pos.x >= level.template.levelWidth || pos.y >= level.template.levelHeight || pos.x < 0 || pos.y < 0)
            {
                return true; // If we go outside the level, pretend we already put a room here
            }
            return (!(level.map[pos.y].rooms[pos.x] == null));
        }


        void GenerateElements(Level level, bool genProps)
        {
            for (int i = 0; i < level.map.Count(); i++)
            {
                for (int j = 0; j < level.map[i].rooms.Count(); j++)
                {
                    if (level.map[i].rooms[j] != null)
                    {
                        GenerateRoomElements(level.map[i].rooms[j], ProxyRoom.ObjTypeWall, level.template, false);

                        if (genProps && level.template.generateProps)
                        {
                            if (level.map[i].rooms[j].isEntry)
                            {

                                GenerateRoomElements(level.map[i].rooms[j], ProxyRoom.ObjTypeEntry, level.template, false);
                            }
                            else if (level.map[i].rooms[j].isEnd)
                            {
                                GenerateRoomElements(level.map[i].rooms[j], ProxyRoom.ObjTypeEnd, level.template, false);
                            }
                            else
                            {
                                GenerateRoomElements(level.map[i].rooms[j], ProxyRoom.ObjTypeRandom, level.template, true);
                            }
                        }
                    }
                }
            }
        }

        void GenerateRoomElements(ProxyRoom proxyRoom, string type, LevelGenTemplate template, bool allowTraps)
        {
            //string roomString = proxyRoom.AsPrintableString();
            List<GameObject> goBLList;
            List<GameObject> goBRList;
            List<GameObject> goTLList;
            List<GameObject> goTRList;

            // TL
            goTLList = FindRoomElements(TOPLEFT, proxyRoom.left, proxyRoom.top, type, template).ToList();
            goTLList.Randomize();

            if (goTLList.FirstOrDefault() != null)
            {
                proxyRoom.topLeft.Add(
                    new Corner()
                    {
                        type = type,
                        name = goTLList[0].name,
                        isTrap = false
                    });
            }

            // TR
            goTRList = FindRoomElements(TOPRIGHT, proxyRoom.right, proxyRoom.top, type, template).ToList();
            goTRList.Randomize();

            if (goTRList.FirstOrDefault() != null)
            {
                proxyRoom.topRight.Add(
                    new Corner()
                    {
                        type = type,
                        name = goTRList[0].name,
                        isTrap = false
                    });
            }

            // BL
            goBLList = FindRoomElements(BOTTOMLEFT, proxyRoom.left, proxyRoom.bottom, type, template).ToList();
            goBLList.Randomize();

            if (goBLList.FirstOrDefault() != null)
            {
                proxyRoom.bottomLeft.Add(
                    new Corner()
                    {
                        type = type,
                        name = goBLList[0].name,
                        isTrap = false
                    });
            }

            // BR
            goBRList = FindRoomElements(BOTTOMRIGHT, proxyRoom.right, proxyRoom.bottom, type, template).ToList();
            goBRList.Randomize();

            if (goBRList.FirstOrDefault() != null)
            {

                proxyRoom.bottomRight.Add(
                    new Corner()
                    {
                        type = type,
                        name = goBRList[0].name,
                        isTrap = false
                    });
            }
        }

        /// <summary>
        /// Return a list of prefabs that fit the level gen's room corner requirements
        /// </summary>
        /// <param name="corner"></param>
        /// <param name="wall1"></param>
        /// <param name="wall2"></param>
        /// <param name="type"></param>
        /// <param name="levelGenTemplate"></param>
        /// <returns></returns>
        IEnumerable<GameObject> FindRoomElements(string corner, string wall1, string wall2, string type, LevelGenTemplate levelGenTemplate)
        {

            IEnumerable<GameObject> results = null;

            switch (type)
            {
                case ProxyRoom.ObjTypeWall:
                    results = levelGenTemplate.levelElements.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

                case ProxyRoom.ObjTypeEntry:
                    results = levelGenTemplate.startProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

                case ProxyRoom.ObjTypeEnd:
                    results = levelGenTemplate.endProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

                case ProxyRoom.ObjTypeTrap:
                    results = levelGenTemplate.trapProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

                case ProxyRoom.ObjTypeRandom:
                    results = levelGenTemplate.randomProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

                case ProxyRoom.ObjTypeFixed:
                    results = levelGenTemplate.fixedProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                    break;

            }

            return results;
        }

        /// <summary>
        /// Take a prefab name, split it into its components and match it against the level generator's requirements
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="corner"></param>
        /// <param name="wall1"></param>
        /// <param name="wall2"></param>
        /// <returns></returns>
        bool MatchSpriteName(string prefabName, string corner, string wall1, string wall2)
        {
            string[] nameSplit = prefabName.ToLower().Split('_');

            if (nameSplit.Length != 4)
            {
                Logger.LogError(name, "invalid prefab name - " + prefabName);
                return false;
            }

            string open = ProxyRoom.Open + ProxyRoom.Any + ProxyRoom.OpenOrDoor + ProxyRoom.OpenOrWall;
            string door = ProxyRoom.Door + ProxyRoom.Any + ProxyRoom.OpenOrDoor + ProxyRoom.DoorOrWall;
            string wall = ProxyRoom.Wall + ProxyRoom.Any + ProxyRoom.OpenOrWall + ProxyRoom.DoorOrWall;
            string exit = ProxyRoom.Exit + door;
            string entry = ProxyRoom.Entry + door;

            string first = nameSplit[3].Substring(0, 1);
            string second = nameSplit[3].Substring(1, 1);

            return (nameSplit[2] == corner.ToLower() &&
                ((wall1.ToLower() == "o" && open.IndexOf(first) >= 0) ||
                    (wall1.ToLower() == "d" && door.IndexOf(first) >= 0) ||
                    (wall1.ToLower() == "w" && wall.IndexOf(first) >= 0) ||
                    (wall1.ToLower() == "e" && exit.IndexOf(first) >= 0) ||
                    (wall1.ToLower() == "n" && entry.IndexOf(first) >= 0)) &&
                ((wall2.ToLower() == "o" && open.IndexOf(second) >= 0) ||
                    (wall2.ToLower() == "d" && door.IndexOf(second) >= 0) ||
                    (wall2.ToLower() == "w" && wall.IndexOf(second) >= 0) ||
                    (wall2.ToLower() == "e" && exit.IndexOf(second) >= 0) ||
                    (wall2.ToLower() == "n" && entry.IndexOf(second) >= 0))
            );
        }

        void GenerateEnemySpawns(Level level)
        {
            var candidates = new List<Vector2Int>();
            level.enemySpawnLocationList = new List<Spawn>();

            for (int i = 0; i < level.map.Count(); i++)
            {
                for (int j = 0; j < level.map[i].rooms.Count(); j++)
                {
                    if (level.map[i].rooms[j] != null && !level.map[i].rooms[j].isNearEntry && !level.map[i].rooms[j].isEnd)
                    {
                        candidates.Add(new Vector2Int(j, i));
                    }
                }
            }

            candidates.Randomize();

            var enemyList = level.template.enemies;

            if (enemyList.Count == 0)
            {
                Logger.Log(name, "No enemies in template");
                return;
            }

            foreach (var candidate in candidates.Take(level.template.enemyCount))
            {

                //int rand = UnityEngine.Random.Range(0, enemyList);

                var enemy = enemyList[UnityEngine.Random.Range(0, enemyList.Count)];

                //FIXME: check the enemy is valid

                level.enemySpawnLocationList.Add(
                    new Spawn()
                    {
                        type = Spawn.ENTITY_TYPE_ENEMY,
                        name = enemy,
                        levelLocation = candidate,
                        worldOffset = Vector2.zero
                    }
                );
            }
        }

        void GenerateTrapSpawns(Level level)
        {
            var candidates = new List<Vector2Int>();
            level.trapSpawnLocationList = new List<Spawn>();

            for (int i = 0; i < level.map.Count(); i++)
            {
                for (int j = 0; j < level.map[i].rooms.Count(); j++)
                {
                    if (level.map[i].rooms[j] != null && !level.map[i].rooms[j].isNearEntry)
                    {
                        candidates.Add(new Vector2Int(j, i));
                    }
                }
            }

            candidates.Randomize();

            foreach (var candidate in candidates.Take(level.template.enemyCount))
            {

                //int rand = UnityEngine.Random.Range(0, entityManager.enemies.Count);

                level.trapSpawnLocationList.Add(
                    new Spawn()
                    {
                        type = Spawn.ENTITY_TYPE_TRAP,
                        name = entityManager.enemies[UnityEngine.Random.Range(0, entityManager.enemies.Count)].name,
                        levelLocation = candidate,
                        worldOffset = Vector2.zero
                    }
                );
            }
        }
    }

}