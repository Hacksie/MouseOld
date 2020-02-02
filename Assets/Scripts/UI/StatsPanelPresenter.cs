using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
        public class StatsPanelPresenter : MonoBehaviour
        {

            //public GameObject taskButtonParent;
            //public GameObject taskButtonPrefab;
            //public UnityEngine.UI.Text taskDescription;

            

            public void Initialize()
            {
                
            }

            public void Repaint()
            {
                if (CoreGame.Instance.State.state == GameStateEnum.PLAYING)
                {
                    if (!this.gameObject.activeInHierarchy)
                    {
                        RepaintStats();
                        Show(true);
                    }
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
             
            }
        }
}