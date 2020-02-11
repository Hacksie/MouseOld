using UnityEngine;
using HackedDesign.Entity;

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
                    ActionManager.instance.AddActionMessage("60 second timer initiated!");
                    CoreGame.Instance.State.currentLevel.startTime = Time.time;
                    CoreGame.Instance.State.currentLevel.timer.Start();
                    CoreGame.Instance.State.currentLight = GlobalLightTypes.Warn;
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Manager Lyon"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Cari"));
                    //Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                    return true;
                case "BatteryFill":
                    Debug.Log("GlobalActions: invoke BatteryFill");
                    ActionManager.instance.AddActionMessage("Battery filled");
                    CoreGame.Instance.State.player.battery = CoreGame.Instance.State.player.maxBattery;
                    return true;
                case "TimerStart":
                    Debug.Log("GlobalActions: invoke TimerStart");
                    return true;
                case "TimerAlert":
                    Debug.Log("GlobalActions: invoke TimerAlert");
                    return true;
                case "TimerExpired":
                    Debug.Log("GlobalActions: invoke TimerEnd");
                    CoreGame.Instance.State.currentLight = GlobalLightTypes.Alert;
                    return true;
                case "EndComputer":
                    Debug.Log("GlobalActions: invoke EndComputer");
                    ActionManager.instance.AddActionMessage("Alert shutdown");
                    CoreGame.Instance.State.currentLevel.timer.Start();
                    CoreGame.Instance.State.currentLight = GlobalLightTypes.Default;
                    CoreGame.Instance.State.currentLevel.completed = true;
                    return true;
                case "Captured":
                    return true;
                case "LevelExit":
                    Debug.Log("GlobalActions: invoke LevelExit");
                    if(CoreGame.Instance.State.currentLevel.completed)
                    {
                        ActionManager.instance.AddActionMessage("Mission completed");
                        CoreGame.Instance.State.currentLevel.timer.Stop();
                        Debug.Log("Level Over");
                        CoreGame.Instance.SetMissionComplete();
                        
                        //Show end screen!
                    }
                    return true;
            }
            return false;
        }
    }
}
