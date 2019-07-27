using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Input {

		public class AndroidInputController : IInputController {

			MobileInputUI mobileInput;

			bool startButtonDown;
			bool selectButtonDown;

			public AndroidInputController (MobileInputUI mobileInputUI) {
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

				//Debug.Log(mobileAxis);

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
				return UnityEngine.Input.GetButtonUp ("Interact");
			}

			// private bool mobileLeft;
			// private bool mobileRight;
			// private bool mobileUp;
			// private bool mobileDown;

			// public Vector2 GetMovementAxis () {
			// 	return new Vector2 (UnityEngine.Input.GetAxis ("Horizontal"), UnityEngine.Input.GetAxis ("Vertical"));
			// }

			// public void ResetInput () {
			// 	UnityEngine.Input.ResetInputAxes ();
			// }

			// public bool StartButtonUp () {
			// 	return UnityEngine.Input.GetButtonUp ("Start");
			// }

			// public bool SelectButtonUp () {
			// 	return UnityEngine.Input.GetButtonUp ("Select");
			// }

			// public bool InteractButtonUp () {
			// 	return UnityEngine.Input.GetButtonUp ("Interact");
			// }

			// private Vector2 GetMobileAxis () {
			// 	Vector2 mobileAxis = Vector2.zero;
			// 	if (mobileLeft) {
			// 		mobileAxis.x -= 1;
			// 	}

			// 	if (mobileRight) {
			// 		mobileAxis.x += 1;
			// 	}

			// 	if (mobileUp) {
			// 		mobileAxis.y += 1;
			// 	}

			// 	if (mobileDown) {
			// 		mobileAxis.y -= 1;
			// 	}

			// 	return mobileAxis;
			// }

			// private void LeftMobileButtonDown () {
			// 	mobileLeft = true;
			// }

			// private void LeftMobileButtonUp () {
			// 	mobileLeft = false;
			// }

			// private void RightMobileButtonDown () {
			// 	mobileRight = true;
			// }

			// private void RightMobileButtonUp () {
			// 	mobileRight = false;
			// }

			// private void UpMobileButtonDown () {
			// 	mobileUp = true;
			// }

			// private void UpMobileButtonUp () {
			// 	mobileUp = false;
			// }

			// private void DownMobileButtonDown () {
			// 	mobileDown = true;
			// }

			// private void DownMobileButtonUp () {
			// 	mobileDown = false;
			// }

		}
	}
}
/*

namespace HackedDesign {
	public class InputController : MonoBehaviour {

		public FollowCamera follow;
		public Computer computer;
		public PauseMenu pauseMenu;
		public bool mobile = true; 
		public GameObject mobileHUD;

		private bool mobileLeft;
		private bool mobileRight;
		private bool mobileUp;
		private bool mobileDown;
		private bool mobilePauseAction;
		private bool mobilePrimaryAction;
		private bool mobileComputerAction;

		// Use this for initialization
		void Start () {
			mobile = SetMobileInput();
			ShowMobileHUD();
		}
		
		void LateUpdate () {

			var focus = follow.focus;

			if(Time.timeScale == 0)
			{
				mobileHUD.SetActive(false);			
			}
			else if(!(Time.timeScale == 0))
			{
				mobileHUD.SetActive(mobile);

				focus.UpdateMovement(UpdateDirection());

				if(Input.GetButtonUp("Submit") || mobilePrimaryAction)
				{
					focus.Action();
				}	
			}

			if(Input.GetButtonUp("Pause") || (mobile && mobilePauseAction)) 
			{
				pauseMenu.Show();
			}

			if(Input.GetButtonUp("Computer") || (mobile && mobileComputerAction))
			{
				computer.Toggle();
			}			

			mobilePrimaryAction = false;
			mobileComputerAction = false;
			mobilePauseAction = false;
		}


		bool SetMobileInput()
		{
			#pragma warning disable 0162
			#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE	
				return true;
			#endif

			return false;
			#pragma warning restore 0162
		}

		void ShowMobileHUD()
		{
			mobileHUD.SetActive(mobile);
		}		

		private Vector2 UpdateDirection()
		{
			Vector2 nativeAxis = GetNativeAxis();
			Vector2 mobileAxis = Vector2.zero;

			if(mobile)
			{
				mobileAxis = GetMobileAxis();
			}

			return ClampInputAxis(nativeAxis, mobileAxis);
		}

		private Vector2 ClampInputAxis(Vector2 native, Vector2 mobile)
		{
			var clamped = native + mobile;

			clamped.x = clamped.x < -1 ? -1 : clamped.x;
			clamped.x = clamped.x > 1 ? 1 : clamped.x;
			clamped.y = clamped.y < -1 ? -1 : clamped.y;
			clamped.y = clamped.y > 1 ? 1 : clamped.y;
			
			return clamped;
		}

		private Vector2 GetMobileAxis()
		{
			Vector2 mobileAxis = Vector2.zero;
			if(mobileLeft)
			{
				mobileAxis.x -= 1;
			}

			if(mobileRight)
			{
				mobileAxis.x += 1;
			}			

			if(mobileUp)
			{
				mobileAxis.y += 1;
			}				

			if(mobileDown)
			{
				mobileAxis.y -= 1;
			}			

			return mobileAxis;
		}

		private Vector2 GetNativeAxis()
		{
			return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		}

		private void LeftMobileButtonDown()
		{
			mobileLeft = true;
		}

		private void LeftMobileButtonUp()
		{
			mobileLeft = false;
		}		

		private void RightMobileButtonDown()
		{
			mobileRight = true;
		}

		private void RightMobileButtonUp()
		{
			mobileRight = false;
		}	

		private void UpMobileButtonDown()
		{
			mobileUp = true;
		}

		private void UpMobileButtonUp()
		{
			mobileUp = false;
		}	

		private void DownMobileButtonDown()
		{
			mobileDown = true;
		}

		private void DownMobileButtonUp()
		{
			mobileDown = false;
		}	

		private void PrimaryActionMobileButtonClick()
		{
			mobilePrimaryAction = true;
		}						

		private void ComputerMobileButtonClick()
		{
			mobileComputerAction = true;
		}							

		private void PauseMobileButtonClick()
		{
			Debug.Log("pause clicked");
			mobilePauseAction = true;
		}			
	}
}


 */