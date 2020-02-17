using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[System.Serializable]
		public class InfoEntity {
			public string id;
			public string name;
			public bool read = false;
			public string parentInfoCategory;
			public string description;
		}
	}
}