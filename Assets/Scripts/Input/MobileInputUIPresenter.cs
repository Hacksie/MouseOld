using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign
{
    namespace Input
    {
        public class MobileInputUIPresenter : MonoBehaviour
        {
            public RectTransform joystickRect;
            public Transform joystickKnob;
            public bool mobileUp;
            public bool mobileLeft;
            public bool mobileRight;
            public bool mobileDown;
            public bool mobileStart;
            public bool mobileSelect;
            public bool mobileInteract;
            public bool mobileOverload;
            public bool mobileHack;
            public bool mobileKeycard;
            public bool mobileBug;
            public Vector2 mobileAxis;
            private Input.IInputController inputController;
            private Vector2 mobilePointerPosition;



            public void Initialize(Input.IInputController inputController)
            {
                this.inputController = inputController;
                UnityEngine.Input.simulateMouseWithTouches = true;
            }

            public void Repaint()
            {
                if (CoreGame.Instance.State.state == GameStateEnum.PLAYING && inputController.ShowMobileInput())
                {
                    if (!this.gameObject.activeInHierarchy)
                    {
                        this.gameObject.SetActive(true);
                    }
                    UpdateTouch();
                }
                else if (this.gameObject.activeInHierarchy)
                {
                    this.gameObject.SetActive(false);

                }
            }

            public void UpdateTouch()
            {
                //  public static Rect RectTransformToScreenSpace(RectTransform transform)
                //  {
                //      Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
                //      Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
                //      rect.x -= (transform.pivot.x * size.x);
                //      rect.y -= ((1.0f - transform.pivot.y) * size.y);
                //      return rect;
                //  }

            
                /*
                if (UnityEngine.Input.GetMouseButton(0) && RectTransformToScreenSpace(joystickRect).Contains(UnityEngine.Input.mousePosition))
                {

                    //Debug.Log(UnityEngine.Input.mousePosition);
                    joystickKnob.position = UnityEngine.Input.mousePosition;
                    //Debug.Log(joystickRect.rect.x + ": " + joystickRect.rect.y + ": " + )
                } 
                else {
                    joystickKnob.position = joystickRect.position;
                }

                mobileAxis = (joystickKnob.position - joystickRect.position).normalized;*/
            }

            private Rect RectTransformToScreenSpace(RectTransform transform)
            {


                Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
                return new Rect((Vector2)transform.position - (size * 0.5f), size);
            }

            public Vector2 GetAxis()
            {
                return mobileAxis;
            }

            public void LeftMobileButtonDown()
            {
                mobileLeft = true;
            }

            public void LeftMobileButtonUp()
            {
                mobileLeft = false;
            }

            public void RightMobileButtonDown()
            {
                mobileRight = true;
            }

            public void RightMobileButtonUp()
            {
                mobileRight = false;
            }

            public void UpMobileButtonDown()
            {
                mobileUp = true;
            }

            public void UpMobileButtonUp()
            {
                mobileUp = false;
            }

            public void DownMobileButtonDown()
            {
                mobileDown = true;
            }

            public void DownMobileButtonUp()
            {
                mobileDown = false;
            }

            public void SelectMobileButtonDown()
            {
                mobileSelect = true;
            }

            public void SelectMobileButtonUp()
            {
                mobileSelect = false;
            }

            public void StartMobileButtonDown()
            {
                mobileStart = true;
            }

            public void StartMobileButtonUp()
            {
                mobileStart = false;
            }

            public void InteractMobileButtonDown()
            {
                mobileInteract = true;
            }

            public void InteractMobileButtonUp()
            {
                mobileInteract = false;
            }

            public void BugMobileButtonDown()
            {
                mobileBug = true;
            }

            public void BugMobileButtonUp()
            {
                mobileBug = false;
            }

            public void HackMobileButtonDown()
            {
                mobileHack = true;
            }

            public void HackMobileButtonUp()
            {
                mobileHack = false;
            }

            public void OverloadMobileButtonDown()
            {
                mobileOverload = true;
            }

            public void OverloadMobileButtonUp()
            {
                mobileOverload = false;
            }
        }
    }
}