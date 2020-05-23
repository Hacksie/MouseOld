using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class PsychPanelPresenter : AbstractPresenter
    {
        SelectMenuManager selectMenuManager;

        public void Initialize(SelectMenuManager selectMenuManager)
        {
            this.selectMenuManager = selectMenuManager;
        }

        public override void Repaint()
        {
            if (selectMenuManager.MenuState == SelectMenuSubState.Psych)
            {
                Show();
                RepaintPsych();
            }
            else
            {
                Hide();
            }
        }

        private void RepaintPsych()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}