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

		[Header("Game State")]
		public Entity.BaseTrap alertTrap; // move this to state		

		public List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger> ();
		public List<Entity.BaseEntity> entityList = new List<Entity.BaseEntity> ();
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