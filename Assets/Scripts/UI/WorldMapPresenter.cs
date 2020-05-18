using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.Level
{

    public class WorldMapPresenter : MonoBehaviour
    {
        public void Initialize()
        {
            
        }

        public void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.WORLDMAP)
            {
                if (!gameObject.activeInHierarchy)
                {
                    Show(true);
                }
            }
            else if (gameObject.activeInHierarchy)
            {
                Show(false);
            }
        }

        private void Show(bool flag)
        {
            gameObject.SetActive(flag);

            if (!flag)
            {
                return;
            }
        }
    }

}