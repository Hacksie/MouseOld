using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Level
    {
        public class LevelGenerator : MonoBehaviour
        {
            const string DEFAULT_ROOM_START = "wnew_entry";
            const string TOPLEFT = "tl";
            const string TOPRIGHT = "tr";
            const string BOTTOMLEFT = "bl";
            const string BOTTOMRIGHT = "br";

            [SerializeField]
            private Entity.EntityManager entityManager = null;

            [SerializeField]
            private LevelGenTemplate[] levelGenTemplates = null;

            public Level GenerateLevel(string template)
            {
                return GenerateLevel(template, 0, 0, 0, 0, 0, 0);
            }

            public Level GenerateLevel(string template, int length, int height, int width, int difficulty, int enemies, int cameras)
            {
                Debug.Log(this.name + ": generating Level");

                if (string.IsNullOrEmpty(template))
                {
                    Debug.LogError(this.name + ": no level template set");
                    return null;
                }

                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogError(this.name + ": no level name set");
                    return null;
                }

                Debug.Log(this.name + ": using template - " + template);

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

                genTemplate.enemies = enemies;
                genTemplate.traps = cameras;

                if (genTemplate == null)
                {
                    Debug.LogError(this.name + ": no level gen template found: " + template);
                    return null;
                }

                Level level;

                if (!string.IsNullOrWhiteSpace(genTemplate.levelResource))
                {
                    level = LoadLevelFromFile(genTemplate);
                    GenerateEntities(level, false);
                }
                else
                {
                    level = GenerateRandomLevel(genTemplate);
                    GenerateEnemySpawns(level, difficulty);
                    GenerateTrapSpawns(level, difficulty);
                    GenerateEntities(level, true);
                }
                level.Print();
                return level;

            }

            protected Level LoadLevelFromFile(LevelGenTemplate genTemplate)
            {
                Level level = new Level(genTemplate);
                Debug.Log(this.name + ": loading level from file: Levels/" + genTemplate.levelResource + @".json");
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
                Vector2Int position = new Vector2Int((level.template.levelWidth - 1) / 2, (level.template.levelHeight - 1));
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
                    Debug.Log(this.name + ": end of main chain");

                    level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.WALL }, true, level); // Place a new tile here
                    level.map[newLocation.y].rooms[newLocation.x].isEnd = true;
                    return true;
                }

                level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.OPEN, ProxyRoom.DOOR }, true, level); // Place a new tile here 

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

                if (room.left == ProxyRoom.DOOR || room.left == ProxyRoom.OPEN)
                {
                    var leftPos = new Vector2Int(pos.x - 1, pos.y);
                    if (!PositionHasRoom(leftPos, level))
                    {
                        results.Add(leftPos);
                    }
                }

                if (room.top == ProxyRoom.DOOR || room.top == ProxyRoom.OPEN)
                {
                    var upPos = new Vector2Int(pos.x, pos.y - 1);
                    if (!PositionHasRoom(upPos, level))
                    {
                        results.Add(upPos);
                    }
                }

                if (room.bottom == ProxyRoom.DOOR || room.bottom == ProxyRoom.OPEN)
                {
                    var bottomPos = new Vector2Int(pos.x, pos.y + 1);
                    if (!PositionHasRoom(bottomPos, level))
                    {
                        results.Add(bottomPos);
                    }
                }

                if (room.right == ProxyRoom.DOOR || room.right == ProxyRoom.OPEN)
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
                                        ProxyRoom.OPEN, ProxyRoom.DOOR, ProxyRoom.WALL, ProxyRoom.WALL,ProxyRoom.WALL

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
                    if (surround[i].x < 0)
                        continue;

                    if (surround[i].y < 0)
                        continue;

                    if (surround[i].x >= level.template.levelWidth)
                        continue;

                    if (surround[i].y >= level.template.levelHeight)
                        continue;

                    // Check for a room
                    if (level.map[surround[i].y].rooms[surround[i].x] == null)
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
                    sides.Add(ProxyRoom.WALL);
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
                    sides.Add(ProxyRoom.WALL);
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
                    sides.Add(ProxyRoom.WALL);
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
                    sides.Add(ProxyRoom.WALL);
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


            void GenerateEntities(Level level, bool genProps)
            {

                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {

                        Vector3 pos = new Vector3(j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

                        if (level.map[i].rooms[j] != null)
                        {

                            GenerateRoomEntities(level.map[i].rooms[j], ProxyRoom.OBJ_TYPE_WALL, level.template, false);

                            if (genProps)
                            {

                                if (level.map[i].rooms[j].isEntry)
                                {

                                    GenerateRoomEntities(level.map[i].rooms[j], ProxyRoom.OBJ_TYPE_ENTRY, level.template, false);
                                }
                                else if (level.map[i].rooms[j].isEnd)
                                {
                                    GenerateRoomEntities(level.map[i].rooms[j], ProxyRoom.OBJ_TYPE_END, level.template, false);
                                }
                                else
                                {
                                    GenerateRoomEntities(level.map[i].rooms[j], ProxyRoom.OBJ_TYPE_RANDOM, level.template, true);
                                }
                            }

                            //GenerateRoomEntities(level.map[i].rooms[j], ProxyRoom.OBJ_TYPE_FIXED, level.template, true);

                        }
                    }
                }


            }

            void GenerateRoomEntities(ProxyRoom proxyRoom, string type, LevelGenTemplate template, bool allowTraps)
            {
                //string roomString = proxyRoom.AsPrintableString();
                List<GameObject> goBLList;
                List<GameObject> goBRList;
                List<GameObject> goTLList;
                List<GameObject> goTRList;

                // TL
                goTLList = FindRoomObject(TOPLEFT, proxyRoom.left, proxyRoom.top, type, template).ToList();
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
                goTRList = FindRoomObject(TOPRIGHT, proxyRoom.right, proxyRoom.top, type, template).ToList();
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
                goBLList = FindRoomObject(BOTTOMLEFT,proxyRoom.left, proxyRoom.bottom, type, template).ToList();
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
                goBRList = FindRoomObject(BOTTOMRIGHT,proxyRoom.right, proxyRoom.bottom, type, template).ToList();
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

            IEnumerable<GameObject> FindRoomObject(string corner, string wall1, string wall2, string type, LevelGenTemplate levelGenTemplate)
            {

                IEnumerable<GameObject> results = null;

                switch (type)
                {
                    case ProxyRoom.OBJ_TYPE_WALL:
                        results = levelGenTemplate.levelElements.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                    case ProxyRoom.OBJ_TYPE_ENTRY:
                        results = levelGenTemplate.startProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                    case ProxyRoom.OBJ_TYPE_END:
                        results = levelGenTemplate.endProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                    case ProxyRoom.OBJ_TYPE_TRAP:
                        results = levelGenTemplate.trapProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                    case ProxyRoom.OBJ_TYPE_RANDOM:
                        results = levelGenTemplate.randomProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                    case ProxyRoom.OBJ_TYPE_FIXED:
                        results = levelGenTemplate.fixedProps.Where(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
                        break;

                }

                return results;
            }

            bool MatchSpriteName(string name, string corner, string wall1, string wall2)
            {
                string[] nameSplit = name.ToLower().Split('_');

                if (nameSplit.Length != 4)
                {
                    Debug.LogError(this.name + ": invalid sprite name");
                    return false;
                }

                string open = "oaxy";
                string door = "daxz";
                string wall = "wayz";
                string exit = "edaxy";
                string entry = "ndaxy";

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

            void GenerateEnemySpawns(Level level, int difficulty)
            {
                List<Vector2Int> candidates = new List<Vector2Int>();
                level.enemySpawnLocationList = new List<Spawn>();

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

                var enemyList = difficulty == 0? level.template.easyEnemies : difficulty == 1? level.template.mediumEnemies : level.template.hardEnemies;

                foreach (var candidate in candidates.Take(level.template.enemies))
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

            void GenerateTrapSpawns(Level level, int difficulty)
            {
                List<Vector2Int> candidates = new List<Vector2Int>();
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

                foreach (var candidate in candidates.Take(level.template.enemies))
                {

                    int rand = UnityEngine.Random.Range(0, entityManager.enemies.Count);

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
}