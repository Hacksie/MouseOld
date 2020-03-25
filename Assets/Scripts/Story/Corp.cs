using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu(fileName = "Corp", menuName = "Mouse/Content/Corp")]
		[System.Serializable]
		public class Corp : InfoEntity {
			public string color;
			public Sprite logo;
		}
	}
}