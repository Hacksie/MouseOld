﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{

    public class State
    {
        public int gameSlot;

        [Header("Game State")]
        public GameState state;

        [Header("Player State")]
        public Character.PlayerState player;

        [Header("Story State")]
		public StoryState story = new StoryState();

        [Header("Level State")]
        public Level.Level level;

        public List<Story.Task> taskList = new List<Story.Task>();

        public Story.Task selectedTask;


        public GameObject alertTrap; // move this to state		

        public List<Triggers.ITrigger> triggerList = new List<Triggers.ITrigger>();
        public List<Entity.BaseEntity> entityList = new List<Entity.BaseEntity>();
    }

	public class StoryState {
		public bool prelude_kari_talk = false;
		public bool prelude_laptop = false;
	}

    public enum GameState
    {
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