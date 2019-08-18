using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign {
	namespace Level {
		public class LevelRenderer : MonoBehaviour {

			const string TOPLEFT = "tl";
			const string TOPRIGHT = "tr";
			const string BOTTOMLEFT = "bl";
			const string BOTTOMRIGHT = "br";

			public GameObject doorewPrefab;
			public GameObject doornsPrefab;
			public GameObject exitewPrefab;
			public GameObject exitnsPrefab;

			private GameObject levelParent;
			private GameObject npcParent;
			private PolyNav.PolyNav2D polyNav2D;

			public List<GameObject> enemyEasyPrefabs;

			public void Initialize (GameObject levelParent, GameObject npcParent, PolyNav.PolyNav2D polyNav2D) {
				this.levelParent = levelParent;
				this.npcParent = npcParent;
				this.polyNav2D = polyNav2D;
			}

			public void Render (Level level) {
				PopulateLevelTilemap (level);
				//PopulateEnemySpawns (level);


				PopulateLevelDoors (level);
				
				//PopulateCameraSpawns (level);

				BoxCollider2D boxCollider = levelParent.GetComponent<BoxCollider2D> ();

				boxCollider.size = new Vector2 (level.template.levelWidth * 4, level.template.levelHeight * 4);
				boxCollider.offset = (boxCollider.size / 2);

				if (level.template.generateNavMesh) {
					polyNav2D.GenerateMap ();
				}			

	
			}

			public void DestroyLevel () {
				// Destroy NPCs
				for (int i = 0; i < npcParent.transform.childCount; i++) {
					GameObject.Destroy (npcParent.transform.GetChild (i).gameObject);
				}

				// Destroy Tiles
				for (int k = 0; k < levelParent.transform.childCount; k++) {
					GameObject.Destroy (levelParent.transform.GetChild (k).gameObject);
				}
			}

			void PopulateLevelTilemap (Level level) {

				//List<Entity.BaseEntity> results = new List<Entity.BaseEntity> ();
				DestroyLevel ();

				for (int i = 0; i < level.map.Count(); i++) {
					for (int j = 0; j < level.map[i].rooms.Count(); j++) {

						Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

						if (level.map[i].rooms[j] != null) {

							if (level.template.floor != null) {
								GameObject.Instantiate (level.template.floor, pos, Quaternion.identity, levelParent.transform);
							}

							// BL
							for (int e = 0; e < level.map[i].rooms[j].bottomLeft.Count; e++) {
								Debug.Log(level.map[i].rooms[j].bottomLeft[e].type + level.map[i].rooms[j].bottomLeft[e].name + level.template.name);
								var go = FindRoomEntity (level.map[i].rooms[j].bottomLeft[e].type, level.map[i].rooms[j].bottomLeft[e].name, level.template);
								if (go == null) {
									Debug.LogError ("null go");
								}
								GameObject.Instantiate (go, pos, Quaternion.identity, levelParent.transform);

								if (level.map[i].rooms[j].bottomLeft[e].isTrap) {
									Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
									npc.Initialize ();
									CoreGame.Instance.CoreState.entityList.Add(npc);
									//results.Add (npc);
								}

								// 		Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
								// 		npc.Initialize ();
								// 		results.Add (npc);
							}

							// BR
							for (int e = 0; e < level.map[i].rooms[j].bottomRight.Count; e++) {
								var go = FindRoomEntity (level.map[i].rooms[j].bottomRight[e].type, level.map[i].rooms[j].bottomRight[e].name, level.template);
								if (go == null) {
									Debug.LogError ("null go");
								}
								GameObject.Instantiate (go, pos, Quaternion.identity, levelParent.transform);
								if (level.map[i].rooms[j].bottomRight[e].isTrap) {
									Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
									npc.Initialize ();
									CoreGame.Instance.CoreState.entityList.Add(npc);
									//results.Add (npc);
								}								
							}

							// TL
							for (int e = 0; e < level.map[i].rooms[j].topLeft.Count; e++) {

								var go = FindRoomEntity (level.map[i].rooms[j].topLeft[e].type, level.map[i].rooms[j].topLeft[e].name, level.template);
								if (go == null) {
									Debug.LogError ("null go");
								}
								GameObject.Instantiate (go, pos, Quaternion.identity, levelParent.transform);

								if (level.map[i].rooms[j].topLeft[e].isTrap) {
									Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
									npc.Initialize ();
									CoreGame.Instance.CoreState.entityList.Add(npc);
									//results.Add (npc);
								}								
							}

							//TR
							for (int e = 0; e < level.map[i].rooms[j].topRight.Count; e++) {
								var go = FindRoomEntity (level.map[i].rooms[j].topRight[e].type, level.map[i].rooms[j].topRight[e].name, level.template);
								if (go == null) {
									Debug.LogError ("null go");
								}
								GameObject.Instantiate (go, pos, Quaternion.identity, levelParent.transform);

								if (level.map[i].rooms[j].topRight[e].isTrap) {
									Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
									npc.Initialize ();
									CoreGame.Instance.CoreState.entityList.Add(npc);
									//results.Add (npc);
								}								
							}
						}
					}

					
				}
				
			}

			GameObject FindRoomEntity (string type, string name, LevelGenTemplate levelGenTemplate) {
				GameObject result = null;

				

				switch (type) {
					case ProxyRoom.OBJ_TYPE_WALL:
						return levelGenTemplate.levelElements.FirstOrDefault (g => g != null && g.name == name);

					case ProxyRoom.OBJ_TYPE_ENTRY:
						result = levelGenTemplate.startProps.FirstOrDefault (g => g != null && g.name == name);

						if (result == null) {
							return levelGenTemplate.randomProps.FirstOrDefault (g => g != null && g.name == name);
						}

						break;

					case ProxyRoom.OBJ_TYPE_END:
						result = levelGenTemplate.endProps.FirstOrDefault (g => g != null && g.name == name);
						if (result == null) {
							return levelGenTemplate.randomProps.FirstOrDefault (g => g != null && g.name == name);
						}

						break;

					case ProxyRoom.OBJ_TYPE_TRAP:
						return levelGenTemplate.trapProps.FirstOrDefault (g => g != null && g.name == name);

					case ProxyRoom.OBJ_TYPE_RANDOM:
						return levelGenTemplate.randomProps.FirstOrDefault (g => g != null && g.name == name);
				}

				return result;

			}

			void PopulateLevelDoors (Level level) {
				if (!level.template.generateDoors) {
					Debug.Log ("Skipping doors");
					return;
				}

				for (int i = 0; i < level.map.Count(); i++) {
					for (int j = 0; j < level.map[i].rooms.Count(); j++) {
						ProxyRoom room = level.map[i].rooms[j];

						if (room != null) {
							if (room.top == ProxyRoom.DOOR) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (doorewPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (room.left == ProxyRoom.DOOR) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (doornsPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (room.top == ProxyRoom.EXIT) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (exitewPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (room.left == ProxyRoom.EXIT) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
							}

						}
					}
				}
			}

			public List<Entity.BaseEnemy> PopulateEnemySpawns (Level level) {
	
				List<Entity.BaseEnemy> results = new List<Entity.BaseEnemy> ();

				if (enemyEasyPrefabs.Count <= 0) {
					return results;
				}

				for (int i = 0; i < level.enemySpawnLocationList.Count; i++) {

					int rand = UnityEngine.Random.Range (0, enemyEasyPrefabs.Count);

					GameObject sggo = enemyEasyPrefabs[rand];
					var go = GameObject.Instantiate (sggo, level.ConvertLevelPosToWorld (level.enemySpawnLocationList[i]), Quaternion.identity, npcParent.transform);
					Entity.BaseEnemy npc = go.GetComponent<Entity.BaseEnemy> ();
					npc.Initialize (polyNav2D);
					//CoreGame.instance.state.entityList.Add(npc);
					results.Add (npc);
				}

				return results;

			}

			// // Move first half back to generator
			public List<Entity.BaseTrap> PopulateTrapSpawns (Level level) {
				//Level level = CoreGame.instance.state.level;

				List<Entity.BaseTrap> results = new List<Entity.BaseTrap> ();

				if (level.template.trapProps.Count <= 0) {
					return results;
				}

				for (int i = 0; i < level.trapSpawnLocationList.Count; i++) {

					int rand = UnityEngine.Random.Range (0, level.template.trapProps.Count);

					GameObject sggo = level.template.trapProps[rand];
					var go = GameObject.Instantiate (sggo, level.ConvertLevelPosToWorld (level.trapSpawnLocationList[i]), Quaternion.identity, npcParent.transform);
					Entity.BaseTrap npc = go.GetComponent<Entity.BaseTrap> ();
					npc.Initialize ();
					results.Add (npc);
				}

				return results;
			}

		}
	}
}