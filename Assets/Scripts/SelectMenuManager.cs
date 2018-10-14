using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class SelectMenuManager : MonoBehaviour {

		public void ResumeEvent () {
			CoreGame.instance.SetResume ();
		}
	}
}