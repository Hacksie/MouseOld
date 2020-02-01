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

            private Entity.EntityManager entityManager;

            public void Initialize(Entity.EntityManager entityManager, GameObject levelParent, GameObject npcParent, PolyNav.PolyNav2D polyNav2D)
            {
                this.levelParent = levelParent;
                this.npcParent = npcParent;
                this.polyNav2D = polyNav2D;
                this.entityManager = entityManager;
            }

            public void Render(Level level)
            {
                Debug.Log(this.name + ": rendering level");
                DestroyLevel();
                PopulateLevelTilemap(level);
                //PopulateEnemySpawns (level);


                PopulateLevelDoors(level);

                //PopulateCameraSpawns (level);

                BoxCollider2D boxCollider = levelParent.GetComponent<BoxCollider2D>();

                boxCollider.size = new Vector2(level.template.levelWidth * 4, level.template.levelHeight * 4);
                boxCollider.offset = (boxCollider.size / 2);

                if (level.template.generateNavMesh)
                {
                    polyNav2D.GenerateMap();
                }


            }

            public void DestroyLevel()
            {
                // Destroy NPCs
                // for (int i = 0; i < npcParent.transform.childCount; i++)
                // {
                //     GameObject.Destroy(npcParent.transform.GetChild(i).gameObject);
                // }

                // Destroy Tiles
                for (int k = 0; k < levelParent.transform.childCount; k++)
                {
                    GameObject.DestroyImmediate(levelParent.transform.GetChild(k).gameObject);
                }
            }

            void PopulateLevelTilemap(Level level)
            {
                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {

                        Vector3 pos = new Vector3(j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

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
                            //Debug.Log(this.name + ": bl count " + level.map[i].rooms[j].bottomLeft.Count);
                            for (int e = 0; e < level.map[i].rooms[j].bottomLeft.Count; e++)
                            {
                                //Debug.Log(level.map[i].rooms[j].bottomLeft[e].type + level.map[i].rooms[j].bottomLeft[e].name + level.template.name);
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
                            //Debug.Log(this.name + ": br count " + level.map[i].rooms[j].bottomRight.Count);
                            for (int e = 0; e < level.map[i].rooms[j].bottomRight.Count; e++)
                            {
                                //Debug.Log(level.map[i].rooms[j].bottomRight[e].type + level.map[i].rooms[j].bottomRight[e].name + level.template.name);
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
                            //Debug.Log(this.name + ": tl count " + level.map[i].rooms[j].topLeft.Count);
                            for (int e = 0; e < level.map[i].rooms[j].topLeft.Count; e++)
                            {
                                // if(level.map[i].rooms[j].isEntry) {
                                // Debug.Log(this.name + ": tl " + level.map[i].rooms[j].topLeft[e].type + " " + level.map[i].rooms[j].topLeft[e].name);
                                // }
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
                            //Debug.Log(this.name + ": tr count " + level.map[i].rooms[j].topRight.Count);
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

            void PopulateLevelDoors(Level level)
            {
                if (!level.template.generateDoors)
                {
                    Debug.Log("Skipping doors");
                    return;
                }

                for (int i = 0; i < level.map.Count(); i++)
                {
                    for (int j = 0; j < level.map[i].rooms.Count(); j++)
                    {
                        ProxyRoom room = level.map[i].rooms[j];

                        if (room != null)
                        {
                            if (room.top == ProxyRoom.DOOR)
                            {
                                Vector3 pos = new Vector3(j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
                                GameObject.Instantiate(doorewPrefab, pos, Quaternion.identity, levelParent.transform);
                            }

                            if (room.left == ProxyRoom.DOOR)
                            {
                                Vector3 pos = new Vector3(j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
                                GameObject.Instantiate(doornsPrefab, pos, Quaternion.identity, levelParent.transform);
                            }

                            if (room.top == ProxyRoom.EXIT)
                            {
                                Vector3 pos = new Vector3(j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
                                GameObject.Instantiate(exitewPrefab, pos, Quaternion.identity, levelParent.transform);
                            }

                            if (room.left == ProxyRoom.EXIT)
                            {
                                Vector3 pos = new Vector3(j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
                                GameObject.Instantiate(exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
                            }

                            if (room.top == ProxyRoom.ENTRY)
                            {
                                Vector3 pos = new Vector3(j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
                                GameObject.Instantiate(entryewPrefab, pos, Quaternion.identity, levelParent.transform);
                            }

                            if (room.left == ProxyRoom.ENTRY)
                            {
                                Vector3 pos = new Vector3(j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
                                GameObject.Instantiate(entrynsPrefab, pos, Quaternion.identity, levelParent.transform);
                            }                            

                        }
                    }
                }
            }

            public List<Entity.BaseEnemy> PopulateEnemySpawns(Level level)
            {

                List<Entity.BaseEnemy> results = new List<Entity.BaseEnemy>();

                if (entityManager.enemies.Count <= 0)
                {
                    return results;
                }

                if (level.enemySpawnLocationList == null)
                {
                    return results;
                }

                for (int i = 0; i < level.enemySpawnLocationList.Count; i++)
                {

                    //int rand = UnityEngine.Random.Range(0, enemyEasyPrefabs.Count);

                    GameObject enemyGameObj = entityManager.enemies.FirstOrDefault(g => g != null && g.name == level.enemySpawnLocationList[i].name);

                    if (enemyGameObj != null)
                    {
                        var go = GameObject.Instantiate(enemyGameObj, level.ConvertLevelPosToWorld(level.enemySpawnLocationList[i].levelLocation) + level.enemySpawnLocationList[i].worldOffset, Quaternion.identity, npcParent.transform);
                        Entity.BaseEnemy npc = go.GetComponent<Entity.BaseEnemy>();
                        if (npc != null)
                        {
                            npc.Initialize(polyNav2D);
                            //CoreGame.instance.state.entityList.Add(npc);
                            results.Add(npc);
                        }
                    }
                }

                return results;

            }


            public List<Entity.BaseTrap> PopulateTrapSpawns(Level level)
            {
                List<Entity.BaseTrap> results = new List<Entity.BaseTrap>();

                //Level level = CoreGame.instance.state.level;
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
                        Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap>();
                        if (npc != null)
                        {
                            npc.Initialize();
                            //CoreGame.instance.state.entityList.Add(npc);
                            results.Add(npc);
                        }
                    }
                }

                return results;
            }

            public List<Entity.BaseEntity> PopulateNPCSpawns(Level level)
            {
                List<Entity.BaseEntity> results = new List<Entity.BaseEntity>();

                if (level.npcSpawnLocationList == null)
                {
                    return results;
                }

                for (int i = 0; i < level.npcSpawnLocationList.Count; i++)
                {
                    Debug.Log(this.name + ": attempting to spawn " + level.npcSpawnLocationList[i].name);
                    Entity.BaseEntity npc = entityManager.GetPooledNPC(level.npcSpawnLocationList[i].name);

                    //GameObject npcGameObj = entityManager.GetPooledNPC(level.npcSpawnLocationList[i].name);
                    if (npc != null)
                    {
                        Debug.Log(this.name + ": moving " + level.npcSpawnLocationList[i].name);
                        npc.transform.position = level.ConvertLevelPosToWorld(level.npcSpawnLocationList[i].levelLocation) + level.npcSpawnLocationList[i].worldOffset;
                        npc.gameObject.SetActive(true);
                        npc.Initialize();
                        results.Add(npc);

                    }
                    else 
                    {
                        Debug.LogError(this.name + ": unable to find pooled NPC");
                    }
                }

                return results;
            }

        }
    }
}