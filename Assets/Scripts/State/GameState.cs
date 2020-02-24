using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.State
{
    [System.Serializable]
    public class GameState
    {
        public int gameVersion = 0; 
        public int gameSlot = 0;

        [Header("Game State")]
        public GameStateEnum state = GameStateEnum.GAMEOVER;

        [Header("Player State")]
        public State.PlayerState player = null;

        [Header("Story State")]
		public StoryState story = new StoryState();

        [Header("Level State")]
        public Level.Level currentLevel = null;

        public bool isRandom = false;

        public List<Story.Task> taskList = new List<Story.Task>();

        public Story.Task selectedTask = null;

        public GameObject alertTrap = null; // move this to state

        public GlobalLightTypes currentLight;
        
        public List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger>();
        //public List<Entities.BaseEntity> npcList = new List<Entities.BaseEntity>();
        public List<Entities.Enemy> enemyList = new List<Entities.Enemy>();
        public List<Entities.BaseEntity> entityList = new List<Entities.BaseEntity>();
        public List<Triggers.Door> doorList = new List<Triggers.Door>();

        
    }

	public class StoryState {
        public int act = 0;
		public bool prelude_cat_talk = false;
		public bool prelude_laptop = false;
	}

    public enum GameStateEnum
    {
        MAINMENU,
        TITLECARD,
        CUTSCENE,
        PLAYING,
        LOADING,
        NARRATION,
        DIALOGUE,
        WORLDMAP,
        STARTMENU,
        SELECTMENU,
        GAMEOVER,
        CAPTURED,
        MISSIONCOMPLETE
    }

    public enum PlayingStateEnum
    {
        DEFAULT,
        HACKING,
    }
}