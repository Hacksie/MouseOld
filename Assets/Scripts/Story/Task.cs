using System.Collections.Generic;

namespace HackedDesign {
	namespace Story {
		[System.Serializable]
		public class Task {
			public string id;
			public string title;
			
			public string description;
			

			public string giver;
			public string reward; 
			public string rewardType;

			public bool started;
			public bool completed;

			public List<TaskObjective> objectives;

			
		}
	}
}