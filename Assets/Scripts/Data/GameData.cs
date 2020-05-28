using System.Collections.Generic;
using HackedDesign.Story;
using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save Properties")]
        [SerializeField] private int gameVersion = 0;
        [SerializeField] private int gameSlot = 0;

        [Header("Game State")]
        [SerializeField] private PlayerData playerState = null;
        [SerializeField] private StoryState story = new StoryState();
        [SerializeField] private Level.Level currentLevel = null;
        [SerializeField] private bool isRandom = false;

        public bool IsRandom { get { return isRandom; } private set { isRandom = value; } }

        private Dictionary<string, Story.Task> taskList = new Dictionary<string, Story.Task>();

        public Story.Task selectedTask = null;

        public GameObject alertTrap = null;

        public GlobalLightTypes currentLight;

        public List<BaseTrigger> triggerList = new List<BaseTrigger>();
        
        public List<IEntity> entityList = new List<IEntity>();
        public List<Door> doorList = new List<Door>();

        public int GameVersion { get { return gameVersion; } set { gameVersion = value; } }
        public int GameSlot { get { return gameSlot; } set { gameSlot = value; } }
        public PlayerData Player { get { return playerState; } set { playerState = value; } }
        public StoryState Story { get { return story; } set { story = value; } }
        public Level.Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }

        public string currentLocation;
        public string currentFloor;

        public Dictionary<string, Task> TaskList { get => taskList; private set => taskList = value; }

        public GameData() : this(false)
        {
        }

        public GameData(bool isRandom)
        {
            this.IsRandom = isRandom;
            this.Player = new PlayerData();
        }
    }

    public class StoryState
    {
        public int act = 0;
        public bool prelude_cat_talk = false;
        public bool prelude_laptop = false;
    }

    public enum SelectMenuSubState
    {
        Info,
        Tasks,
        Stash,
        Psych,
        Map
    }
}