using UnityEngine;

namespace HackedDesign {
    public class CursorPresenter : MonoBehaviour {

        public void Repaint () {
            switch (CoreGame.Instance.state.state) {
                case GameState.GameStateEnum.MAINMENU:
                case GameState.GameStateEnum.DIALOGUE:
                case GameState.GameStateEnum.NARRATION:
                case GameState.GameStateEnum.SELECTMENU:
                case GameState.GameStateEnum.STARTMENU:
                case GameState.GameStateEnum.WORLDMAP:
                case GameState.GameStateEnum.GAMEOVER:
                case GameState.GameStateEnum.MISSIONCOMPLETE:
                case GameState.GameStateEnum.CAPTURED:
                    Cursor.visible = true;
                    break;
                default:
                    //Cursor.visible = false;
                    break;
            }
        }

    }
}