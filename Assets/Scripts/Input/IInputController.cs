using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Input {

		public interface IInputController {
			bool ShowMobileInput();
			Vector2 GetMovementAxis();
			bool StartButtonUp();
			bool SelectButtonUp();
			void ResetInput();
			bool InteractButtonUp();
		}
	}
}