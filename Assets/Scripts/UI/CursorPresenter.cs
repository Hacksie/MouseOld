using UnityEngine;

namespace HackedDesign {
    public class CursorPresenter : MonoBehaviour {

        public void Repaint () {
            switch (CoreGame.Instance.state.state) {
                case State.GameStateEnum.MAINMENU:
                case State.GameStateEnum.DIALOGUE:
                case State.GameStateEnum.NARRATION:
                case State.GameStateEnum.SELECTMENU:
                case State.GameStateEnum.STARTMENU:
                case State.GameStateEnum.WORLDMAP:
                case State.GameStateEnum.GAMEOVER:
                case State.GameStateEnum.MISSIONCOMPLETE:
                case State.GameStateEnum.CAPTURED:
                    Cursor.visible = true;
                    break;
                default:
                    //Cursor.visible = false;
                    break;
            }
        }

    }
}