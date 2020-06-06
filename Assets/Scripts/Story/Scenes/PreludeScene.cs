using UnityEngine;
using HackedDesign.Entities;

namespace HackedDesign.Story
{
    public class PreludeScene : GlobalScene
    {
        public PreludeScene(string templateName, int length, int height, int width, int difficulty, int enemies, int traps): base(templateName, length, height, width, difficulty, enemies, traps)
        {
            LoadLevel();
            GameManager.Instance.SceneInitialize();
            //GameManager.Instance.SetTitlecard(); //FIXME: Make this async / loaderbar
        }

        public override void Begin()
        {
            Invoke("Prelude1");
        }

        public override void Next()
        {
            SceneManager.Instance.CurrentScene = new PreludeBarScene("Aisana Bar", length, height, width, difficulty, enemies, traps);
        }

        public override bool Invoke(string actionName)
        {
            switch (actionName)
            {
                case "Bootstrap1":
                case "Prelude1":
                    Debug.Log("PreludeActions: invoke Prelude1");
                    InfoRepository.Instance.AddToKnownEntities("Mouse");
                    InfoRepository.Instance.AddToKnownEntities("Aisana");
                    InfoRepository.Instance.AddToKnownEntities("ManagerLyon");
                    InfoRepository.Instance.AddToKnownEntities("Cat");
                    InfoRepository.Instance.AddToKnownEntities("Saika");
                    SceneManager.Instance.AddToKnownLocations("AisanaContractorTower2");
                    SceneManager.Instance.AddToKnownLocations("AisanaContractorTower1");
                    //InfoRepository.Instance.AddToKnownEntities("AisanaContractorTower2");
                    //InfoRepository.Instance.AddToKnownEntities("AisanaContractorTower1");
                    //InfoRepository.Instance.AddToKnownEntities("SaikaCorpHQ");
                    //InfoRepository.Instance.AddToKnownEntities("OliviasApartment");
                    //InfoRepository.Instance.AddToKnownEntities("AisanaContractBar");
                    SceneManager.Instance.AddActionMessage("Task added to current tasks - Bootstrap");
                    TaskRepository.Instance.AddTask("bootstrap");
                    TaskRepository.Instance.SelectCurrentTask("bootstrap");
                    GameManager.Instance.Data.CurrentLevel.completed = true;

                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude1");
                    return true;
                case "Prelude2":
                    Debug.Log("PreludeActions: invoke Prelude2");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude2");
                    return true;
                case "Prelude3":
                    Debug.Log("PreludeActions: invoke Prelude3");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude3");
                    return true;
                case "Prelude4":
                    Debug.Log("PreludeActions: invoke Prelude4");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude4");
                    return true;
                case "Prelude5":
                    Debug.Log("PreludeActions: invoke Prelude5");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude5");
                    return true;
                case "PreludeLaptop":
                    PreludeLaptop();
                    return true;
                case "PreludeGun":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeGun");
                    return true;
                case "PreludeClothes":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeClothes");
                    return true;
                case "PreludeCat":
                case "PreludeCat1":
                    Debug.Log("PreludeActions: prelude Cat");
                    GameManager.Instance.Data.Story.storyEvents.Add("PreludeCat");
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat1");
                    return true;
                case "PreludeFridge":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeFridge");
                    return true;
                case "PreludeExit":
                    PreludeExit();
                    return true;
                case "PreludeBarKat":
                    Debug.Log("Snowowl");
                    return true;
            }

            return base.Invoke(actionName);
        }

        private void PreludeLaptop()
        {
            Debug.Log("PreludeActions: prelude laptop");
            GameManager.Instance.Data.Story.storyEvents.Add("PreludeLaptop");
            GameManager.Instance.SetSelectMenu(SelectMenuSubState.Tasks);
        }


        private void PreludeExit()
        {
            if (Complete())
            {
                GameManager.Instance.SetWorldMap(); 
            }
            else
            {
                
                Logger.Log("PreludeScene", "PreludeActions: can't exit, haven't received mission");
            }
        }

        public override bool Complete()
        {
            return GameManager.Instance.Data.CurrentLevel.completed && TaskRepository.Instance.HasTask("bootstrap") && GameManager.Instance.Data.Story.storyEvents.Contains("PreludeLaptop");
        }
    }
}
