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

            //HideAll();
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            switch (selectMenuManager.MenuState)
            {
                case SelectMenuSubState.Info:
                    infoPanel.Repaint();
                    break;
                case SelectMenuSubState.Tasks:
                    taskPanel.Repaint();
                    break;
                case SelectMenuSubState.Stash:
                    stashPanel.Repaint();
                    break;
                case SelectMenuSubState.Psych:
                    psychPanel.Repaint();
                    break;
            }
        }

        public override void Show()
        {
            switch (selectMenuManager.MenuState)
            {
                case SelectMenuSubState.Info:
                    infoPanel.Show();
                    break;
                case SelectMenuSubState.Tasks:
                    taskPanel.Show();
                    break;
                case SelectMenuSubState.Stash:
                    stashPanel.Show();
                    break;
                case SelectMenuSubState.Psych:
                    psychPanel.Show();
                    break;
            }
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }

        private void HideAll()
        {
            infoPanel.Hide();
            taskPanel.Hide();
            stashPanel.Hide();
            psychPanel.Hide();
        }

        public void ResumeClickEvent()
        {
            HideAll();
            GameManager.Instance.SetPlaying();
        }

        public void InfoClickEvent()
        {
            Debug.Log("Select Menu Info Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Info;
            HideAll();
            infoPanel.Show();
            infoPanel.Repaint();
        }

        public void TaskClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Tasks;
            HideAll();
            taskPanel.Show();
            taskPanel.Repaint();
        }

        public void StashClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Stash;
            HideAll();
            stashPanel.Show();
            stashPanel.Repaint();
        }

        public void PsychClickEvent()
        {
            Debug.Log("Select Menu Task Clicked");
            selectMenuManager.MenuState = SelectMenuSubState.Psych;
            HideAll();
            psychPanel.Show();
            psychPanel.Repaint();
        }
    }
}