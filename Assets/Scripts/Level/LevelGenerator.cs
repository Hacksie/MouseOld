using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HackedDesign.Level
{
    public abstract class LevelGenerator : ILevelGenerator
    {
        protected const string TopLeft = "tl";
        protected const string TopRight = "tr";
        protected const string BottomLeft = "bl";
        protected const string BottomRight = "br";

        protected float randomChance = 0.75f;
        protected float lineOfSightChance = 0.3f;

        public static ILevelGenerator GetGenerator(LevelGenTemplate template)
        {
            ILevelGenerator generator;

            if (!string.IsNullOrWhiteSpace(template.levelResource))
            {
                generator = new FileLevelGenerator();
            }
            else
            {
                generator = new RandomLevelGenerator();
            }

            return generator;
        }

        public static Level Generate(LevelGenTemplate template) => Generate(template, 0, 0, 0, 0, 0, 0);

        public static Level Generate(LevelGenTemplate template, int difficulty, int enemies, int traps) => Generate(template, 0, 0, 0, difficulty, enemies, traps);

        public static Level Generate(LevelGenTemplate template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            var generator = GetGenerator(template);
            return generator.GenerateLevel(template, length, height, width, difficulty, enemies, traps);
        }


        public Level GenerateLevel(LevelGenTemplate template) => GenerateLevel(template, 0, 0, 0, 0, 0, 0);

        public Level GenerateLevel(LevelGenTemplate template, int difficulty, int enemies, int traps) => GenerateLevel(template, 0, 0, 0, difficulty, enemies, traps);

        public abstract Level GenerateLevel(LevelGenTemplate template, int length, int height, int width, int difficulty, int enemies, int traps);
  
        protected void GenerateElements(Level level)
        {
            for (int y = 0; y < level.map.Count(); y++)
            {
                for (int x = 0; x < level.map[y].rooms.Count(); x++)
                {
                    if (level.map[y].rooms[x] != null)
                    {
                        GenerateRoomElements(level.map[y].rooms[x], ProxyRoom.ObjTypeWall, 1, level.template);

                        if (!level.template.generateRandomProps)
                        {
                            continue;
                        }

                        if (level.map[y].rooms[x].isEntry)
                        {
                            GenerateRoomElements(level.map[y].rooms[x], ProxyRoom.ObjTypeEntry, 1, level.template);
                        }
                        else if (level.map[y].rooms[x].isEnd)
                        {
                            GenerateRoomElements(level.map[y].rooms[x], ProxyRoom.ObjTypeEnd, 1, level.template);
                        }
                        else
                        {
                            GenerateRoomElements(level.map[y].rooms[x], ProxyRoom.ObjTypeRandom, randomChance, level.template);

                            if (!level.trapSpawnLocationList.Any(t => t.levelLocation == new Vector2Int(x, y)))
                            {
                                GenerateRoomElements(level.map[y].rooms[x], ProxyRoom.ObjTypeLineOfSight, lineOfSightChance, level.template);
                            }
                            else
                            {
                                Logger.Log("LevelGenerator", "Skipping line of sight due to trap location");
                            }
                        }
                    }
                }
            }
        }

        protected void GenerateRoomElements(ProxyRoom proxyRoom, string type, float chance, LevelGenTemplate template)
        {
            AddRoomElement(ref proxyRoom.topLeft, TopLeft, proxyRoom.left, proxyRoom.top, type, chance, template);
            AddRoomElement(ref proxyRoom.topRight, TopRight, proxyRoom.right, proxyRoom.top, type, chance, template);
            AddRoomElement(ref proxyRoom.bottomLeft, BottomLeft, proxyRoom.left, proxyRoom.bottom, type, chance, template);
            AddRoomElement(ref proxyRoom.bottomRight, BottomRight, proxyRoom.right, proxyRoom.bottom, type, chance, template);
        }

        protected void AddRoomElement(ref List<Corner> cornerElements, string corner, string wall1, string wall2, string type, float chance, LevelGenTemplate template)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= chance)
            {
                var goList = FindRoomElements(corner, wall1, wall2, type, template).ToList();

                if (goList != null && goList.Count > 0)
                {
                    goList.Randomize();

                    cornerElements.Add(
                        new Corner()
                        {
                            type = type,
                            name = goList[0].name,
                            isTrap = false
                        });
                }
            }
        }

        protected IEnumerable<GameObject> FindRoomElements(string corner, string wall1, string wall2, string type, LevelGenTemplate levelGenTemplate)
        {
            IEnumerable<GameObject> results = null;

            switch (type)
            {
                case ProxyRoom.ObjTypeWall:
                    results = levelGenTemplate.levelElements.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
                case ProxyRoom.ObjTypeEntry:
                    results = levelGenTemplate.startProps.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
                case ProxyRoom.ObjTypeEnd:
                    results = levelGenTemplate.endProps.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
                case ProxyRoom.ObjTypeRandom:
                    results = levelGenTemplate.randomProps.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
                case ProxyRoom.ObjTypeFixed:
                    results = levelGenTemplate.fixedProps.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
                case ProxyRoom.ObjTypeLineOfSight:
                    results = levelGenTemplate.lineOfSightProps.Where(g => g != null && MatchPrefabName(g.name, corner, wall1, wall2));
                    break;
            }

            return results;
        }

        protected bool MatchPrefabName(string prefabName, string corner, string wall1, string wall2)
        {
            string[] nameSplit = prefabName.ToLower().Split('_');
            corner = corner.ToLower();
            wall1 = wall1.ToLower();
            wall2 = wall2.ToLower();

            if (nameSplit.Length != 4)
            {
                Logger.LogError("LevelGenerator", "Invalid prefab name - ", prefabName);
                return false;
            }

            string first = nameSplit[3].Substring(0, 1);
            string second = nameSplit[3].Substring(1, 1);

            return (nameSplit[2] == corner &&
                ((wall1 == ProxyRoom.Open && ProxyRoom.OpenOptions.IndexOf(first) >= 0) ||
                    (wall1 == ProxyRoom.Door && ProxyRoom.DoorOptions.IndexOf(first) >= 0) ||
                    (wall1 == ProxyRoom.Wall && ProxyRoom.WallOptions.IndexOf(first) >= 0) ||
                    (wall1 == ProxyRoom.Exit && ProxyRoom.ExitOptions.IndexOf(first) >= 0) ||
                    (wall1 == ProxyRoom.Entry && ProxyRoom.EntryOptions.IndexOf(first) >= 0)) &&
                ((wall2 == ProxyRoom.Open && ProxyRoom.OpenOptions.IndexOf(second) >= 0) ||
                    (wall2 == ProxyRoom.Door && ProxyRoom.DoorOptions.IndexOf(second) >= 0) ||
                    (wall2 == ProxyRoom.Wall && ProxyRoom.WallOptions.IndexOf(second) >= 0) ||
                    (wall2 == ProxyRoom.Exit && ProxyRoom.ExitOptions.IndexOf(second) >= 0) ||
                    (wall2 == ProxyRoom.Entry && ProxyRoom.EntryOptions.IndexOf(second) >= 0))
            );
        }    
    }
}