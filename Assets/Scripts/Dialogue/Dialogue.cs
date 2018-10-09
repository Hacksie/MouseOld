using UnityEngine;

namespace HackedDesign {

	namespace Dialogue {
		[CreateAssetMenu (fileName = "Dialogue", menuName = "Mouse/Dialogue/Dialogue")]
		public class Dialogue : ScriptableObject {
			public Character.Character speaker;
			//public string speaker;
			[TextArea]
			public string text;
			public Sprite button;
			public DialogueAction dialogueAction;
		}
	}

}
