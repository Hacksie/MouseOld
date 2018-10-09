using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Character {
		[CreateAssetMenu (fileName = "Character", menuName = "Mouse/Character/Character")]
		public class Character : ScriptableObject {
			public string fullName;
			public string handle;
			//public Corp corp;
			public string serial;
			public Sprite avatar;
		}
	}
}