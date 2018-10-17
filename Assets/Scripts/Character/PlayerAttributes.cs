using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Character {
		[CreateAssetMenu (fileName = "PlayerAttributes", menuName = "Mouse/Character/PlayerAttributes")]
		public class PlayerAttributes : ScriptableObject {
			public int charisma = 0;
			public int intimidation = 0;
			public int software = 0;
			public int hardware = 0;
		}
	}
}