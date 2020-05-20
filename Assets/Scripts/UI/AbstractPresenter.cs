using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public abstract class AbstractPresenter : MonoBehaviour
    {
        public void Show()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }

        protected void Toggle()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        public abstract void Repaint();
    }
}