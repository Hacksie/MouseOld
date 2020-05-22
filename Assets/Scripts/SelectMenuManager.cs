using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackedDesign {
	public class SelectMenuManager : MonoBehaviour {

		public static SelectMenuManager instance;

		void Start()
		{
			instance = this;
		}

		public SelectMenuSubState MenuState { get; set; } = SelectMenuSubState.Info;


	}
}