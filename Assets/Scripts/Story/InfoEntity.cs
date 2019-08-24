using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "InfoEntity", menuName = "Mouse/Story/Entity")]
		public class InfoEntity : ScriptableObject {
			public bool known;
			//public bool unread;
			public string parentInfoCategory;
			public string description;
		}
	}
}