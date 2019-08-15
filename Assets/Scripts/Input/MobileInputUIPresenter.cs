using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Input {
        public class MobileInputUIPresenter : MonoBehaviour {
            public bool mobileUp;
            public bool mobileLeft;
            public bool mobileRight;
            public bool mobileDown;
            public bool mobileStart;
            public bool mobileSelect;

            public void Repaint () {
                if (CoreGame.Instance.CoreState.state == GameState.PLAYING && CoreGame.Instance.inputController.ShowMobileInput ()) {
                    if (!this.gameObject.activeInHierarchy) {
                        this.gameObject.SetActive (true);
                    }
                } else if (this.gameObject.activeInHierarchy) {
                    this.gameObject.SetActive (false);

                }

            }

            public void LeftMobileButtonDown () {
                mobileLeft = true;
            }

            public void LeftMobileButtonUp () {
                mobileLeft = false;
            }

            public void RightMobileButtonDown () {
                mobileRight = true;
            }

            public void RightMobileButtonUp () {
                mobileRight = false;
            }

            public void UpMobileButtonDown () {
                mobileUp = true;
            }

            public void UpMobileButtonUp () {
                mobileUp = false;
            }

            public void DownMobileButtonDown () {
                mobileDown = true;
            }

            public void DownMobileButtonUp () {
                mobileDown = false;
            }

            public void SelectMobileButtonDown () {
                mobileSelect = true;
            }

            public void SelectMobileButtonUp () {
                mobileSelect = false;
            }

            public void StartMobileButtonDown () {
                mobileStart = true;
            }

            public void StartMobileButtonUp () {
                mobileStart = false;
            }
        }
    }
}