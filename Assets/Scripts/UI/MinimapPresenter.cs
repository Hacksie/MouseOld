using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign
{
    namespace Level
    {
        public class MinimapPresenter : MonoBehaviour
        {
            public GameObject locationSprite;
            public GameObject entrySprite;
            public GameObject endSprite;
            public GameObject wallSpriteParent;
            public List<GameObject> rooms;
            public List<Image> walls;
            public List<Sprite> wallSprites;

            private Level level;
            public Color hide;
            public Color show;
            
            

            void Start()
            {
                // if (locationSprite == null || entrySprite == null || endSprite == null ||  wallSpriteParent == null)
                // {
                //     Debug.LogError("Mini Map Panel Presenter defaults not set");
                // }
            }

            public void Initialize(Level level)
            {
                this.level = level;
                PopulateWalls();
            }

            private void PopulateWalls()
            {
                for (int i = 0; i < 9; i++)
                {
                    walls.Add(rooms[i].transform.GetChild(0).GetComponent<Image>());
                    walls.Add(rooms[i].transform.GetChild(1).GetComponent<Image>());
                    walls.Add(rooms[i].transform.GetChild(2).GetComponent<Image>());
                    walls.Add(rooms[i].transform.GetChild(3).GetComponent<Image>());
                    walls.Add(rooms[i].transform.GetChild(4).GetComponent<Image>());
                    walls.Add(rooms[i].transform.GetChild(5).GetComponent<Image>());
                }
            }

            public void Repaint()
            {
                if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
                {
                    if (!this.gameObject.activeInHierarchy)
                    {
                        Show(true);
                    }
                    RepaintMap();
                }
                else if (this.gameObject.activeInHierarchy)
                {
                    Show(false);
                }
            }

            private void Show(bool flag)
            {
                this.gameObject.SetActive(flag);
            }

            private void RepaintMap()
            {
                var pos2d = level.ConvertWorldToLevelPos(CoreGame.Instance.GetPlayer().transform.position);
                RepaintRoom(0, 0, pos2d.x - 1, pos2d.y - 1);
                RepaintRoom(1, 0, pos2d.x, pos2d.y - 1);
                RepaintRoom(2, 0, pos2d.x + 1, pos2d.y - 1);
                RepaintRoom(0, 1, pos2d.x - 1, pos2d.y);
                RepaintRoom(1, 1, pos2d.x, pos2d.y);
                RepaintRoom(2, 1, pos2d.x + 1, pos2d.y);
                RepaintRoom(0, 2, pos2d.x - 1, pos2d.y + 1);
                RepaintRoom(1, 2, pos2d.x, pos2d.y + 1);
                RepaintRoom(2, 2, pos2d.x + 1, pos2d.y + 1);
            }

            private void RepaintRoom(int i, int j, int x, int y)
            {
                if (y < 0 || y >= CoreGame.Instance.State.currentLevel.map.Count() || CoreGame.Instance.State.currentLevel.map[y] == null)
                {
                    walls[6 * (j * 3 + i)].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+1].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+2].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+3].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+4].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+5].gameObject.SetActive(false);
                    return;
                }

                // When the map gets serialized, the 'empty' rooms become non empty, because fuck me, right?
                if (x < 0 || x >= CoreGame.Instance.State.currentLevel.map[y].rooms.Count() || CoreGame.Instance.State.currentLevel.map[y].rooms[x] == null || string.IsNullOrWhiteSpace(CoreGame.Instance.State.currentLevel.map[y].rooms[x].AsPrintableString()))
                {
                    walls[6 * (j * 3 + i)].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+1].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+2].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+3].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+4].gameObject.SetActive(false);
                    walls[6 * (j * 3 + i)+5].gameObject.SetActive(false);
                    return;
                }

                string chunkString = CoreGame.Instance.State.currentLevel.map[y].rooms[x].AsPrintableString();

                Sprite blSprite = FindChunkObject("bl", chunkString.Substring(0, 1), chunkString.Substring(2, 1));
                Sprite brSprite = FindChunkObject("br", chunkString.Substring(3, 1), chunkString.Substring(2, 1));

                Sprite tlSprite = FindChunkObject("tl", chunkString.Substring(0, 1), chunkString.Substring(1, 1));
                Sprite trSprite = FindChunkObject("tr", chunkString.Substring(3, 1), chunkString.Substring(1, 1));

                walls[6 * (j * 3 + i)].sprite = tlSprite;
                walls[6 * (j * 3 + i)].gameObject.SetActive(true);
                walls[6 * (j * 3 + i) + 1].sprite = trSprite;
                walls[6 * (j * 3 + i) + 1].gameObject.SetActive(true);
                walls[6 * (j * 3 + i) + 2].sprite = blSprite;
                walls[6 * (j * 3 + i) + 2].gameObject.SetActive(true);
                walls[6 * (j * 3 + i) + 3].sprite = brSprite;
                walls[6 * (j * 3 + i) + 3].gameObject.SetActive(true);
                walls[6 * (j * 3 + i) + 4].gameObject.SetActive(CoreGame.Instance.State.currentLevel.map[y].rooms[x].isEntry);
                walls[6 * (j * 3 + i) + 5].gameObject.SetActive(CoreGame.Instance.State.currentLevel.map[y].rooms[x].isEnd); 

            }
            private Sprite FindChunkObject(string corner, string wall1, string wall2)
            {
                return wallSprites.First(g => g != null && MatchSpriteName(g.name, corner, wall1, wall2));
            }

            private bool MatchSpriteName(string name, string corner, string wall1, string wall2)
            {
                string[] nameSplit = name.ToLower().Split('_');

                if (nameSplit.Length != 2)
                {
                    Debug.Log("Invalid sprite name");
                    return false;
                }

                // Hack to deal with entry & exits
                wall1 = wall1.Replace('e', 'd').Replace('n', 'd');
                wall2 = wall2.Replace('e', 'd').Replace('n', 'd');

                return (nameSplit[0] == corner.ToLower() &&
                    (wall1.ToLower() == nameSplit[1].Substring(0, 1) && wall2.ToLower() == nameSplit[1].Substring(1, 1)));
            }

        }
    }
}