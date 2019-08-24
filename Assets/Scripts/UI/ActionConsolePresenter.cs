using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign
{
    public class ActionConsolePresenter : MonoBehaviour
    {
        Story.ActionManager actionManager;
        public Text[] line;

        public void Initialize(Story.ActionManager actionManager)
        {
            this.actionManager = actionManager;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.CoreState.state == GameState.PLAYING)
            {
                Show(true);
            }
            else
            {
                Show(false);
            }

        }

        private void Show(bool flag)
        {
            Debug.Log(this.name + ": show narration " + flag);

            this.gameObject.SetActive(flag);

            if (!flag)
            {
                return;
            }

            var console = this.actionManager.console.Reverse().Take(6).Reverse().ToArray();
            for (int i = 0; i < 6; i++)
            {
                if (i < console.Count())
                {
                    line[i].text = console[i].message;
                }
                else
                {
                    line[i].text = "";
                }
            }
        }
    }

}