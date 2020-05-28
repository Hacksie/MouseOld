using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HackedDesign.Level
{
    public class RandomLevelGenerator : LevelGenerator
    {
        protected const string DefaultRoomStart = "wnew_entry";

        public override Level GenerateLevel(LevelGenTemplate template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            if (template is null)
            {
                Logger.LogError("RandomLevelGenerator", "No level template set");
                return null;
            }

            Logger.Log("RandomLevelGenerator", "Using template - ", template.name);

            template.levelLength = length > 0 ? length : template.levelLength;
            template.levelHeight = height > 0 ? height : template.levelHeight;
            template.levelWidth = width > 0 ? width : template.levelWidth;
            template.enemyCount = enemies > 0 ? enemies : template.enemyCount;
            template.trapCount = traps > 0 ? traps : template.trapCount;

            Logger.Log("RandomLevelGenerator", "Generating Level ", template.levelLength.ToString(), " x ", template.levelWidth.ToString(), " x ", template.levelHeight.ToString());

            Level level;

            level = GenerateRandomLevel(template);
            GenerateEnemySpawns(level);
            GenerateTrapSpawns(level);
            GenerateElements(level);

            level.Print();
            return level;
        }

        protected Level GenerateRandomLevel(LevelGenTemplate template)
        {
            if (template is null)
            {
                Logger.LogError("RandomLevelGenerator", "Template not set");
                return null;
            }

            var level = new Level(template);
            var position = GenerateStartingLocation(level);

            if (level.length > 1)
            {
                GenerateMainChain(new Vector2Int(position.x, position.y - 1), level, level.length - 1);
            }

            GenerateAuxRooms(level);

            level.Print();

            return level;
        }

        private Vector2Int GenerateStartingLocation(Level level)
        {
            Logger.Log("RandomLevelGenerator", "Generating Starting Location");

            // Starting at the bottom and going up means we should never create a chain that fails completely and rolls all the way back to the entry
            // This is important!				
            // It also means the player starts at the bottom and plays upwards, which is ideal
            var position = new Vector2Int((level.template.levelWidth - 1) / 2, (level.template.levelHeight - 1));
            level.map[position.y].rooms[position.x] = RoomFromString(string.IsNullOrEmpty(level.template.startingRoomString) ? DefaultRoomStart : level.template.startingRoomString, true, true, true);
            level.playerSpawn = new Spawn()
            {
                type = Spawn.ENTITY_TYPE_PLAYER,
                name = "Mouse",
                levelLocation = position
            };
            return position;
        }

        private bool GenerateMainChain(Vector2Int newLocation, Level level, int lengthRemaining)
        {
            if (lengthRemaining == 0)
            {
                return true;
            }

            Logger.Log("RandomLevelGenerator", "Generating main chain");

            // The end room is considered special
            if (lengthRemaining == 1)
            {
                Logger.Log("RandomLevelGenerator", "End of main chain");
                level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.Wall }, true, level); // Place a new tile here
                level.map[newLocation.y].rooms[newLocation.x].isEnd = true;
                return true;
            }

            level.map[newLocation.y].rooms[newLocation.x] = GenerateRoom(newLocation, new List<string>() { ProxyRoom.Open, ProxyRoom.Door }, true, level); // Place a new tile here 

            var directions = PossibleBuildDirections(newLocation, level);
            directions.Randomize();

            bool result = false;
            // Iterate over potential directions from here
            foreach (var direction in directions)
            {
                result = GenerateMainChain(direction, level, lengthRemaining - 1);
                if (result) // If the chain is okay, don't need to try any new directions. We could probably return true here
                {
                    break;
                }
            }

            // If we didn't complete the chain, abandon this location
            // Fixme: we probably have to change a side because of this
            if (!result)
            {
                Logger.Log("RandomLevelGenerator", "Abandoning chain, rolling back one step");
            }

            return result;
        }

        private List<Vector2Int> PossibleBuildDirections(Vector2Int pos, Level level)
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

        private ProxyRoom RoomFromString(string str, bool isEntry, bool isMainChain, bool isNearEntry)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            ProxyRoom response = new ProxyRoom()
            {
                isEntry = isEntry,
                isNearEntry = isNearEntry,
                isMainChain = isMainChain
            };

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
                response.isEntry = splitString[1] == ProxyRoom.ObjTypeEntry;
                response.isEnd = splitString[1] == ProxyRoom.ObjTypeEnd;
            }

            return response;
        }

        private void GenerateAuxRooms(Level level)
        {
            Logger.Log("RandomLevelGenerator", "Generating Aux Rooms");
            bool newRooms = true;

            // iterate through every position, checking for neighbours and creating rooms accordingly. 
            // Keep iterating until we stop creating rooms				
            while (newRooms)
            {
                newRooms = false;
                for (int y = 0; y < level.template.levelHeight; y++)
                {
                    for (int x = 0; x < level.template.levelWidth; x++)
                    {
                        if ((level.map[y].rooms[x] != null))
                        {
                            Vector2Int pos = new Vector2Int(x, y);
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

        private ProxyRoom GenerateRoom(Vector2Int location, List<string> freeChoiceSides, bool isMainChain, Level level)
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

        private bool IsNearEntry(Vector2Int location, Level level)
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

        private List<string> PossibleTopSides(Vector2Int pos, List<string> freeChoice, Level level)
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
            if (room is null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the top
            sides.Add(room.bottom);
            return sides;
        }

        private List<string> PossibleBottomSides(Vector2Int pos, List<string> freeChoice, Level level)
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
            if (room is null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the bottom
            sides.Add(room.top);
            return sides;
        }

        private List<string> PossibleLeftSides(Vector2Int pos, List<string> freeChoice, Level level)
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
            if (room is null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the left
            sides.Add(room.right);
            return sides;
        }

        private List<string> PossibleRightSides(Vector2Int position, List<string> freeChoice, Level level)
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
            if (room is null)
            {
                return freeChoice;
            }

            // Otherwise, match what's currently on the right
            sides.Add(room.left);
            return sides;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool PositionHasRoom(Vector2Int pos, Level level) => ((pos.x >= level.template.levelWidth || pos.y >= level.template.levelHeight || pos.x < 0 || pos.y < 0) || (level.map[pos.y].rooms[pos.x] != null));

        private void GenerateEnemySpawns(Level level)
        {
            var candidates = new List<Vector2Int>();
            level.enemySpawnLocationList = new List<Spawn>();

            for (int y = 0; y < level.map.Count(); y++)
            {
                for (int x = 0; x < level.map[y].rooms.Count(); x++)
                {
                    if (level.map[y].rooms[x] != null && !level.map[y].rooms[x].isNearEntry && !level.map[y].rooms[x].isEnd)
                    {
                        candidates.Add(new Vector2Int(x, y));
                    }
                }
            }

            candidates.Randomize();

            var enemyList = level.template.enemies;

            if (enemyList.Count == 0)
            {
                Logger.Log("RandomLevelGenerator", "No enemies in template");
                return;
            }

            foreach (var candidate in candidates.Take(level.template.enemyCount))
            {
                var enemy = enemyList[UnityEngine.Random.Range(0, enemyList.Count)];


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

        private void GenerateTrapSpawns(Level level)
        {
            var trapList = level.template.traps;

            if (trapList.Count == 0)
            {
                Logger.Log("RandomLevelGenerator", "No enemies in template");
                return;
            }

            var candidates = new List<Vector2Int>();
            level.trapSpawnLocationList = new List<Spawn>();

            for (int y = 0; y < level.map.Count(); y++)
            {
                for (int x = 0; x < level.map[y].rooms.Count(); x++)
                {
                    if (level.map[y].rooms[x] != null && !level.map[y].rooms[x].isNearEntry && !level.map[y].rooms[x].isEnd)
                    {
                        candidates.Add(new Vector2Int(x, y));
                    }
                }
            }

            candidates.Randomize();

            foreach (var candidate in candidates.Take(level.template.enemyCount))
            {
                var trap = trapList[UnityEngine.Random.Range(0, trapList.Count)];

                level.trapSpawnLocationList.Add(
                    new Spawn()
                    {
                        type = Spawn.ENTITY_TYPE_ENEMY,
                        name = trap,
                        levelLocation = candidate,
                        worldOffset = Vector2.zero
                    }
                );

            }
        }
    }
}