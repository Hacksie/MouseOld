using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public abstract class GlobalScene : IScene
    {
        protected string templateName;
        protected int length;
        protected int height;
        protected int width;
        protected int difficulty;
        protected int enemies;
        protected int traps;

        protected GlobalScene(string templateName, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            this.templateName = templateName;
            this.length = length;
            this.height = height;
            this.width = width;
            this.difficulty = difficulty;
            this.enemies = enemies;
            this.traps = traps;
        }

        protected void LoadLevel()
        {
            GameManager.Instance.SetLoading();
            var levelTemplate = SceneManager.Instance.GetLevelGenTemplate(this.templateName);
            GameManager.Instance.Data.CurrentLevel = Level.LevelGenerator.Generate(levelTemplate, this.length, this.height, this.width, this.difficulty, this.enemies, this.traps);
        }

        public virtual bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "TriggerEntry":
                    Logger.Log("GlobalActions", "GlobalActions: invoke TriggerEntry");
                    // FIXME: Check if any other condition exists first!
                    if (!GameManager.Instance.Data.CurrentLevel.entryTriggered)
                    {
                        GameManager.Instance.Data.CurrentLevel.entryTriggered = true;

                        SceneManager.Instance.AddActionMessage("Entry triggered");
                        var timer = GameManager.Instance.Data.CurrentLevel.template.levelLength * 10;

                        SceneManager.Instance.AddActionMessage($"{timer} seconds until security triggers!");
                        GameManager.Instance.Data.CurrentLevel.startTime = Time.time;
                        GameManager.Instance.Data.CurrentLevel.timer.Start(timer);
                    }
                    //CoreGame.Instance.state.currentLight = GlobalLightTypes.Warn;
                    return true;
                case "BatteryFill":
                    Logger.Log("GlobalActions", "GlobalActions: invoke BatteryFill");
                    SceneManager.Instance.AddActionMessage("battery filled");
                    GameManager.Instance.Data.Player.battery = GameManager.Instance.Data.Player.maxBattery;
                    return true;
                case "TimerStart":
                    Logger.Log("GlobalActions", "GlobalActions: invoke TimerStart");
                    return true;
                case "TimerAlert":
                    Logger.Log("GlobalActions", "invoke TimerAlert");
                    return true;
                case "TimerExpired":
                    Logger.Log("GlobalActions", "invoke TimerEnd");
                    GameManager.Instance.Data.currentLight = GlobalLightTypes.Alert;
                    return true;
                case "EndComputer":
                    Logger.Log("GlobalActions", "invoke EndComputer");
                    SceneManager.Instance.AddActionMessage("alert shutdown");
                    GameManager.Instance.Data.CurrentLevel.timer.Start(GameManager.Instance.Data.Player.baselevelTimer);
                    GameManager.Instance.Data.currentLight = GlobalLightTypes.Default;
                    GameManager.Instance.Data.CurrentLevel.completed = true;
                    return true;
                case "Captured":
                    return true;
                case "LevelExit":
                    Logger.Log("GlobalActions", "invoke LevelExit");
                    if (Complete())
                    {
                        if (GameManager.Instance.Data.CurrentLevel.template.hostile)
                        {
                            SceneManager.Instance.AddActionMessage("mission completed");
                            GameManager.Instance.Data.CurrentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            GameManager.Instance.SetMissionComplete();
                        }
                        else
                        {
                            SceneManager.Instance.AddActionMessage("level completed");
                            GameManager.Instance.Data.CurrentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            GameManager.Instance.SetLevelComplete();
                        }
                    }
                    else
                    {
                        Logger.Log("GlobalScene", "Cannot exit, level incomplete");
                    }
                    return true;
                case "Act1":
                    SceneManager.Instance.Invoke("Prelude1");
                    return true;
                case "Act2":
                    return true;
                case "Act3":
                    return true;
                case "Act4":
                    return true;
                case "Act5":
                    return true;
            }
            return false;
        }

        public abstract void Begin();
        public abstract void Next();
        public abstract bool Complete();

    }
}
