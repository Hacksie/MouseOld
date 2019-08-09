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

			public List<GameObject> securityGuardEasyPrefabs;
			public List<GameObject> securityCameraPrefabs;

			public void Initialize (GameObject levelParent, GameObject npcParent, PolyNav.PolyNav2D polyNav2D) {
				this.levelParent = levelParent;
				this.npcParent = npcParent;
				this.polyNav2D = polyNav2D;
			}

			public void Render () {
				DestroyLevel ();
				Level level = CoreGame.instance.state.level;

				PopulateLevelTilemap (level);

				BoxCollider2D boxCollider = levelParent.GetComponent<BoxCollider2D> ();

				boxCollider.size = new Vector2 (level.template.levelWidth * 4, level.template.levelHeight * 4);
				boxCollider.offset = (boxCollider.size / 2);

				if (level.template.generateNavMesh) {
					polyNav2D.GenerateMap ();
				}

				PopulateLevelDoors (level);
				PopulateEnemySpawns (level);
				PopulateCameraSpawns (level);

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
				DestroyLevel ();

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4), 0);

						if (level.proxyLevel[j, i] != null) {

							if (level.template.floor != null) {
								GameObject.Instantiate (level.template.floor, pos, Quaternion.identity, levelParent.transform);
							}

							PopulateRoomSprites (level.proxyLevel[j, i], pos, levelParent.transform, RoomObjectType.Walls, level.template, false);
							if (level.proxyLevel[j, i].isEntry) {
								PopulateRoomSprites (level.proxyLevel[j, i], pos, levelParent.transform, RoomObjectType.Entry, level.template, false);
							} else if (level.proxyLevel[j, i].isEnd) {
								PopulateRoomSprites (level.proxyLevel[j, i], pos, levelParent.transform, RoomObjectType.End, level.template, false);
							} else {
								PopulateRoomSprites (level.proxyLevel[j, i], pos, levelParent.transform, RoomObjectType.Random, level.template, true);
							}

						}
					}
				}
			}

			void PopulateRoomSprites (ProxyRoom proxyRoom, Vector3 pos, Transform parent, RoomObjectType type, LevelGenTemplate template, bool allowEmpty) {
				string roomString = proxyRoom.AsPrintableString ();

				// TL
				if (!allowEmpty || (UnityEngine.Random.Range (0, 2) == 0)) {

					List<GameObject> goTLList = FindRoomObject (TOPLEFT, roomString.Substring (0, 1), roomString.Substring (1, 1), type, template).ToList ();

					goTLList.Randomize ();

					if (goTLList.FirstOrDefault () != null) {
						GameObject.Instantiate (goTLList[0], pos, Quaternion.identity, parent.transform);
					}
				}

				// TR

				if (!allowEmpty || (UnityEngine.Random.Range (0, 2) == 0)) {
					List<GameObject> goTRList = FindRoomObject (TOPRIGHT, roomString.Substring (3, 1), roomString.Substring (1, 1), type, template).ToList ();
					goTRList.Randomize ();

					if (goTRList.FirstOrDefault () != null) {
						GameObject.Instantiate (goTRList[0], pos, Quaternion.identity, parent.transform);
					}
				}

				// BL
				if (!allowEmpty || (UnityEngine.Random.Range (0, 2) == 0)) {
					List<GameObject> goBList = FindRoomObject (BOTTOMLEFT, roomString.Substring (0, 1), roomString.Substring (2, 1), type, template).ToList ();
					goBList.Randomize ();

					if (goBList.FirstOrDefault () != null) {
						GameObject.Instantiate (goBList[0], pos, Quaternion.identity, parent.transform);
					}
				}

				// BR
				if (!allowEmpty || (UnityEngine.Random.Range (0, 2) == 0)) {
					List<GameObject> goBRist = FindRoomObject (BOTTOMRIGHT, roomString.Substring (3, 1), roomString.Substring (2, 1), type, template).ToList ();
					goBRist.Randomize ();

					if (goBRist.FirstOrDefault () != null) {
						GameObject.Instantiate (goBRist[0], pos, Quaternion.identity, parent.transform);
					}
				}
			}

			IEnumerable<GameObject> FindRoomObject (string corner, string wall1, string wall2, RoomObjectType type, LevelGenTemplate levelGenTemplate) {

				IEnumerable<GameObject> results = null;

				switch (type) {
					case RoomObjectType.Walls:
						results = levelGenTemplate.levelElements.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						break;

					case RoomObjectType.Entry:
						results = levelGenTemplate.startProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						if (results.Count () == 0) {
							results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						}

						break;

					case RoomObjectType.End:
						results = levelGenTemplate.endProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						if (results.Count () == 0) {
							results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
						}

						break;

					case RoomObjectType.Random:

						results = levelGenTemplate.randomProps.Where (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));

						break;

				}

				return results;

			}

			bool MatchSpriteName (string name, string corner, string wall1, string wall2) {
				string[] nameSplit = name.ToLower ().Split ('_');

				if (nameSplit.Length != 4) {
					Debug.Log ("Invalid sprite name");
					return false;
				}

				string open = "oaxy";
				string door = "daxz";
				string wall = "wayz";
				string exit = "edaxy";

				string first = nameSplit[3].Substring (0, 1);
				string second = nameSplit[3].Substring (1, 1);

				return (nameSplit[2] == corner.ToLower () &&
					((wall1.ToLower () == "o" && open.IndexOf (first) >= 0) ||
						(wall1.ToLower () == "d" && door.IndexOf (first) >= 0) ||
						(wall1.ToLower () == "w" && wall.IndexOf (first) >= 0) ||
						(wall1.ToLower() == "e" && exit.IndexOf(first) >= 0)) &&
					((wall2.ToLower () == "o" && open.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "d" && door.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "w" && wall.IndexOf (second) >= 0) ||
						(wall2.ToLower () == "e" && exit.IndexOf (second) >= 0))						
						);
			}

			void PopulateLevelDoors (Level level) {
				if (!level.template.generateDoors) {
					Debug.Log ("Skipping doors");
					return;
				}

				for (int i = 0; i < level.template.levelHeight; i++) {
					for (int j = 0; j < level.template.levelWidth; j++) {
						ProxyRoom placeholder = level.proxyLevel[j, i];

						if (placeholder != null) {
							if (placeholder.top == RoomSide.Door) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (doorewPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (placeholder.left == RoomSide.Door) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (doornsPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (placeholder.top == RoomSide.Exit) {
								Vector3 pos = new Vector3 (j * 4 + 2, i * -4 + ((level.template.levelHeight - 1) * 4) + 4, 0);
								GameObject.Instantiate (exitewPrefab, pos, Quaternion.identity, levelParent.transform);
							}

							if (placeholder.left == RoomSide.Exit) {
								Vector3 pos = new Vector3 (j * 4, i * -4 + ((level.template.levelHeight - 1) * 4) + 2, 0);
								GameObject.Instantiate (exitnsPrefab, pos, Quaternion.identity, levelParent.transform);
							}							

							

						}
					}
				}
			}

			// Move first half back to generator
			void PopulateEnemySpawns (Level level) {

				if (securityGuardEasyPrefabs.Count <= 0) {
					return;
				}

				for (int i = 0; i < level.enemySpawnLocationList.Count; i++) {

					int rand = UnityEngine.Random.Range (0, securityGuardEasyPrefabs.Count);

					GameObject sggo = securityGuardEasyPrefabs[rand];
					var go = GameObject.Instantiate (sggo, level.ConvertLevelPosToWorld (level.enemySpawnLocationList[i]), Quaternion.identity, npcParent.transform);
				}
			}

			// Move first half back to generator
			void PopulateCameraSpawns (Level level) {

				if (securityCameraPrefabs.Count <= 0) {
					return;
				}

				for (int i = 0; i < level.cameraSpawnLocationList.Count; i++) {

					int rand = UnityEngine.Random.Range (0, securityCameraPrefabs.Count);

					GameObject sggo = securityCameraPrefabs[rand];
					var go = GameObject.Instantiate (sggo, level.ConvertLevelPosToWorld (level.cameraSpawnLocationList[i]), Quaternion.identity, npcParent.transform);
				}
			}			

			enum RoomObjectType {
				Walls,
				Entry,
				End,
				Random
			}

		}
	}
}