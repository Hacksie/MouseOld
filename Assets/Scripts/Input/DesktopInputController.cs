using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Input {

		public class DesktopInputController : IInputController {
			public Vector2 GetMovementAxis () {
				return new Vector2 (UnityEngine.Input.GetAxis ("Horizontal"), UnityEngine.Input.GetAxis ("Vertical"));
			}

			public bool StartButtonUp () {
				return UnityEngine.Input.GetButtonUp ("Start");
			}

			public bool SelectButtonUp () {
				return UnityEngine.Input.GetButtonUp ("Select");
			}

			public void ResetInput () {
				UnityEngine.Input.ResetInputAxes ();
			}
		}
	}
}