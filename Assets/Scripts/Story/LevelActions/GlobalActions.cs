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
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Arisana"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Manager Lyon"));
                    //InfoManager.instance.AddToKnownEntities(InfoManager.instance.entities.Find(e => e.name == "Cari"));
                    //Dialogue.NarrationManager.instance.ShowNarration("Prelude1");
                    return true;
            }
            return false;
        }
    }
}
