using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign {

	namespace Dialogue {

		[System.Serializable]
		public class Narration {
			public string id;
			public string speaker;
			[TextArea]
			public string text;
			public Sprite button;
			public string action;
		}
	}

}
