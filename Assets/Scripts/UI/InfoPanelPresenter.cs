using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HackedDesign.Story;

namespace HackedDesign.UI
{
    public class InfoPanelPresenter : AbstractPresenter
    {
        public Transform infoCategoriesParent;
        public Transform infoEntitiesParent;
        public Transform infoDescriptionParent;

        SelectMenuManager selectMenuManager;
        [SerializeField]
        private Text description = null;

        public void Start()
        {
            if (infoCategoriesParent == null)
            {
                Debug.LogError("No infoCategoriesParent set");
            }

            if (infoEntitiesParent == null)
            {
                Debug.LogError("No infoEntitiesParent set");
            }

            if (infoDescriptionParent == null)
            {
                Debug.LogError("No infoDescriptionParent set");
            }
            if (description == null)
            {
                Logger.LogError(name, "No description set");
            }
        }

        public void Initialize(SelectMenuManager selectMenuManager)
        {
            this.selectMenuManager = selectMenuManager;
        }

        public override void Repaint()
        {
            if (GameManager.Instance.GameState.PlayState == PlayStateEnum.SelectMenu && selectMenuManager.MenuState == SelectMenuState.Info)
            {
                Show();
                RepaintCategories();
            }
            else
            {
                Hide();
            }
        }

 
        public void RepaintCategories()
        {
            EventSystem.current.SetSelectedGameObject(null);

            var categories = InfoRepository.Instance.categories.Where(e => e.available).ToList();

            if (string.IsNullOrWhiteSpace(InfoRepository.Instance.SelectedInfoCategory))
            {
                InfoRepository.Instance.SelectedInfoCategory = categories[0].id;
            }

            for (int i = 0; i < infoCategoriesParent.childCount; i++)
            {
                Transform cbt = infoCategoriesParent.GetChild(i);
                Button b = cbt.GetComponent<Button>();
                Text t = cbt.GetComponentInChildren<Text>();

                if (categories.Count() > i)
                {
                    cbt.gameObject.SetActive(true); // Just in case it isn't active

                    cbt.name = categories[i].text;

                    var infoEntityDescriptor = cbt.gameObject.GetComponent<InfoEntityDescriptor>();

                    if (infoEntityDescriptor == null)
                    {
                        infoEntityDescriptor = cbt.gameObject.AddComponent<InfoEntityDescriptor>();
                    }

                    infoEntityDescriptor.id = categories[i].id;
                    t.text = categories[i].text;

                    if (InfoRepository.Instance.SelectedInfoCategory == categories[i].id)
                    {
                        EventSystem.current.SetSelectedGameObject(cbt.gameObject);
                    }
                }
                else
                {
                    cbt.gameObject.SetActive(false);
                }
            }


            RepaintEntities();
        }

        public void SelectCategory()
        {
            var infoEntityDescriptor = EventSystem.current.currentSelectedGameObject.GetComponent<InfoEntityDescriptor>();

            if (infoEntityDescriptor != null)
            {
                InfoRepository.Instance.SelectedInfoCategory = infoEntityDescriptor.id;
                RepaintEntities();
            }
        }

        public void RepaintEntities()
        {
            var entities = InfoRepository.Instance.GetKnownEntities(InfoRepository.Instance.SelectedInfoCategory);

            if (entities == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(InfoRepository.Instance.SelectedInfoEntity))
            {
                InfoRepository.Instance.SelectedInfoEntity = entities[0].id;
            }

            var entitiesArray = entities.ToArray(); // FIXME: can we just use the iterator
            for (int i = 0; i < infoEntitiesParent.childCount; i++)
            {
                Transform cbt = infoEntitiesParent.GetChild(i);
                Text t = cbt.GetComponentInChildren<Text>(); // FIXME: Cache this

                if (entities != null && entitiesArray.Length > i)
                {
                    cbt.gameObject.SetActive(true); // Just in case it isn't active
                    cbt.name = entitiesArray[i].name;
                    t.text = entitiesArray[i].name;

                    var infoEntityDescriptor = cbt.gameObject.GetComponent<InfoEntityDescriptor>();

                    if (infoEntityDescriptor == null)
                    {
                        infoEntityDescriptor = cbt.gameObject.AddComponent<InfoEntityDescriptor>();
                    }

                    infoEntityDescriptor.id = entities[i].id;

                    if (InfoRepository.Instance.SelectedInfoEntity == entities[i].id)
                    {
                        EventSystem.current.SetSelectedGameObject(cbt.gameObject);
                    }
                }
                else
                {
                    cbt.gameObject.SetActive(false);
                }
            }
        }

        public void SelectEntity()
        {
            var infoEntityDescriptor = EventSystem.current.currentSelectedGameObject.GetComponent<InfoEntityDescriptor>();

            if (infoEntityDescriptor != null)
            {
                InfoRepository.Instance.SelectedInfoEntity = infoEntityDescriptor.id;
                RepaintDescription();
            }
        }

        public void RepaintDescription()
        {
            var entity = InfoRepository.Instance.GetEntity(InfoRepository.Instance.SelectedInfoEntity);

            if (entity != null)
            {
                description.text = entity.description;
            }
            else
            {
                description.text = "<entity not found>";
            }
        }
    }
    //}
}