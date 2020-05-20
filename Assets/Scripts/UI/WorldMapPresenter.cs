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
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.Worldmap)
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