using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace HackedDesign
{
    namespace Level
    {
        public class LevelRenderer : MonoBehaviour
        {
            private const string TOPLEFT = "tl";
            private const string TOPRIGHT = "tr";
            private const string BOTTOMLEFT = "bl";
            private const string BOTTOMRIGHT = "br";

            [Header("Prefabs")]
            [SerializeField] private GameObject doorewPrefab;
            [SerializeField] private GameObject doornsPrefab;
            [SerializeField] private GameObject exitewPrefab;
            [SerializeField] private GameObject exitnsPrefab;
            [SerializeField] private GameObject entryewPrefab;
            [SerializeField] private GameObject entrynsPrefab;
            [SerializeField] private GameObject roomCenterPrefab;
            [SerializeField] private GameObject roomCornerCenter;
            [SerializeField] private GameObject pointOfInterestPrefab;

            private PlayerController playerController;
            private GameObject levelParent;
            private GameObject enemiesParent;
            private PolyNav.PolyNav2D polyNav2D;
            private Entities.EntityManager entityManager;

            public void Initialize(PlayerController playerController, Entities.EntityManager entityManager, GameObject levelParent, GameObject enemiesParent, PolyNav.PolyNav2D polyNav2D)
            {
                this.playerController = playerController;
                this.levelParent = levelParent;
                this.enemiesParent = enemiesParent;
                this.polyNav2D = polyNav2D;
                this.entityManager = entityManager;
            }

            public void Render(Level level)
            {
                Logger.Log(name, "rendering level");
                DestroyLevel();
                PopulateLevelTilemap(level);
                UpdateLevelBoundingBox(level);

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

                        var room = level.map[i].rooms[j];

                        if (room == null)
                        {
                            continue;
                        }

                        Vector3 roomPosition = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical), 0);


                        if (!string.IsNullOrWhiteSpace(room.floor))
                        {
                            var floor = level.template.floors.FirstOrDefault(o => o != null && o.name == room.floor);

                            if (floor != null)
                            {
                                Instantiate(floor, roomPosition, Quaternion.identity, levelParent.transform);
                            }
                        }
                        else
                        {
                            if (room.isMainChain)
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
                        for (int e = 0; e < room.bottomLeft.Count; e++)
                        {
                            var go = FindRoomEntity(room.bottomLeft[e].type, room.bottomLeft[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        // BR
                        for (int e = 0; e < room.bottomRight.Count; e++)
                        {
                            var go = FindRoomEntity(room.bottomRight[e].type, room.bottomRight[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        // TL
                        for (int e = 0; e < room.topLeft.Count; e++)
                        {

                            var go = FindRoomEntity(room.topLeft[e].type, room.topLeft[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        //TR
                        for (int e = 0; e < room.topRight.Count; e++)
                        {
                            var go = FindRoomEntity(room.topRight[e].type, room.topRight[e].name, level.template);
                            if (go == null)
                            {
                                Logger.LogError(name, "null game object returned from FindRoomEntity");
                            }
                            Instantiate(go, roomPosition, Quaternion.identity, levelParent.transform);
                        }

                        //Instantiate(roomCenterPrefab, roomPosition + new Vector3(level.template.spanHorizontal / 2, level.template.spanVertical / 2, 0), Quaternion.identity, levelParent.transform);

                        if (!(room.isEntry || room.isEnd))
                        {
                            Instantiate(pointOfInterestPrefab, roomPosition + new Vector3(level.template.spanHorizontal / 2, level.template.spanVertical / 2, 0), Quaternion.identity, levelParent.transform);
                        }
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

                    case ProxyRoom.ObjTypeLineOfSight:
                        return levelGenTemplate.lineOfSightProps.FirstOrDefault(g => g != null && g.name == name);

                }

                return result;

            }

            public void PopulateLevelDoors(Level level, List<Door> doorList)
            {
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
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal / 2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(doorewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Door)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical / 2), 0);
                            var go = Instantiate(doornsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.top == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal / 2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical / 2), 0);
                            var go = Instantiate(exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                        if (room.bottom == ProxyRoom.Exit)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal / 2), (i + 1) * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                        if (room.top == ProxyRoom.Entry)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal / 2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(entryewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.Entry)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical / 2), 0);
                            var go = Instantiate(entrynsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Door door = go.GetComponent<Door>();
                            if (door != null)
                            {
                                doorList.Add(door);
                            }
                        }
                    }
                }

                return;
            }

            public void PopulateNPCSpawns(Level level, List<IEntity> entityList)
            {
                if (level.npcSpawnLocationList == null)
                {
                    Logger.Log(name, "Empty NPC spawn location list");
                    return;
                }

                for (int i = 0; i < level.npcSpawnLocationList.Count; i++)
                {
                    Logger.Log(name, "Attempting to spawn ", level.npcSpawnLocationList[i].name);
                    IEntity npc = entityManager.GetPooledNPC(level.npcSpawnLocationList[i].name);

                    if (npc == null)
                    {
                        Logger.LogError(name, "Unable to find pooled NPC", level.npcSpawnLocationList[i].name);
                        continue;
                    }

                    npc.Initialize(true, this.playerController.transform);
                    npc.SetPosition(level.ConvertLevelPosToWorld(level.npcSpawnLocationList[i].levelLocation) + level.npcSpawnLocationList[i].worldOffset);
                    npc.Activate();

                    entityList.Add(npc);

                }
            }

            public void PopulateEnemySpawns(Level level, List<IEntity> enemyList)
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
                    Logger.Log(this, "Attempting to spawn " + level.enemySpawnLocationList[i].name);

                    GameObject enemyPrefab = entityManager.enemies.FirstOrDefault(g => g != null && g.name == level.enemySpawnLocationList[i].name);

                    if (enemyPrefab is null)
                    {
                        continue;
                    }

                    var go = Instantiate(enemyPrefab, level.ConvertLevelPosToWorld(level.enemySpawnLocationList[i].levelLocation) + level.enemySpawnLocationList[i].worldOffset, Quaternion.identity, enemiesParent.transform);
                    IEntity enemy = go.GetComponent<IEntity>();

                    if (enemy is null)
                    {
                        Logger.LogError(this, "Null Enemy object");
                        continue;
                    }

                    enemy.Initialize(false, this.playerController.transform);
                    enemy.SetEntityDefinition(Story.InfoRepository.Instance.GenerateRandomEnemy((Story.Enemy)enemy.GetEntityDefinition()));
                    enemyList.Add(enemy);
                }
            }

            public void PopulateTrapSpawns(Level level, List<IEntity> trapList)
            {
                Logger.Log(name, "Populating trap spawns");

                if (entityManager.traps.Count <= 0)
                {
                    Logger.Log(name, "No traps to spawn");
                    return;
                }

                if (level.trapSpawnLocationList == null)
                {
                    return;
                }

                for (int i = 0; i < level.trapSpawnLocationList.Count; i++)
                {
                    Logger.Log(this, "Attempting to spawn " + level.trapSpawnLocationList[i].name);

                    GameObject trapPrefab = entityManager.traps.FirstOrDefault(g => g != null && g.name == level.trapSpawnLocationList[i].name);

                    if (trapPrefab is null)
                    {
                        continue;
                    }

                    var go = Instantiate(trapPrefab, level.ConvertLevelPosToWorld(level.trapSpawnLocationList[i].levelLocation) + level.trapSpawnLocationList[i].worldOffset, Quaternion.identity, enemiesParent.transform);
                    IEntity trap = go.GetComponent<IEntity>();

                    if (trap is null)
                    {
                        Logger.LogError(this, "Null Enemy object");
                        continue;
                    }

                    trap.Initialize(false, this.playerController.transform);
                    trap.SetEntityDefinition(Story.InfoRepository.Instance.GenerateRandomTrap((Story.Trap)trap.GetEntityDefinition()));
                    trapList.Add(trap);
                }
            }
        }
    }
}