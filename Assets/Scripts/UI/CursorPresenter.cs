using UnityEngine;

namespace HackedDesign {
    public class CursorPresenter : MonoBehaviour {

        public void Repaint () {
            switch (CoreGame.Instance.CoreState.state) {
                case GameState.MAINMENU:
                case GameState.DIALOGUE:
                case GameState.NARRATION:
                case GameState.SELECTMENU:
                case GameState.STARTMENU:
                case GameState.WORLDMAP:
                case GameState.GAMEOVER:
                    Cursor.visible = true;
                    break;
                default:
                    Cursor.visible = false;
                    break;
            }
        }

    }
}