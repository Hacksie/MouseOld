using UnityEngine;

namespace HackedDesign.UI
{
    public class CursorPresenter : AbstractPresenter
    {
        [SerializeField] private bool debugCursor = false;
        public override void Repaint()
        {
            switch (GameManager.Instance.state.state)
            {
                case GameStateEnum.MAINMENU:
                case GameStateEnum.DIALOGUE:
                case GameStateEnum.NARRATION:
                case GameStateEnum.SELECTMENU:
                case GameStateEnum.STARTMENU:
                case GameStateEnum.WORLDMAP:
                case GameStateEnum.GAMEOVER:
                case GameStateEnum.MISSIONCOMPLETE:
                case GameStateEnum.CAPTURED:
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