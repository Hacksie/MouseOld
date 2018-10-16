using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MainMenu : MonoBehaviour {

		public void ContinueEvent () {
			Debug.Log ("Continue Event");
		}

		public void NewGameEvent () {
			Debug.Log ("New Game Event");
			StartCoroutine (LoadNewGameScenes ());
		}

		public void OptionsEvent () {
			Debug.Log ("Options Event");
		}

		public void CreditsEvent () {
			Debug.Log ("Credits Event");
		}

		public void QuitEvent () {
			Debug.Log ("Quit Event");
			Application.Quit ();
		}

		IEnumerator LoadNewGameScenes () {
			Debug.Log ("Loading new game scenes");

			AsyncOperation asyncLoadBaseScene = SceneManager.LoadSceneAsync ("Core", LoadSceneMode.Additive);
			asyncLoadBaseScene.allowSceneActivation = false;

			yield return null;

			//Wait until we are done loading the scene
			while (asyncLoadBaseScene.progress < 0.9f) {
				Debug.Log ("Loading scene #:" + "Core" + " [][] Progress: " + asyncLoadBaseScene.progress);
				yield return null;
			}

			Debug.Log ("Core ready");

			AsyncOperation asyncLoadRubyScene = SceneManager.LoadSceneAsync ("IntroRoom", LoadSceneMode.Additive);
			asyncLoadRubyScene.allowSceneActivation = false;

			yield return null;

			//Wait until we are done loading the scene
			while (asyncLoadRubyScene.progress < 0.9f) {
				Debug.Log ("Loading scene #:" + "IntroRoom" + " [][] Progress: " + asyncLoadRubyScene.progress);
				yield return null;
			}

			Debug.Log ("IntroRoom ready");

			asyncLoadBaseScene.allowSceneActivation = true;
			asyncLoadRubyScene.allowSceneActivation = true;

			while (!asyncLoadBaseScene.isDone || !asyncLoadRubyScene.isDone) {
				Debug.Log ("Activating scenes");
				yield return null;
			}

			SceneManager.SetActiveScene (SceneManager.GetSceneByName ("IntroRoom"));
			
			CoreGame.instance.Initialization ();
			CoreGame.instance.SceneInitialize ();
			
			
			SceneManager.UnloadSceneAsync ("MainMenu");
			
		}
	}
}

