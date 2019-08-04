
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

	public class State {
		public int gameSlot;
		public GameState state;
		public Level.Level level;
		public List<Story.Task> taskList = new List<Story.Task> ();

		public Story.Task selectedTask;

	}

	public enum GameState {
		MAINMENU,
		CUTSCENE,
		PLAYING,
		LOADING,
		NARRATION,
		DIALOGUE,
		WORLDMAP,
		STARTMENU,
		SELECTMENU,
		GAMEOVER

	}
}