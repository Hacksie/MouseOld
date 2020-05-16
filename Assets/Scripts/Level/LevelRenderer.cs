using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Level
    {
        public class LevelRenderer : MonoBehaviour
        {

            [Header("Settings")]
            //public float spanHorizontal = 4;
            //public float spanVertical = 4;
                
            const string TOPLEFT = "tl";
            const string TOPRIGHT = "tr";
            const string BOTTOMLEFT = "bl";
            const string BOTTOMRIGHT = "br";

            [Header("Prefabs")]
            public GameObject doorewPrefab;
            public GameObject doornsPrefab;
            public GameObject exitewPrefab;
            public GameObject exitnsPrefab;
            public GameObject entryewPrefab;
            public GameObject entrynsPrefab;
            public GameObject pointOfInterestPrefab;

            [Header("Configured Game Objects")]
            private GameObject levelParent;
            private GameObject enemiesParent;
            private PolyNav.PolyNav2D polyNav2D;

            [Header("Runtime Game Objects")]
            private Entities.EntityManager entityManager;
            private Story.InfoManager infoManager;

            public void Initialize(Entities.EntityManager entityManager, Story.InfoManager infoManager, GameObject levelParent, GameObject enemiesParent, PolyNav.PolyNav2D polyNav2D)
            {
                this.levelParent = levelParent;
                this.enemiesParent = enemiesParent;
                this.polyNav2D = polyNav2D;
                this.entityManager = entityManager;
                this.infoManager = infoManager;
            }

            public void Render(Level level)
            {
                Logger.Log(name, "rendering level");
                DestroyLevel();
                PopulateLevelTilemap(level);
                //UpdateLevelBoundingBox(level);

                if (level.template.generateNavMesh)
                {
                    polyNav2D.GenerateMap();
                }
            }

            public void UpdateLevelBoundingBox(Level level)
            {
                BoxCollider2D boxCollider = levelParent.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(level.template.levelWidth * level.template.spanHorizontal, level.template.levelHeight * level.template.spanVertical);
                boxCollider.offset = (boxCollider.size / 2);
                //boxCollider.enabled = true;
            }

            public void DestroyLevel()
            {
                // Destroy NPCs
                for (int i = 0; i < enemiesParent.transform.childCount; i++)
                {
                    Destroy(enemiesParent.transform.GetChild(i).gameObject);
                }

                // Destroy Tiles
                for (int k = 0; k < levelParent.transform.childCount; k++)
                {
                    Destroy(levelParent.transform.GetChild(k).gameObject);
                }
            }

            void PopulateLevelTilemap(Level level)
            {
                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {
                        Vector3 roomPosition = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical), 0);

                        if (level.map[i].rooms[j] == null)
                        {
                            continue;
                        }


                        if (!string.IsNullOrWhiteSpace(level.map[i].rooms[j].floor))
                        {
                            var floor = level.template.floors.FirstOrDefault(o => o != null && o.name == level.map[i].rooms[j].floor);

                            if (floor != null)
                            {
                                Instantiate(floor, roomPosition, Quaternion.identity, levelParent.transform);
                            }
                        }
                        else
                        {
                            if (level.map[i].rooms[j].isMainChain)
                            {
                                if (level.template.mainChainFloor.Count > 0 && level.template.mainChainFloor != null)
                                {
                                    Instantiate(level.template.mainChainFloor[0], roomPosition, Quaternion.identity, levelParent.transform);
                                }
                            }
                            else
                            {
                                if (level.template.floors.Count > 0 && level.template.floors != null)
                                {
                                    Instantiate(level.template.floors[0], roomPosition, Quaternion.identity, levelParent.transform);
                                }
                            }
                        }

                        // BL
                        for (int e = 0; e < level.map[i].rooms[j].bottomLeft.Count; e++)
                        {
                            var go = FindRoomEntity(level.map[i].rooms[j].bottomLeft[e].type, level.map[i].rooms[j].bottomLeft[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        // BR
                        for (int e = 0; e < level.map[i].rooms[j].bottomRight.Count; e++)
                        {
                            var go = FindRoomEntity(level.map[i].rooms[j].bottomRight[e].type, level.map[i].rooms[j].bottomRight[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        // TL
                        for (int e = 0; e < level.map[i].rooms[j].topLeft.Count; e++)
                        {

                            var go = FindRoomEntity(level.map[i].rooms[j].topLeft[e].type, level.map[i].rooms[j].topLeft[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);

 
                        }

                        //TR
                        for (int e = 0; e < level.map[i].rooms[j].topRight.Count; e++)
                        {
                            var go = FindRoomEntity(level.map[i].rooms[j].topRight[e].type, level.map[i].rooms[j].topRight[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                          }

                        Instantiate(pointOfInterestPrefab, roomPosition + new Vector3(level.template.spanHorizontal / 2, level.template.spanVertical / 2, 0), Quaternion.identity, levelParent.transform);
                    }
                }
            }

            GameObject FindRoomEntity(string type, string name, LevelGenTemplate levelGenTemplate)
            {
                GameObject result = null;

                switch (type)
                {
                    case ProxyRoom.ObjTypeWall:
                        return levelGenTemplate.levelElements.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.ObjTypeEntry:
                        result = levelGenTemplate.startProps.FirstOrDefault(g => g != null && g.name == name);

                        if (result == null)
                        {
                            return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);
                        }

                        break;

                    case ProxyRoom.ObjTypeEnd:
                        result = levelGenTemplate.endProps.FirstOrDefault(g => g != null && g.name == name);
                        if (result == null)
                        {
                            return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);
                        }

                        break;

                    case ProxyRoom.ObjTypeTrap:
                        return levelGenTemplate.trapProps.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.ObjTypeRandom:
                        return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.ObjTypeFixed:
                        return levelGenTemplate.fixedProps.FirstOrDefault(g => g != null && g.name == name);
                }

                return result;

            }

            public void PopulateLevelDoors(Level level, List<Triggers.Door> doorList)
            {
                //List<Triggers.Door> results = new List<Triggers.Door>();

                if (!level.template.generateDoors)
                {
                    Logger.Log(name, "Skipping doors");
                    return;
                }
                //FIXME: swap i & j to be consistent!
                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {
                        ProxyRoom room = level.map[i].rooms[j];

                        if (room == null)
                        {
                            continue;
                        }

                        if (room.top == ProxyRoom.Door)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(doorewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Door)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(doornsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.top == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                        if (room.bottom == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), (i + 1) * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                        if (room.top == ProxyRoom.Entry)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(entryewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Entry)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(entrynsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                    }
                }

                return;
            }

            public void PopulateNPCSpawns(Level level, List<Entities.BaseEntity> entityList)
            {
                if (level.npcSpawnLocationList == null)
                {
                    Logger.Log(name, "empty npc spawn location list");
                    return;
                }

                for (int i = 0; i < level.npcSpawnLocationList.Count; i++)
                {
                    Logger.Log(name, "attempting to spawn", level.npcSpawnLocationList[i].name);
                    Entities.BaseEntity npc = entityManager.GetPooledNPC(level.npcSpawnLocationList[i].name);


                    if (npc == null)
                    {
                        Logger.LogError(name, "unable to find pooled NPC", level.npcSpawnLocationList[i].name);
                        continue;
                    }

                    npc.transform.position = level.ConvertLevelPosToWorld(level.npcSpawnLocationList[i].levelLocation) + level.npcSpawnLocationList[i].worldOffset;
                    npc.gameObject.SetActive(true);
                    npc.Initialize();
                    entityList.Add(npc);

                }
            }

            public void PopulateEnemySpawns(Level level, List<Entities.Enemy> enemyList)
            {
                Logger.Log(name, "Populating enemy spawns");

                if (entityManager.enemies.Count <= 0)
                {
                    Logger.Log(name, "No enemies to spawn");
                    return;
                }

                if (level.enemySpawnLocationList == null)
                {
                    return;
                }

                for (int i = 0; i < level.enemySpawnLocationList.Count; i++)
                {
                    Logger.Log(name, "Attempting to spawn " + level.enemySpawnLocationList[i].name);

                    GameObject enemyPrefab = entityManager.enemies.FirstOrDefault(g => g != null && g.name == level.enemySpawnLocationList[i].name);

                    if (enemyPrefab == null)
                    {
                        continue;
                    }

                    var go = Instantiate(enemyPrefab, level.ConvertLevelPosToWorld(level.enemySpawnLocationList[i].levelLocation) + level.enemySpawnLocationList[i].worldOffset, Quaternion.identity, enemiesParent.transform);
                    Entities.Enemy enemy = go.GetComponent<Entities.Enemy>();


                    if (enemy == null)
                    {
                        Logger.LogError(name, "Null Enemy object");
                        continue;
                    }

                    enemy.Initialize(CoreGame.Instance.GetPlayer().transform, polyNav2D);

                    Story.Enemy uniqueEnemy = infoManager.GenerateRandomEnemy(enemy.enemy);
                    enemyList.Add(enemy);
                }
            }


            public List<Entities.BaseTrap> PopulateTrapSpawns(Level level)
            {
                List<Entities.BaseTrap> results = new List<Entities.BaseTrap>();

                if (level.trapSpawnLocationList == null)
                {
                    return results;
                }

                if (entityManager.traps.Count <= 0)
                {
                    return results;
                }

                for (int i = 0; i < level.trapSpawnLocationList.Count; i++)
                {

                    GameObject trapGameObj = entityManager.traps.FirstOrDefault(g => g != null && g.name == level.trapSpawnLocationList[i].name);

                    if (trapGameObj != null)
                    {
                        var go = GameObject.Instantiate(trapGameObj, level.ConvertLevelPosToWorld(level.trapSpawnLocationList[i].levelLocation) + level.trapSpawnLocationList[i].worldOffset, Quaternion.identity, enemiesParent.transform);
                        Entities.BaseTrap npc = go.GetComponent<Entities.BaseTrap>();
                        if (npc != null)
                        {
                            npc.Initialize();
                            results.Add(npc);
                        }
                    }
                }

                return results;
            }
        }
    }
}