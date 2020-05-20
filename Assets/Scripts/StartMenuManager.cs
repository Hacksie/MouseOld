using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class StartMenuManager : MonoBehaviour {

		public void ResumeEvent () {
			GameManager.Instance.SetPlaying ();
		}

		public void ResetEvent () {
			
		}

		public void QuitEvent () {
			//FIXME: Ask for save?
			GameManager.Instance.EndGame();
			//SceneManager.LoadScene ("MainMenu");
		}

	}
}