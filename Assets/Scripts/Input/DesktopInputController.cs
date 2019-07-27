﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Input {

		public class DesktopInputController : IInputController {

			public void SetMobileInput(MobileInputUI mobileInputUI) 
			{
				// do nothing;
			}

			public bool ShowMobileInput(){
				return false;
			}


			public Vector2 GetMovementAxis () {
				return new Vector2 (UnityEngine.Input.GetAxis ("Horizontal"), UnityEngine.Input.GetAxis ("Vertical"));
			}

			public void ResetInput () {
				UnityEngine.Input.ResetInputAxes ();
			}		

			public bool StartButtonUp () {
				return UnityEngine.Input.GetButtonUp ("Start");
			}

			public bool SelectButtonUp () {
				return UnityEngine.Input.GetButtonUp ("Select");
			}

			public bool InteractButtonUp() {
				return UnityEngine.Input.GetButtonUp("Interact");
			}
		}
	}
}