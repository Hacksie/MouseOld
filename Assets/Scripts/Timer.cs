using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HackedDesign {
	[CreateAssetMenu (fileName = "Timer", menuName = "Mouse/Gameplay/Timer")]
	public class Timer : ScriptableObject {

		
		public float maxTime = 5 * 60;
		public float warningTime = 30;
		public float alertTime = 10;
		public float startTime;
		public bool running;

		public Color color;
		public Color warningColor;
		public Color alertColor;

		public void UpdateTimer()
		{

		}

		public void Start()
		{
			startTime = Time.time;
			running = true;
			//public float startTime  = Time.time;
			//public float currentTime  = Time.time;			
		}

		// public void Start()
		// {
		// 	public float startTime  = Time.time;
		// 	public float currentTime  = Time.time;
		// }

		// //FIXME: Subscribe an watcher to look for when it hits 0
		// public void Reset()
		// {
		// 	currentTime = maxTime;
		// }

	}
}