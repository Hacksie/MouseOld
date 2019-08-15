using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class StartMenuManager : MonoBehaviour {

		public void ResumeEvent () {
			CoreGame.Instance.SetPlaying ();
		}

		public void QuitEvent () {
			//FIXME: Ask for save?
			CoreGame.Instance.EndGame();
			//SceneManager.LoadScene ("MainMenu");
		}

	}
}