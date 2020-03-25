using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {

		[CreateAssetMenu(fileName = "InfoEntity", menuName = "Mouse/Content/Info Entity")]
		[System.Serializable]
		public class InfoEntity : ScriptableObject {
			public string id;
			public bool read = false;
			public string category;
			public string parentInfoCategory;
			public string description;
		}
	}
}