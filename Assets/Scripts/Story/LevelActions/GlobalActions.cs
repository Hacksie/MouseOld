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
                case "OverloadEntry":
                    Debug.Log("GlobalActions: invoke OverloadEntry");
                    CoreGame.Instance.State.currentLevel.timer.Start();
                    CoreGame.Instance.State.currentLight = GlobalLightTypes.Warn;
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Manager Lyon"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Cari"));
                    //Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                    return true;
                case "BatteryFill":
                    Debug.Log("GlobalActions: invoke BatteryFill");
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
                    CoreGame.Instance.State.currentLevel.timer.Start();
                    CoreGame.Instance.State.currentLight = GlobalLightTypes.Default;
                    CoreGame.Instance.State.currentLevel.completed = true;
                    return true;
                case "LevelExit":
                    Debug.Log("GlobalActions: invoke LevelExit");
                    if(CoreGame.Instance.State.currentLevel.completed)
                    {
                        CoreGame.Instance.State.currentLevel.timer.Stop();
                        Debug.Log("Level Over");
                        
                        //Show end screen!
                    }
                    return true;
            }
            return false;
        }
    }
}
