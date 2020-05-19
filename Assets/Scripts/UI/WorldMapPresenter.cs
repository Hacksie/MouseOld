using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class WorldMapPresenter : AbstractPresenter
    {
        public void Initialize()
        {

        }

        public override void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.WORLDMAP)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}