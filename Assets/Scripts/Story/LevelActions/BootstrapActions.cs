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
                    InfoRepository.Instance.AddToKnownEntities("Mouse");
                    InfoRepository.Instance.AddToKnownEntities("Arisana");
                    InfoRepository.Instance.AddToKnownEntities("ManagerLyon");
                    InfoRepository.Instance.AddToKnownEntities("Cat");
                    InfoRepository.Instance.AddToKnownEntities("Saika");
                    Dialogue.NarrationManager.instance.ShowNarration("Bootstrap1");
                    return true;
            }
            return false;
        }
    }
}
