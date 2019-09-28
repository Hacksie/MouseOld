using UnityEngine;

namespace HackedDesign {
    public class CursorPresenter : MonoBehaviour {

        public void Repaint () {
            switch (CoreGame.Instance.State.state) {
                case GameStateEnum.MAINMENU:
                case GameStateEnum.DIALOGUE:
                case GameStateEnum.NARRATION:
                case GameStateEnum.SELECTMENU:
                case GameStateEnum.STARTMENU:
                case GameStateEnum.WORLDMAP:
                case GameStateEnum.GAMEOVER:
                    Cursor.visible = true;
                    break;
                default:
                    Cursor.visible = false;
                    break;
            }
        }

    }
}