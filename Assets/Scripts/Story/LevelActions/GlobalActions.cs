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
                    Debug.Log("GlobalActions: invoke HackEntry");
                    // FIXME: Check if any other condition exists first!
                    ActionManager.instance.AddActionMessage("Entry hacked");
                    ActionManager.instance.AddActionMessage("Security systems activated!");
                    ActionManager.instance.AddActionMessage($"{CoreGame.Instance.state.player.baselevelTimer.ToString()} second timer initiated!");
                    CoreGame.Instance.state.currentLevel.startTime = Time.time;
                    CoreGame.Instance.state.currentLevel.timer.Start(CoreGame.Instance.state.player.baselevelTimer);
                    CoreGame.Instance.state.currentLight = State.GlobalLightTypes.Warn;
                    return true;
                case "BatteryFill":
                    Debug.Log("GlobalActions: invoke BatteryFill");
                    ActionManager.instance.AddActionMessage("Battery filled");
                    CoreGame.Instance.state.player.battery = CoreGame.Instance.state.player.maxBattery;
                    return true;
                case "TimerStart":
                    Debug.Log("GlobalActions: invoke TimerStart");
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
                    ActionManager.instance.AddActionMessage("Alert shutdown");
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
                            ActionManager.instance.AddActionMessage("Mission completed");
                            CoreGame.Instance.state.currentLevel.timer.Stop();
                            Logger.Log("GlobalActions", "Level Over");
                            CoreGame.Instance.SetMissionComplete();
                        }
                        else
                        {
                            ActionManager.instance.AddActionMessage("Level completed");
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
