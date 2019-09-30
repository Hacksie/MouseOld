using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "Task", menuName = "Mouse/Story/Task")]
		public class Task : ScriptableObject {
			public string title;
			[TextArea]
			public string description;

			public Character manager;

			public bool started;
			public bool completed;

			public List<TaskObjective> objectives;
			public List<Story.Character> keyContacts;
		}
	}
}