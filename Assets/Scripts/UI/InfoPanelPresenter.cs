using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackedDesign.Story
{
    public class InfoPanelPresenter : MonoBehaviour
    {
        public Transform infoCategoriesParent;
        public Transform infoEntitiesParent;
        public Transform infoDescriptionParent;
        InfoManager infoManager;
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

        public void Initialize(InfoManager infoManager, SelectMenuManager selectMenuManager)
        {
            this.infoManager = infoManager;
            this.selectMenuManager = selectMenuManager;
        }

        public void Repaint()
        {
            if (CoreGame.Instance.state.state == GameState.GameStateEnum.SELECTMENU && selectMenuManager.MenuState == SelectMenuManager.SelectMenuState.INFO)
            {
                if (!gameObject.activeInHierarchy)
                {
                    Show(true);
                }
            }
            else if (this.gameObject.activeInHierarchy)
            {
                Show(false);
            }

        }

        private void Show(bool flag)
        {
            Debug.Log("Set Info Panel " + flag);

            this.gameObject.SetActive(flag);

            if (!flag)
            {
                return;
            }

            RepaintCategories();
        }

        public void RepaintCategories()
        {

            EventSystem.current.SetSelectedGameObject(null);

            var categories = infoManager.categories.Where(e => e.available).ToList();

            if (string.IsNullOrWhiteSpace(infoManager.selectedInfoCategory))
            {
                infoManager.selectedInfoCategory = categories[0].id;
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

                    if (infoManager.selectedInfoCategory == categories[i].id)
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
                infoManager.selectedInfoCategory = infoEntityDescriptor.id;
                RepaintEntities();
            }
        }

        public void RepaintEntities()
        {
            var entities = infoManager.GetKnownEntities(infoManager.selectedInfoCategory);

            if (entities == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(infoManager.selectedInfoEntity))
            {
                infoManager.selectedInfoEntity = entities[0].id;
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

                    if (infoManager.selectedInfoEntity == entities[i].id)
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
                infoManager.selectedInfoEntity = infoEntityDescriptor.id;
                RepaintDescription();
            }
        }


        public void RepaintDescription()
        {

            //Transform cbt = infoDescriptionParent.GetChild (i);
            //Button b = cbt.GetComponent<Button> ();

            //Text t = infoDescriptionParent.GetComponentInChildren<Text>();

            var entity = infoManager.GetEntity(infoManager.selectedInfoEntity);

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