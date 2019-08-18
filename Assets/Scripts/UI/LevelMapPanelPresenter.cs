﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Level {
        public class LevelMapPanelPresenter : MonoBehaviour {
            // Start is called before the first frame update

            private SelectMenuManager selectMenuManager;

            public List<Sprite> wallSprites;
            public Image locationSprite;
            public Image entrySprite;
            public Image endSprite;
            public Image mapUIPrefab;
            public GameObject wallSpriteParent;

            private Level level;

            void Start () {
                if (locationSprite == null || entrySprite == null || endSprite == null || mapUIPrefab == null || wallSpriteParent == null) {
                    Debug.LogError ("Level Map Panel Presenter defaults not set");
                }
            }

            public void Initialize (SelectMenuManager selectMenuManager, Level level) {
                this.selectMenuManager = selectMenuManager;
                this.level = level;

                DestroyWallSprites ();
                PopulateWallSprites ();
            }

            private void DestroyWallSprites () {

                for (int i = 0; i < wallSpriteParent.transform.childCount; i++) {
                    GameObject.Destroy (wallSpriteParent.transform.GetChild (i).gameObject);
                }
            }

            private void PopulateWallSprites () {

                entrySprite.gameObject.SetActive(false);
                endSprite.gameObject.SetActive(false);

                for (int i = 0; i < level.template.levelHeight; i++) {
                    for (int j = 0; j < level.template.levelWidth; j++) {

                        if (level.map[i].rooms[j] != null) {
                            string chunkString = level.map[i].rooms[j].AsPrintableString ();

                            Sprite blSprite = FindChunkObject ("bl", chunkString.Substring (0, 1), chunkString.Substring (2, 1));
                            Sprite brSprite = FindChunkObject ("br", chunkString.Substring (3, 1), chunkString.Substring (2, 1));

                            Sprite tlSprite = FindChunkObject ("tl", chunkString.Substring (0, 1), chunkString.Substring (1, 1));
                            Sprite trSprite = FindChunkObject ("tr", chunkString.Substring (3, 1), chunkString.Substring (1, 1));

                            Image goBL = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18, (level.template.levelHeight - i) * 18, 0) + this.transform.position, Quaternion.identity, wallSpriteParent.transform);
                            Image goBR = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18 + 9, (level.template.levelHeight - i) * 18, 0) + this.transform.position, Quaternion.identity, wallSpriteParent.transform);
                            Image goTL = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position, Quaternion.identity, wallSpriteParent.transform);
                            Image goTR = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18 + 9, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position, Quaternion.identity, wallSpriteParent.transform);

                            if (level.map[i].rooms[j].isEntry) {
                                entrySprite.transform.position = new Vector3 (j * 18 + 3f, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position;
                                entrySprite.gameObject.SetActive(true);
                            }

                            if (level.map[i].rooms[j].isEnd) {
                                endSprite.transform.position = new Vector3 (j * 18 + 3f, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position;
                                endSprite.gameObject.SetActive(true);
                            }

                            goBL.sprite = blSprite;
                            goBR.sprite = brSprite;
                            goTL.sprite = tlSprite;
                            goTR.sprite = trSprite;

                            //UnityEngine.UI.Image newImg = new UnityEngine.UI.Image();

                        }

                    }

                }
            }

            public void Repaint () {

                if (CoreGame.Instance.CoreState.state == GameState.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.MAP) {
                    if(!this.gameObject.activeInHierarchy) {
                        this.gameObject.SetActive(true);
                    }

                    var pos2d = level.ConvertWorldToLevelPos (CoreGame.Instance.GetPlayer ().transform.position);

                    locationSprite.transform.position = new Vector3 (pos2d.x * 18 + 3f, (level.template.levelHeight - pos2d.y) * 18 + 9, 0) + this.transform.position;
                } else {
                        this.gameObject.SetActive(false);
                }
            }

            public void SetPlayerLocation (Vector3 pos) {
                var pos2d = level.ConvertWorldToLevelPos (pos);

                locationSprite.transform.position = new Vector3 (pos2d.x * 18 + 3f, (level.template.levelHeight - pos2d.y) * 18 + 9, 0) + this.transform.position;

            }

            private Sprite FindChunkObject (string corner, string wall1, string wall2) {
                return wallSprites.First (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
            }

            private bool MatchSpriteName (string name, string corner, string wall1, string wall2) {
                string[] nameSplit = name.ToLower ().Split ('_');

                if (nameSplit.Length != 2) {
                    Debug.Log ("Invalid sprite name");
                    return false;
                }

                // Hack to deal with exits
                wall1 = wall1.Replace('e','d');
                wall2 = wall2.Replace('e','d');

                return (nameSplit[0] == corner.ToLower () &&
                    (wall1.ToLower () == nameSplit[1].Substring (0, 1) && wall2.ToLower () == nameSplit[1].Substring (1, 1)));
            }

            // Update is called once per frame
            void Update () {

            }
        }
    }
}