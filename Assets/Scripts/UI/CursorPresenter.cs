using UnityEngine;

namespace HackedDesign.UI
{
    public class CursorPresenter : AbstractPresenter
    {
        [SerializeField] private bool debugCursor = false;
        public override void Repaint()
        {
            switch (CoreGame.Instance.state.state)
            {
                case GameState.GameStateEnum.MAINMENU:
                case GameState.GameStateEnum.DIALOGUE:
                case GameState.GameStateEnum.NARRATION:
                case GameState.GameStateEnum.SELECTMENU:
                case GameState.GameStateEnum.STARTMENU:
                case GameState.GameStateEnum.WORLDMAP:
                case GameState.GameStateEnum.GAMEOVER:
                case GameState.GameStateEnum.MISSIONCOMPLETE:
                case GameState.GameStateEnum.CAPTURED:
                    if (!Cursor.visible)
                        Cursor.visible = true;
                    break;
                default:
                    if (Cursor.visible && !debugCursor)
                         Cursor.visible = false;
                    break;
            }
        }

    }
}