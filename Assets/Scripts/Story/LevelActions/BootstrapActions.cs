using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class BootstrapActions : ILevelActions
    {
        private Dialogue.NarrationManager narrationManager;

        public BootstrapActions(Dialogue.NarrationManager narrationManager)
        {
            this.narrationManager = narrationManager;
        }

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
                    this.narrationManager.ShowNarration("Bootstrap1");
                    return true;
            }
            return false;
        }
    }
}
