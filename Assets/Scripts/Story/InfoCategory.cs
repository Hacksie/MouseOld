using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "InfoCategory", menuName = "Mouse/Story/Category")]
		public class InfoCategory : ScriptableObject {
			public bool available;
		}
	}
}