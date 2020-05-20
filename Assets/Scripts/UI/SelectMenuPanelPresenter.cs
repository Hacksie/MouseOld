using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HackedDesign.Story;

namespace HackedDesign.UI
{
    public class SelectMenuPanelPresenter : AbstractPresenter
    {

        SelectMenuManager selectMenuManager;

        private InfoPanelPresenter infoPanel = null;
        private TaskPanelPresenter taskPanel = null;
        private StashPanelPresenter stashPanel = null;
        private PsychPanelPresenter psychPanel = null;

        [SerializeField] private Button InfoButton = null;
        [SerializeField] private Button TasksButton = null;
        [SerializeField] private Button StashButton = null;
        [SerializeField] private Button PsychButton = null;
        [SerializeField] private Button MapButton = null;


        public void Initialize(SelectMenuManager selectMenuManager, InfoPanelPresenter infoPanel, TaskPanelPresenter taskPanel, StashPanelPresenter stashPanel, PsychPanelPresenter psychPanel)
        {
            this.selectMenuManager = selectMenuManager;
            this.infoPanel = infoPanel;
            this.taskPanel = taskPanel;
            this.stashPanel = stashPanel;
            this.psychPanel = psychPanel;
        }

        public override void Repaint()
        {
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.SelectMenu)
            {
                Show();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                switch (selectMenuManager.MenuState)
                {
                    case SelectMenuState.Info:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(InfoButton.gameObject);
                        break;
                    case SelectMenuState.Tasks:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(TasksButton.gameObject);
                        break;
                    case SelectMenuState.Stash:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(StashButton.gameObject);
                        break;
                    case SelectMenuState.Psych:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PsychButton.gameObject);
                        break;
                    case SelectMenuState.Map:
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(MapButton.gameObject);
                        break;
                }

            }
            else
            {
                Hide();
            }

            infoPanel.Repaint();
            taskPanel.Repaint();
            stashPanel.Repaint();
            psychPanel.Repaint();
        }

        private void Show(bool flag)
        {
            gameObject.SetActive(flag);
              
        }

        public void ResumeClickEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void InfoClickEvent()
        {
            Debug.Log("Select Menu Info Clicked");
            selectMenuManager.MenuState = SelectMenuState.Info;
            Show(true);
        }

        public void TaskClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuState.Tasks;
            Show(true);
        }

        public void StashClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuState.Stash;
            Show(true);
        }

        public void PsychClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuState.Psych;
            Show(true);
        }

        public void MapClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuState.Map;
            Show(true);
        }


    }
}