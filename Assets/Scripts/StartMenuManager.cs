using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class StartMenuManager : MonoBehaviour {

		public void ResumeEvent () {
			CoreGame.instance.SetPlaying ();
		}

		public void QuitEvent () {
			//FIXME: Ask for save?
			CoreGame.instance.EndGame();
			//SceneManager.LoadScene ("MainMenu");
		}

	}
}