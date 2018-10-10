using UnityEngine;

namespace HackedDesign {

	namespace Dialogue {
		[CreateAssetMenu (fileName = "Dialogue", menuName = "Mouse/Dialogue/Dialogue")]
		public class Dialogue : ScriptableObject {
			public Character.Character speaker;
			//public string speaker;
			[TextArea]
			public string text;
			public string button1text;
			public string button2text;
			public string button3text;
			public string button4text;

			//public Sprite button;
			public DialogueAction dialogueAction1;
			public DialogueAction dialogueAction2;
			public DialogueAction dialogueAction3;
			public DialogueAction dialogueAction4;
		}
	}

}
