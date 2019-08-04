using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign {

	namespace Dialogue {
		[CreateAssetMenu (fileName = "Narration", menuName = "Mouse/Dialogue/Narration")]
		public class Narration : ScriptableObject {
			public Story.Character speaker;
			[TextArea]
			public string text;
			public Sprite button;
			public string narrationAction;
		}
	}

}
