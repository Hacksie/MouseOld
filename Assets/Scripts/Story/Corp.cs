using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "Story", menuName = "Mouse/Story/Corp")]
		public class Corp : InfoEntity {
			public Color color;
			public Sprite logo;
		}
	}
}