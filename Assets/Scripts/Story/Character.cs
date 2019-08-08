using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "Story", menuName = "Mouse/Story/Character")]
		public class Character : InfoEntity {
			public string fullName;
			public string handle;
			public Corp corp;
			public string serial;
			public Sprite avatar;
			public CharacterStatus status;

			public enum CharacterStatus {
				Contractor,
				Manager,
				Unemployed,
				AI
			}
		
		}

		
	}
}