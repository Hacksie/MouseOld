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
        public RectTransform panel;
        public Text[] line;

        Story.ActionManager actionManager;


        public void Initialize(Story.ActionManager actionManager)
        {
            this.actionManager = actionManager;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
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
            this.gameObject.SetActive(flag);

            if (!flag)
            {
                return;
            }

            panel.sizeDelta = new Vector2(panel.sizeDelta.x, 0);

            var console = this.actionManager.console.Reverse().Take(line.Count()).Reverse().ToArray();
            for (int i = 0; i < line.Count(); i++)
            {
                if (i < console.Count())
                {
                    line[i].text = console[i].message;
                    panel.sizeDelta = new Vector2(panel.sizeDelta.x, panel.sizeDelta.y + line[i].rectTransform.sizeDelta.y);
                }
                else
                {
                    line[i].text = "";
                }
            }
        }
    }
}