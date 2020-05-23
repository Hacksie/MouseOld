using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class GlobalActions : ILevelActions
    {
        public bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "TriggerEntry":
                    Logger.Log("GlobalActions", "GlobalActions: invoke TriggerEntry");
                    // FIXME: Check if any other condition exists first!
                    if (!GameManager.Instance.GameState.CurrentLevel.entryTriggered)
                    {
                        GameManager.Instance.GameState.CurrentLevel.entryTriggered = true;

                        ActionManager.instance.AddActionMessage("Entry triggered");
                        var timer = GameManager.Instance.GameState.CurrentLevel.template.levelLength * 10;

                        ActionManager.instance.AddActionMessage($"{timer} seconds until security triggers!");
                        GameManager.Instance.GameState.CurrentLevel.startTime = Time.time;
                        GameManager.Instance.GameState.CurrentLevel.timer.Start(timer);
                    }
                    //CoreGame.Instance.state.currentLight = GlobalLightTypes.Warn;
                    return true;
                case "BatteryFill":
                    Logger.Log("GlobalActions", "GlobalActions: invoke BatteryFill");
                    ActionManager.instance.AddActionMessage("battery filled");
                    GameManager.Instance.GameState.Player.battery = GameManager.Instance.GameState.Player.maxBattery;
                    return true;
                case "TimerStart":
                    Logger.Log("GlobalActions", "GlobalActions: invoke TimerStart");
                    return true;
                case "TimerAlert":
                    Logger.Log("GlobalActions", "invoke TimerAlert");
                    return true;
                case "TimerExpired":
                    Logger.Log("GlobalActions", "invoke TimerEnd");
                    GameManager.Instance.GameState.currentLight = GlobalLightTypes.Alert;
                    return true;
                case "EndComputer":
                    Logger.Log("GlobalActions", "invoke EndComputer");
                    ActionManager.instance.AddActionMessage("alert shutdown");
                    GameManager.Instance.GameState.CurrentLevel.timer.Start(GameManager.Instance.GameState.Player.baselevelTimer);
                    GameManager.Instance.GameState.currentLight = GlobalLightTypes.Default;
                    GameManager.Instance.GameState.CurrentLevel.completed = true;
                    return true;
                case "Captured":
                    return true;
                case "LevelExit":
                    Logger.Log("GlobalActions", "invoke LevelExit");
                    if (GameManager.Instance.GameState.CurrentLevel.completed)
                    {
                        if (GameManager.Instance.GameState.CurrentLevel.template.hostile)
                        {
                            ActionManager.instance.AddActionMessage("mission completed");
                            GameManager.Instance.GameState.CurrentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            GameManager.Instance.SetMissionComplete();
                        }
                        else
                        {
                            ActionManager.instance.AddActionMessage("level completed");
                            GameManager.Instance.GameState.CurrentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            GameManager.Instance.SetLevelComplete();
                        }
                    }
                    else
                    {
                        //CoreGame.Instance.denied.Play();
                    }
                    return true;
                case "Act1":
                    ActionManager.instance.Invoke("Prelude1");
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
    }
}
