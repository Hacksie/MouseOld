using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public abstract class AbstractPresenter : MonoBehaviour
    {
        public virtual void Show()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Hide()
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