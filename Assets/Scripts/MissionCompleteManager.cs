using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class MissionCompleteManager : MonoBehaviour {
		public void ResumeEvent () {
            //FIXME: This would return to hub room
			CoreGame.Instance.EndGame ();
		}        
    }
}

