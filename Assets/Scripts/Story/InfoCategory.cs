using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[System.Serializable]
		public class InfoCategory {
			public string id;
			public string text;
			public bool available = true;
			public string lookupList;
		}
	}
}