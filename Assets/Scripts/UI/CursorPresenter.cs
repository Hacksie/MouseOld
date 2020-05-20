using UnityEngine;

namespace HackedDesign.UI
{
    public class CursorPresenter : AbstractPresenter
    {
        [SerializeField] private bool debugCursor = false;
        public override void Repaint()
        {
            if (GameManager.Instance.IsInGame())
            {
                var show = !GameManager.Instance.GameState.IsPlaying() || debugCursor;

                if(Cursor.visible != show)
                    Cursor.visible = show;
            }
            else
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
            }
        }
    }
}