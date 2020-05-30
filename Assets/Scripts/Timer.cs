using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HackedDesign {
	//[CreateAssetMenu (fileName = "Timer", menuName = "Mouse/Gameplay/Timer")]
	[System.Serializable]
	public class Timer {
		//public float maxTime = 5 * 60;
		public float maxTime = 60;
		public float warningTime = 20;
		public float alertTime = 10;
		public float startTime;
		public bool running;
		public bool warning=false;
		public bool end=false;

		public void Start(int timeout)
		{
			startTime = Time.time;
			running = true;
			warning = end = false;
			this.maxTime = timeout;
			Story.SceneManager.Instance.Invoke("TimerStart");

		}

		public void Stop()
		{
			running = false;
		}

		public void Update()
		{
			if(!running)
			{
				return;
			}
			if(running && !end && Time.time - startTime >= maxTime)
			{
				end = true;
				running = false;
				Story.SceneManager.Instance.Invoke("TimerExpired");
			}			

			if(running && !warning && Time.time - startTime >= (maxTime-warningTime))
			{
				warning = true;
				Story.SceneManager.Instance.Invoke("TimerAlert");
			}


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