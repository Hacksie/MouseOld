using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign {
    namespace Map {
        public class MapUI : MonoBehaviour {
            public List<Sprite> mapsprites;
            public Image locationSprite;
            public Image entrySprite;
            public Image endSprite;
            public Image mapUIPrefab;
            public GameObject parent;

            private Level.Level level;

            public void InitMapUI (Level.Level level) {

                this.level = level;

                for (int i = 0; i < parent.transform.childCount; i++) {
                    GameObject.Destroy (parent.transform.GetChild (i).gameObject);
                }

                for (int i = 0; i < level.template.levelHeight; i++) {
                    for (int j = 0; j < level.template.levelWidth; j++) {

                        if (level.proxyLevel[j, i] != null) {
                            string chunkString = level.proxyLevel[j, i].AsPrintableString ();

                            Sprite blSprite = FindChunkObject ("bl", chunkString.Substring (0, 1), chunkString.Substring (2, 1));
                            Sprite brSprite = FindChunkObject ("br", chunkString.Substring (3, 1), chunkString.Substring (2, 1));

                            Sprite tlSprite = FindChunkObject ("tl", chunkString.Substring (0, 1), chunkString.Substring (1, 1));
                            Sprite trSprite = FindChunkObject ("tr", chunkString.Substring (3, 1), chunkString.Substring (1, 1));

                            Image goBL = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18, (level.template.levelHeight - i) * 18, 0) + this.transform.position, Quaternion.identity, parent.transform);
                            Image goBR = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18 + 9, (level.template.levelHeight - i) * 18, 0) + this.transform.position, Quaternion.identity, parent.transform);
                            Image goTL = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position, Quaternion.identity, parent.transform);
                            Image goTR = GameObject.Instantiate<Image> (mapUIPrefab, new Vector3 (j * 18 + 9, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position, Quaternion.identity, parent.transform);

                            if(level.proxyLevel[j, i].isEntry) {
                                entrySprite.transform.position = new Vector3(j * 18 + 3f, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position;
                            }

                            if(level.proxyLevel[j, i].isEnd) {
                                endSprite.transform.position = new Vector3(j * 18 + 3f, (level.template.levelHeight - i) * 18 + 9, 0) + this.transform.position;
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

            public void ShowMap(bool flag)
            {

            }

            public void SetPlayerLocation(Vector3 pos) 
            {
                var pos2d = level.ConvertWorldToLevelPos(pos);

                locationSprite.transform.position = new Vector3(pos2d.x * 18 + 3f, (level.template.levelHeight - pos2d.y) * 18 + 9, 0) + this.transform.position;

            }

            Sprite FindChunkObject (string corner, string wall1, string wall2) {
                return mapsprites.First (g => g != null && MatchSpriteName (g.name, corner, wall1, wall2));
            }

            private bool MatchSpriteName (string name, string corner, string wall1, string wall2) {
                string[] nameSplit = name.ToLower ().Split ('_');

                if (nameSplit.Length != 2) {
                    Debug.Log ("Invalid sprite name");
                    return false;
                }

                return (nameSplit[0] == corner.ToLower () &&
                    (wall1.ToLower () == nameSplit[1].Substring (0, 1) && wall2.ToLower () == nameSplit[1].Substring (1, 1)));
            }

        }
    }
}