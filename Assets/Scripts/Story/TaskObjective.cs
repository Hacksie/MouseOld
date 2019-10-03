using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Story {
		[CreateAssetMenu (fileName = "Objective", menuName = "Mouse/Story/Objective")]
		public class TaskObjective : ScriptableObject {
			public string objective;
			public bool completed;
			public bool optional;
		}
	}
}