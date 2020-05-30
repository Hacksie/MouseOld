using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class ActionConsolePresenter : AbstractPresenter
    {
        public RectTransform panel;
        public Text[] line;

        Story.SceneManager actionManager;


        public void Initialize(Story.SceneManager actionManager)
        {
            this.actionManager = actionManager;
        }

        public override void Repaint()
        {
            UpdateLines();
            /*
            if (GameManager.Instance.GameState.IsPlaying())
            {
                Show();
                
            }
            else
            {
                Hide();
            }*/
        }

        private void UpdateLines()
        {
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, 0);

            //FIXME: fix the GC alloc
            int min = Mathf.Max(actionManager.console.Count() - line.Count(), 0);

            //var console = this.actionManager.console.Reverse().Take(line.Count()).Reverse().ToArray();

            for (int i = line.Count() - 1; i >= 0; i--)
            {
                if (actionManager.console.Count() - 1 - i < min)
                {
                    line[i].text = "";
                }
                else
                {
                    line[i].text = actionManager.console[actionManager.console.Count() - 1 - i].message;
                    panel.sizeDelta = new Vector2(panel.sizeDelta.x, panel.sizeDelta.y + line[i].rectTransform.sizeDelta.y);
                }
            }

            /*
            for (int i = actionManager.console.Count(); i > min; i--)
            {
                line[i].text = actionManager.console[i].message;
                    panel.sizeDelta = new Vector2(panel.sizeDelta.x, panel.sizeDelta.y + line[i].rectTransform.sizeDelta.y);


                if (i < console.Count())
                {
                    line[i].text = console[i].message;
                    panel.sizeDelta = new Vector2(panel.sizeDelta.x, panel.sizeDelta.y + line[i].rectTransform.sizeDelta.y);
                }
                else
                {
                    line[i].text = "";
                }
            }*/
        }
    }
}