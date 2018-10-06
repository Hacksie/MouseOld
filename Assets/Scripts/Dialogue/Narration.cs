using UnityEngine;

namespace HackedDesign {

	namespace Dialogue {
		[CreateAssetMenu (fileName = "Narration", menuName = "Mouse/Dialogue/Narration")]
		public class Narration : ScriptableObject {
			//public Character.Character speaker;
			[TextArea]
			public string text;
			public Sprite button;
			//public DialogueAction dialogueAction;
		}
	}

}
