using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

namespace HackedDesign
{
    public class StatsPanelPresenter : MonoBehaviour
    {
        public GameObject fullBattery;
        public GameObject halfBattery;
        public GameObject almostBattery;
        public GameObject emptyBattery;
        public Image keycard1;
        public Image keycard2;
        public Image keycard3;
        public Image keycard4;
        public Image keycard5;
        public Color keycardEmptyColour;
        public Color keycardFullColour;

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
            RepaintKeycards();
            RepaintBattery();
        }

        private void RepaintKeycards()
        {
            keycard1.color = CoreGame.Instance.State.player.keycards >= 1 ? keycardFullColour : keycardEmptyColour;
            keycard2.color = CoreGame.Instance.State.player.keycards >= 2 ? keycardFullColour : keycardEmptyColour;
            keycard3.color = CoreGame.Instance.State.player.keycards >= 3 ? keycardFullColour : keycardEmptyColour;
            keycard4.color = CoreGame.Instance.State.player.keycards >= 4 ? keycardFullColour : keycardEmptyColour;
            keycard5.color = CoreGame.Instance.State.player.keycards >= 5 ? keycardFullColour : keycardEmptyColour;
            //keycard5.SetActive(CoreGame.Instance.State.player.keycards == 5);
            //keycard4.SetActive(CoreGame.Instance.State.player.keycards >= 4);
            //keycard3.SetActive(CoreGame.Instance.State.player.keycards >= 3);
            //keycard2.SetActive(CoreGame.Instance.State.player.keycards >= 2);
            //keycard1.SetActive(CoreGame.Instance.State.player.keycards >= 1);
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