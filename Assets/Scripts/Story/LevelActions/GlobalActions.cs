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
                case "HackEntry":
                    Logger.Log("GlobalActions", "GlobalActions: invoke HackEntry");
                    // FIXME: Check if any other condition exists first!
                    ActionManager.instance.AddActionMessage("entry hacked");
                    ActionManager.instance.AddActionMessage("security systems activated!");
                    ActionManager.instance.AddActionMessage($"{CoreGame.Instance.state.player.baselevelTimer.ToString()} second timer initiated!");
                    CoreGame.Instance.state.currentLevel.startTime = Time.time;
                    CoreGame.Instance.state.currentLevel.timer.Start(CoreGame.Instance.state.player.baselevelTimer);
                    CoreGame.Instance.state.currentLight = State.GlobalLightTypes.Warn;
                    return true;
                case "BatteryFill":
                    Logger.Log("GlobalActions", "GlobalActions: invoke BatteryFill");
                    ActionManager.instance.AddActionMessage("battery filled");
                    CoreGame.Instance.state.player.battery = CoreGame.Instance.state.player.maxBattery;
                    return true;
                case "TimerStart":
                    Logger.Log("GlobalActions", "GlobalActions: invoke TimerStart");
                    return true;
                case "TimerAlert":
                    Logger.Log("GlobalActions", "invoke TimerAlert");
                    return true;
                case "TimerExpired":
                    Logger.Log("GlobalActions", "invoke TimerEnd");
                    CoreGame.Instance.state.currentLight = State.GlobalLightTypes.Alert;
                    return true;
                case "EndComputer":
                    Logger.Log("GlobalActions", "invoke EndComputer");
                    ActionManager.instance.AddActionMessage("alert shutdown");
                    CoreGame.Instance.state.currentLevel.timer.Start(CoreGame.Instance.state.player.baselevelTimer);
                    CoreGame.Instance.state.currentLight = State.GlobalLightTypes.Default;
                    CoreGame.Instance.state.currentLevel.completed = true;
                    return true;
                case "Captured":
                    return true;
                case "LevelExit":
                    Logger.Log("GlobalActions", "invoke LevelExit");
                    if(CoreGame.Instance.state.currentLevel.completed)
                    {
                        if (CoreGame.Instance.state.currentLevel.template.hostile)
                        {
                            ActionManager.instance.AddActionMessage("mission completed");
                            CoreGame.Instance.state.currentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            CoreGame.Instance.SetMissionComplete();
                        }
                        else
                        {
                            ActionManager.instance.AddActionMessage("level completed");
                            CoreGame.Instance.state.currentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            CoreGame.Instance.SetLevelComplete();
                        }
                    }
                    else
                    {
                        CoreGame.Instance.denied.Play();
                    }
                    return true;
            }
            return false;
        }
    }
}
