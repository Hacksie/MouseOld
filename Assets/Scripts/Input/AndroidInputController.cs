using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Input {

		public class AndroidInputController : IInputController {

			MobileInputUIPresenter mobileInput;

			bool startButtonDown;
			bool selectButtonDown;
			bool interactButtonDown;
			bool overloadButtonDown;
			bool hackButtonDown;
			bool keycardButtonDown;

			public AndroidInputController (MobileInputUIPresenter mobileInputUI) {
				this.mobileInput = mobileInputUI;
			}

			public bool ShowMobileInput () {
				return true;
			}

			private Vector2 GetMobileAxis () {
				Vector2 mobileAxis = Vector2.zero;
				if (mobileInput.mobileLeft) {
					mobileAxis.x -= 1;
				}

				if (mobileInput.mobileRight) {
					mobileAxis.x += 1;
				}

				if (mobileInput.mobileUp) {
					mobileAxis.y += 1;
				}

				if (mobileInput.mobileDown) {
					mobileAxis.y -= 1;
				}

				return mobileAxis;
			}

			public Vector2 GetMovementAxis () {
				Vector2 nativeAxis = GetNativeAxis ();
				Vector2 mobileAxis = GetMobileAxis ();
				return ClampInputAxis (nativeAxis, mobileAxis);
			}

			public Vector2 GetNativeAxis () {
				return new Vector2 (UnityEngine.Input.GetAxis ("Horizontal"), UnityEngine.Input.GetAxis ("Vertical"));
			}

			private Vector2 ClampInputAxis (Vector2 native, Vector2 mobile) {
				var clamped = native + mobile;

				clamped.x = clamped.x < -1 ? -1 : clamped.x;
				clamped.x = clamped.x > 1 ? 1 : clamped.x;
				clamped.y = clamped.y < -1 ? -1 : clamped.y;
				clamped.y = clamped.y > 1 ? 1 : clamped.y;

				return clamped;
			}

			public void ResetInput () {
				UnityEngine.Input.ResetInputAxes ();
			}

			public bool StartButtonUp () {

				if (mobileInput.mobileStart) {
					startButtonDown = true;
				} else if (startButtonDown) {
					startButtonDown = false;
					return true;

				}

				return UnityEngine.Input.GetButtonUp ("Start");
			}

			public bool SelectButtonUp () {

				if (mobileInput.mobileSelect) {
					selectButtonDown = true;
				} else if (selectButtonDown) {
					selectButtonDown = false;
					return true;
				}
				return UnityEngine.Input.GetButtonUp ("Select");
			}

			public bool InteractButtonUp () {
				if(mobileInput.mobileInteract) {
					interactButtonDown = true;
				} else if(interactButtonDown) {
					interactButtonDown = false;
					return true;
				}
				return UnityEngine.Input.GetButtonUp ("Interact");
			}

			public bool OverloadButtonUp () {
				if(mobileInput.mobileOverload) {
					overloadButtonDown = true;
				} else if(overloadButtonDown) {
					overloadButtonDown = false;
					return true;
				}
				return UnityEngine.Input.GetButtonUp ("Overload");
			}	

			public bool HackButtonUp() {
				if(mobileInput.mobileHack) {
					hackButtonDown = true;
				} else if(hackButtonDown) {
					hackButtonDown = false;
					return true;
				}
				return UnityEngine.Input.GetButtonUp("Hack");
			}

			public bool KeycardButtonUp() {
				if(mobileInput.mobileKeycard) {
					keycardButtonDown = true;
				} else if(keycardButtonDown) {
					keycardButtonDown = false;
					return true;
				}
				return UnityEngine.Input.GetButtonUp("Keycard");
			}				
		}
	}
}
