using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class BootstrapActions : ILevelActions
    {
        public bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "Bootstrap":
                    Logger.Log("BootstrapActions", "Invoke Bootstrap");
                    return true;
                case "Bootstrap1":
                    Logger.Log("BootstrapActions", "Invoke Bootstrap1");
                    InfoManager.instance.AddToKnownEntities("Mouse");
                    InfoManager.instance.AddToKnownEntities("Arisana");
                    InfoManager.instance.AddToKnownEntities("ManagerLyon");
                    InfoManager.instance.AddToKnownEntities("Cat");
                    InfoManager.instance.AddToKnownEntities("Saika");
                    Dialogue.NarrationManager.instance.ShowNarration("Bootstrap1");
                    return true;
            }
            return false;
        }
    }
}
