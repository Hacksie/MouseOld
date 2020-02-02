using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class StatsPanelPresenter : MonoBehaviour
    {
        public GameObject fullBattery;
        public GameObject halfBattery;
        public GameObject almostBattery;
        public GameObject emptyBattery;

        public void Initialize()
        {

        }

        public void Repaint()
        {
            if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
            {
                if (!this.gameObject.activeInHierarchy)
                {
                    Show(true);
                }
                RepaintStats();
            }
            else if (this.gameObject.activeInHierarchy)
            {
                Show(false);
            }
        }

        private void Show(bool flag)
        {
            this.gameObject.SetActive(flag);
        }

        private void RepaintStats()
        {
            RepaintBattery();
        }
        private void RepaintBattery()
        {
            float percent = 1.0f * CoreGame.Instance.State.player.battery / CoreGame.Instance.State.player.maxBattery;
            if (percent >= .60)
            {
                fullBattery.SetActive(true);
                halfBattery.SetActive(false);
                almostBattery.SetActive(false);
                emptyBattery.SetActive(false);
            }
            else if (percent >= .25)
            {
                fullBattery.SetActive(false);
                halfBattery.SetActive(true);
                almostBattery.SetActive(false);
                emptyBattery.SetActive(false);
            }
            else if (percent > 0)
            {
                fullBattery.SetActive(false);
                halfBattery.SetActive(false);
                almostBattery.SetActive(true);
                emptyBattery.SetActive(false);
            }
            else
            {
                fullBattery.SetActive(false);
                halfBattery.SetActive(false);
                almostBattery.SetActive(false);
                emptyBattery.SetActive(true);
            }
        }
    }
}