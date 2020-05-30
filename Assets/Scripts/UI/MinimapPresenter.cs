using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using HackedDesign.Level;

namespace HackedDesign.UI
{

    public class MinimapPresenter : AbstractPresenter
    {
        public List<GameObject> rooms;
        public List<Image> walls;
        public List<Sprite> wallSprites;

        private Level.Level level;
        public Color hide;
        public Color show;

        private Dictionary<string, Sprite> wallSpriteDictionary = new Dictionary<string, Sprite>();

        private Transform playerTransform;

        private Vector2Int lastMapPosition = Vector2Int.zero;


        public void Initialize(Level.Level level, Transform playerTransform)
        {
            this.playerTransform = playerTransform;
            this.level = level;
            wallSpriteDictionary.Clear();
            PopulateWalls();
            PopulateWallNames();
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

        private void PopulateWallNames() => wallSprites.ForEach(e =>
                                          {
                                              wallSpriteDictionary.Add(e.name.Substring(0, 2) + e.name.Substring(3, 1) + e.name.Substring(4, 1), e);
                                          });        

        public override void Repaint()
        {
            RepaintMap();

            // if (GameManager.Instance.GameState.PlayState == PlayStateEnum.Playing)
            // {
            //     Show();
            //     RepaintMap();
            // }
            // else
            // {
            //     Hide();
            // }
        }

        private void RepaintMap()
        {
            var mapPosition = level.ConvertWorldToLevelPos(playerTransform.position);
            // Only repaint if we change map cell
            if (mapPosition != lastMapPosition)
            {
                lastMapPosition = mapPosition;
                RepaintRoom(0, 0, mapPosition.x - 1, mapPosition.y - 1);
                RepaintRoom(1, 0, mapPosition.x, mapPosition.y - 1);
                RepaintRoom(2, 0, mapPosition.x + 1, mapPosition.y - 1);
                RepaintRoom(0, 1, mapPosition.x - 1, mapPosition.y);
                RepaintRoom(1, 1, mapPosition.x, mapPosition.y);
                RepaintRoom(2, 1, mapPosition.x + 1, mapPosition.y);
                RepaintRoom(0, 2, mapPosition.x - 1, mapPosition.y + 1);
                RepaintRoom(1, 2, mapPosition.x, mapPosition.y + 1);
                RepaintRoom(2, 2, mapPosition.x + 1, mapPosition.y + 1);
            }
        }

        private void RepaintRoom(int minimapX, int minimapY, int mapX, int mapY)
        {
            if (mapY < 0 || mapY >= GameManager.Instance.Data.CurrentLevel.map.Count() || GameManager.Instance.Data.CurrentLevel.map[mapY] == null)
            {
                walls[6 * (minimapY * 3 + minimapX)].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 1].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 2].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 3].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 4].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 5].gameObject.SetActive(false);
                return;
            }

            // When the map gets serialized, the 'empty' rooms become non null, because fuck me, right?
            if (mapX < 0 || mapX >= GameManager.Instance.Data.CurrentLevel.map[mapY].rooms.Count() || GameManager.Instance.Data.CurrentLevel.map[mapY].rooms[mapX] == null || string.IsNullOrWhiteSpace(GameManager.Instance.Data.CurrentLevel.map[mapY].rooms[mapX].left))
            {
                walls[6 * (minimapY * 3 + minimapX)].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 1].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 2].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 3].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 4].gameObject.SetActive(false);
                walls[6 * (minimapY * 3 + minimapX) + 5].gameObject.SetActive(false);
                return;
            }
            ProxyRoom room = GameManager.Instance.Data.CurrentLevel.map[mapY].rooms[mapX];

            Sprite blSprite = FindChunkObject("bl", room.left, room.bottom);
            Sprite brSprite = FindChunkObject("br", room.right, room.bottom);
            Sprite tlSprite = FindChunkObject("tl", room.left, room.top);
            Sprite trSprite = FindChunkObject("tr", room.right, room.top);

            //FIXME: Test for null

            walls[6 * (minimapY * 3 + minimapX)].sprite = tlSprite;
            walls[6 * (minimapY * 3 + minimapX)].gameObject.SetActive(true);
            walls[6 * (minimapY * 3 + minimapX) + 1].sprite = trSprite;
            walls[6 * (minimapY * 3 + minimapX) + 1].gameObject.SetActive(true);
            walls[6 * (minimapY * 3 + minimapX) + 2].sprite = blSprite;
            walls[6 * (minimapY * 3 + minimapX) + 2].gameObject.SetActive(true);
            walls[6 * (minimapY * 3 + minimapX) + 3].sprite = brSprite;
            walls[6 * (minimapY * 3 + minimapX) + 3].gameObject.SetActive(true);
            walls[6 * (minimapY * 3 + minimapX) + 4].gameObject.SetActive(GameManager.Instance.Data.CurrentLevel.map[mapY].rooms[mapX].isEntry);
            walls[6 * (minimapY * 3 + minimapX) + 5].gameObject.SetActive(GameManager.Instance.Data.CurrentLevel.map[mapY].rooms[mapX].isEnd);

        }
        private Sprite FindChunkObject(string corner, string wall1, string wall2)
        {
            // Hack to deal with entry & exits
            wall1 = wall1 == "e" ? "d" : wall1 == "n" ? "d" : wall1;
            wall2 = wall2 == "e" ? "d" : wall2 == "n" ? "d" : wall2;

            string combined = corner + wall1 + wall2;

            if (wallSpriteDictionary.ContainsKey(combined))
            {
                return wallSpriteDictionary[combined];
            }
            return null;
        }
    }
}