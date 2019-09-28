using UnityEngine;

namespace HackedDesign {

	namespace Dialogue {
		[CreateAssetMenu (fileName = "Dialogue", menuName = "Mouse/Dialogue/Dialogue")]
		public class Dialogue : ScriptableObject {
			public Story.Character speaker;
			//public string speaker;
			[TextArea]
			public string text;
			public string button1text;
			public string button2text;
			public string button3text;
			public string button4text;

			//public Sprite button;
			public string dialogueAction1;
			public string dialogueAction2;
			public string dialogueAction3;
			public string dialogueAction4;
		}
	}

}
