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

            public GameObject doorewPrefab;
            public GameObject doornsPrefab;
            public GameObject exitewPrefab;
            public GameObject exitnsPrefab;
            public GameObject entryewPrefab;
            public GameObject entrynsPrefab;

            private GameObject levelParent;
            private GameObject npcParent;
            private PolyNav.PolyNav2D polyNav2D;

            private Entities.EntityManager entityManager;
            private Story.InfoManager infoManager;

            public void Initialize(Entities.EntityManager entityManager, Story.InfoManager infoManager, GameObject levelParent, GameObject npcParent, PolyNav.PolyNav2D polyNav2D)
            {
                this.levelParent = levelParent;
                this.npcParent = npcParent;
                this.polyNav2D = polyNav2D;
                this.entityManager = entityManager;
                this.infoManager = infoManager;
            }

            public void Render(Level level)
            {
                Debug.Log(this.name + ": rendering level");
                DestroyLevel();
                PopulateLevelTilemap(level);




                //PopulateCameraSpawns (level);

                BoxCollider2D boxCollider = levelParent.GetComponent<BoxCollider2D>();

                boxCollider.size = new Vector2(level.template.levelWidth * level.template.spanHorizontal, level.template.levelHeight * level.template.spanVertical);
                boxCollider.offset = (boxCollider.size / 2);

                if (level.template.generateNavMesh)
                {
                    polyNav2D.GenerateMap();
                }


            }

            public void DestroyLevel()
            {
                // Destroy NPCs
                for (int i = 0; i < npcParent.transform.childCount; i++)
                {
                    GameObject.Destroy(npcParent.transform.GetChild(i).gameObject);
                }

                // Destroy Tiles
                for (int k = 0; k < levelParent.transform.childCount; k++)
                {
                    GameObject.Destroy(levelParent.transform.GetChild(k).gameObject);
                }
            }

            void PopulateLevelTilemap(Level level)
            {
                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {

                        Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical), 0);

                        if (level.map[i].rooms[j] != null)
                        {
                            if (!string.IsNullOrWhiteSpace(level.map[i].rooms[j].floor))
                            {
                                var floor = level.template.floors.FirstOrDefault(o => o != null && o.name == level.map[i].rooms[j].floor);

                                if (floor != null)
                                {
                                    GameObject.Instantiate(floor, pos, Quaternion.identity, levelParent.transform);
                                }
                            }
                            else
                            {
                                if (level.template.floors.Count > 0 && level.template.floors != null)
                                {
                                    GameObject.Instantiate(level.template.floors[0], pos, Quaternion.identity, levelParent.transform);
                                }
                            }

                            // BL
                            for (int e = 0; e < level.map[i].rooms[j].bottomLeft.Count; e++)
                            {
                                var go = FindRoomEntity(level.map[i].rooms[j].bottomLeft[e].type, level.map[i].rooms[j].bottomLeft[e].name, level.template);
                                if (go == null)
                                {
                                    Debug.LogError(this.name + ": null game object returned from FindRoomEntity");
                                }
                                GameObject.Instantiate(go, pos, Quaternion.identity, levelParent.transform);

                                // if (level.map[i].rooms[j].bottomLeft[e].isTrap)
                                // {
                                //     Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap>();
                                //     npc.Initialize();
                                //     CoreGame.Instance.CoreState.entityList.Add(npc);
                                //     //results.Add (npc);
                                // }

                                // 		Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
                                // 		npc.Initialize ();
                                // 		results.Add (npc);
                            }

                            // BR
                            for (int e = 0; e < level.map[i].rooms[j].bottomRight.Count; e++)
                            {
                                var go = FindRoomEntity(level.map[i].rooms[j].bottomRight[e].type, level.map[i].rooms[j].bottomRight[e].name, level.template);
                                if (go == null)
                                {
                                    Debug.LogError("null go");
                                }
                                GameObject.Instantiate(go, pos, Quaternion.identity, levelParent.transform);
                                // if (level.map[i].rooms[j].bottomRight[e].isTrap)
                                // {
                                //     Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap>();
                                //     npc.Initialize();
                                //     CoreGame.Instance.CoreState.entityList.Add(npc);
                                //     //results.Add (npc);
                                // }
                            }

                            // TL
                            for (int e = 0; e < level.map[i].rooms[j].topLeft.Count; e++)
                            {
  
                                var go = FindRoomEntity(level.map[i].rooms[j].topLeft[e].type, level.map[i].rooms[j].topLeft[e].name, level.template);
                                if (go == null)
                                {
                                    Debug.LogError("null go");
                                }
                                GameObject.Instantiate(go, pos, Quaternion.identity, levelParent.transform);

                                // if (level.map[i].rooms[j].topLeft[e].isTrap)
                                // {
                                //     Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap>();
                                //     npc.Initialize();
                                //     CoreGame.Instance.CoreState.entityList.Add(npc);
                                //     //results.Add (npc);
                                // }
                            }

                            //TR
                            for (int e = 0; e < level.map[i].rooms[j].topRight.Count; e++)
                            {
                                var go = FindRoomEntity(level.map[i].rooms[j].topRight[e].type, level.map[i].rooms[j].topRight[e].name, level.template);
                                if (go == null)
                                {
                                    Debug.LogError("null go");
                                }
                                GameObject.Instantiate(go, pos, Quaternion.identity, levelParent.transform);

                                // if (level.map[i].rooms[j].topRight[e].isTrap)
                                // {
                                //     Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap>();
                                //     npc.Initialize();
                                //     CoreGame.Instance.CoreState.entityList.Add(npc);
                                //     //results.Add (npc);
                                // }
                            }
                        }
                    }
                }
            }

            GameObject FindRoomEntity(string type, string name, LevelGenTemplate levelGenTemplate)
            {
                GameObject result = null;

                switch (type)
                {
                    case ProxyRoom.OBJ_TYPE_WALL:
                        return levelGenTemplate.levelElements.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.OBJ_TYPE_ENTRY:
                        result = levelGenTemplate.startProps.FirstOrDefault(g => g != null && g.name == name);

                        if (result == null)
                        {
                            return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);
                        }

                        break;

                    case ProxyRoom.OBJ_TYPE_END:
                        result = levelGenTemplate.endProps.FirstOrDefault(g => g != null && g.name == name);
                        if (result == null)
                        {
                            return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);
                        }

                        break;

                    case ProxyRoom.OBJ_TYPE_TRAP:
                        return levelGenTemplate.trapProps.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.OBJ_TYPE_RANDOM:
                        return levelGenTemplate.randomProps.FirstOrDefault(g => g != null && g.name == name);

                    case ProxyRoom.OBJ_TYPE_FIXED:
                        return levelGenTemplate.fixedProps.FirstOrDefault(g => g != null && g.name == name);
                }

                return result;

            }

            public List<Triggers.Door> PopulateLevelDoors(Level level)
            {
                List<Triggers.Door> results = new List<Triggers.Door>();

                if (!level.template.generateDoors)
                {
                    Debug.Log("Skipping doors");
                    return results;
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

                        if (room.top == ProxyRoom.DOOR)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(doorewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.DOOR)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(doornsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }

                        if (room.top == ProxyRoom.EXIT)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.EXIT)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }
                        if (room.bottom == ProxyRoom.EXIT)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), (i + 1) * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }
                        if (room.top == ProxyRoom.ENTRY)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal + (level.template.spanHorizontal /2), i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + level.template.spanVertical, 0);
                            var go = Instantiate(entryewPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }

                        if (room.left == ProxyRoom.ENTRY)
                        {
                            Vector3 pos = new Vector3(j * level.template.spanHorizontal, i * -level.template.spanVertical + ((level.template.levelHeight - 1) * level.template.spanVertical) + (level.template.spanVertical /2), 0);
                            var go = Instantiate(entrynsPrefab, pos, Quaternion.identity, levelParent.transform);
                            Triggers.Door door = go.GetComponent<Triggers.Door>();
                            if (door != null)
                            {
                                results.Add(door);
                            }
                        }
                    }
                }

                return results;
            }

            public void PopulateNPCSpawns(Level level, List<Entities.BaseEntity> entityList)
            {
                if (level.npcSpawnLocationList == null)
                {
                    return;
                }

                for (int i = 0; i < level.npcSpawnLocationList.Count; i++)
                {
                    Debug.Log(this.name + ": attempting to spawn " + level.npcSpawnLocationList[i].name);
                    Entities.BaseEntity npc = entityManager.GetPooledNPC(level.npcSpawnLocationList[i].name);


                    if (npc == null)
                    {
                        Debug.LogError(this.name + ": unable to find pooled NPC");
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

                    var go = Instantiate(enemyPrefab, level.ConvertLevelPosToWorld(level.enemySpawnLocationList[i].levelLocation) + level.enemySpawnLocationList[i].worldOffset, Quaternion.identity, npcParent.transform);
                    Entities.Enemy enemy = go.GetComponent<Entities.Enemy>();


                    if (enemy == null)
                    {
                        Logger.LogError(this.name, "Null Enemy object");
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
                        var go = GameObject.Instantiate(trapGameObj, level.ConvertLevelPosToWorld(level.trapSpawnLocationList[i].levelLocation) + level.trapSpawnLocationList[i].worldOffset, Quaternion.identity, npcParent.transform);
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