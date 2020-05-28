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
            ActionManager.Instance.CurrentScene = new PreludeBarScene("Aisana Bar", length, height, width, difficulty, enemies, traps);
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
                    InfoRepository.Instance.AddToKnownEntities("AisanaContractTower2");
                    InfoRepository.Instance.AddToKnownEntities("AisanaContractTower1");
                    //InfoRepository.Instance.AddToKnownEntities("SaikaCorpHQ");
                    InfoRepository.Instance.AddToKnownEntities("OliviasApartment");
                    InfoRepository.Instance.AddToKnownEntities("AisanaContractBar");
                    ActionManager.Instance.AddActionMessage("Task added to current tasks - Bootstrap");
                    TaskRepository.Instance.AddTask("bootstrap");
                    TaskRepository.Instance.SelectCurrentTask("bootstrap");
                    GameManager.Instance.GameState.CurrentLevel.completed = true;

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
                case "Prelude6":
                    Debug.Log("PreludeActions: invoke Prelude6");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude6");
                    return true;
                case "Prelude7":
                    Debug.Log("PreludeActions: invoke Prelude7");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude7");
                    return true;
                case "Prelude8":
                    Debug.Log("PreludeActions: invoke Prelude8");
                    Dialogue.NarrationManager.Instance.ShowNarration("Prelude8");
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
                    GameManager.Instance.GameState.Story.prelude_cat_talk = true;
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat1");
                    return true;
                case "PreludeCat2":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat2");
                    return true;
                case "PreludeCat3":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat3");
                    return true;
                case "PreludeCat4":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat4");
                    return true;
                case "PreludeCat5":
                    Dialogue.NarrationManager.Instance.ShowNarration("PreludeCat5");
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

        public void PreludeCat3()
        {

        }
        public void PreludeCat4()
        {

        }


        public void PreludeLaptop()
        {
            Debug.Log("PreludeActions: prelude laptop");
            GameManager.Instance.GameState.Story.prelude_laptop = true;
            SelectMenuManager.instance.MenuState = SelectMenuSubState.Tasks;
            GameManager.Instance.SetSelectMenu();
        }


        public void PreludeExit()
        {
            if (GameManager.Instance.GameState.CurrentLevel.completed)
            {
                GameManager.Instance.SetWorldMap();
                //CoreGame.Instance.LoadNewLevel("Bootstrap");
            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }
            /*
            if (CoreGame.Instance.state.taskList.Exists(t => t.title == "Milk Run"))
            {
                Debug.Log("PreludeActions: can exit");
                

            }
            else
            {
                Debug.Log("PreludeActions: can't exit, haven't received mission");
            }*/
        }
    }
}
