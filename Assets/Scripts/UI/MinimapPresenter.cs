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

        private List<WallSpriteDetails> wallSpritesDetails = new List<WallSpriteDetails>();


        public void Initialize(Level.Level level)
        {
            this.level = level;
            PopulateWalls();
            PopulateWallNames();
        }

        private void PopulateWallNames()
        {
            wallSprites.ForEach(e => wallSpritesDetails.Add(new WallSpriteDetails { name = e.name, corner = e.name.Substring(0, 2), wall1 = e.name.Substring(3, 1), wall2 = e.name.Substring(4, 1) }));
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

        public override void Repaint()
        {
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.Playing)
            {
                Show();
                RepaintMap();
            }
            else
            {
                Hide();
            }
        }

        private void RepaintMap()
        {
            var pos2d = level.ConvertWorldToLevelPos(GameManager.Instance.GetPlayer().transform.position);
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
            if (y < 0 || y >= GameManager.Instance.GameState.CurrentLevel.map.Count() || GameManager.Instance.GameState.CurrentLevel.map[y] == null)
            {
                walls[6 * (j * 3 + i)].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 1].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 2].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 3].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 4].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 5].gameObject.SetActive(false);
                return;
            }

            // When the map gets serialized, the 'empty' rooms become non null, because fuck me, right?
            if (x < 0 || x >= GameManager.Instance.GameState.CurrentLevel.map[y].rooms.Count() || GameManager.Instance.GameState.CurrentLevel.map[y].rooms[x] == null || string.IsNullOrWhiteSpace(GameManager.Instance.GameState.CurrentLevel.map[y].rooms[x].left))
            {
                walls[6 * (j * 3 + i)].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 1].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 2].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 3].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 4].gameObject.SetActive(false);
                walls[6 * (j * 3 + i) + 5].gameObject.SetActive(false);
                return;
            }
            ProxyRoom room = GameManager.Instance.GameState.CurrentLevel.map[y].rooms[x];

            Sprite blSprite = FindChunkObject("bl", room.left, room.bottom);
            Sprite brSprite = FindChunkObject("br", room.right, room.bottom);
            Sprite tlSprite = FindChunkObject("tl", room.left, room.top);
            Sprite trSprite = FindChunkObject("tr", room.right, room.top);

            //FIXME: Test for null

            walls[6 * (j * 3 + i)].sprite = tlSprite;
            walls[6 * (j * 3 + i)].gameObject.SetActive(true);
            walls[6 * (j * 3 + i) + 1].sprite = trSprite;
            walls[6 * (j * 3 + i) + 1].gameObject.SetActive(true);
            walls[6 * (j * 3 + i) + 2].sprite = blSprite;
            walls[6 * (j * 3 + i) + 2].gameObject.SetActive(true);
            walls[6 * (j * 3 + i) + 3].sprite = brSprite;
            walls[6 * (j * 3 + i) + 3].gameObject.SetActive(true);
            walls[6 * (j * 3 + i) + 4].gameObject.SetActive(GameManager.Instance.GameState.CurrentLevel.map[y].rooms[x].isEntry);
            walls[6 * (j * 3 + i) + 5].gameObject.SetActive(GameManager.Instance.GameState.CurrentLevel.map[y].rooms[x].isEnd);

        }
        private Sprite FindChunkObject(string corner, string wall1, string wall2)
        {
            for (int i = 0; i < wallSprites.Count; i++)
            {
                if (wallSprites[i] != null && MatchSpriteName(wallSpritesDetails[i], corner, wall1, wall2))
                {
                    return wallSprites[i];
                }
            }
            return null;
        }

        private bool MatchSpriteName(WallSpriteDetails spriteDetails, string corner, string wall1, string wall2)
        {
            // Hack to deal with entry & exits
            wall1 = wall1 == "e" ? "d" : wall1 == "n" ? "d" : wall1;
            wall2 = wall2 == "e" ? "d" : wall2 == "n" ? "d" : wall2;

            return (spriteDetails.corner == corner && (wall1 == spriteDetails.wall1 && wall2 == spriteDetails.wall2));
        }

        struct WallSpriteDetails
        {
            public string name;
            public string corner;
            public string wall1;
            public string wall2;
        }
    }

}