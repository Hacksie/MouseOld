using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        public class InfoManager : MonoBehaviour
        {

            public static InfoManager instance;

            [Header("Config")]
            public List<InfoCategory> categories = new List<InfoCategory>();
            public List<Character> characters = new List<Character>();
            public List<Corp> corps = new List<Corp>();

            public string categoryResource = @"Info/Categories/";
            public string corpsResource = @"Info/Corps/";
            public string charactersResource = @"Info/Characters/";

            [Header("State")]
            //public List<InfoEntity> knownEntities = new List<InfoEntity>();
            public List<Character> knownCharacters = new List<Character>();
            public List<Corp> knownCorps = new List<Corp>();


            public string selectedInfoCategory;
            public string selectedInfoEntity;

            public InfoManager()
            {
                instance = this;
            }

            public void Initialize()
            {
                LoadCategories();
                LoadCorps();
                LoadCharacters();

            }

            public void LoadCategories()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(categoryResource);

                foreach (var file in jsonTextFiles)
                {
                    var cat = JsonUtility.FromJson<InfoCategory>(file.text);
                    Debug.Log(this.name + " category added: " + cat.id);
                    categories.Add(cat);
                }
            }

            public void LoadCorps()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(corpsResource);

                foreach (var file in jsonTextFiles)
                {
                    var corp = JsonUtility.FromJson<Corp>(file.text);
                    Debug.Log(this.name + " corp added: " + corp.id);
                    corps.Add(corp);
                    //entities.Add(corp);
                }
            }

            public void LoadCharacters()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(charactersResource);

                foreach (var file in jsonTextFiles)
                {
                    var character = JsonUtility.FromJson<Character>(file.text);

                    character.SetRandomAttributes();
                    characters.Add(character);
                    //entities.Add(character);
                    Debug.Log(this.name + " character added: " + character.id);
                }
            }

            public List<InfoCategory> GetCategories()
            {
                return categories;
            }

            public Character GetCharacter(string id)
            {
                return characters.Find(e => e.id == id);
            }

            public Corp GetCorp(string id)
            {
                return corps.Find(e => e.id == id);
            }            

            // public InfoEntity GetEntity(string id)
            // {
            //     return entities.Find(e => e.id == id);
            // }

            public void AddToKnownCharacters(string id)
            {
                if (!knownCharacters.Exists(e => e.id == id))
                {
                    var character = characters.Find(e => e.id == id);
                    if (character != null)
                    {
                        Debug.Log(this.name + ": adding entity " + character.id + " to known entities");
                        knownCharacters.Add(character);
                        ActionManager.instance.AddActionMessage("'" + character.id + "' added to " + character.parentInfoCategory);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                    }
                }
            }
            public void AddToKnownCorps(string id)
            {
                if (!knownCorps.Exists(e => e.id == id))
                {
                    var corp = corps.Find(e => e.id == id);
                    if (corp != null)
                    {
                        Debug.Log(this.name + ": adding entity " + corp.id + " to known entities");
                        knownCorps.Add(corp);
                        ActionManager.instance.AddActionMessage("'" + corp.id + "' added to " + corp.parentInfoCategory);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                    }
                }
            }

            // public void AddToKnownEntities(string id)
            // {
            //     if (!knownEntities.Exists(e=> e.id == id))
            //     {
            // 		var entity = entities.Find(e => e.id == id);
            // 		if(entity != null)
            // 		{
            //         Debug.Log(this.name + ": adding entity " + entity.id + " to known entities");
            //         knownEntities.Add(entity);
            // 		ActionManager.instance.AddActionMessage("'" + entity.id + "' added to " + entity.parentInfoCategory);
            // 		}
            // 		else 
            // 		{
            // 			Debug.LogError(this.name + ":  entity not found: " + id);
            // 		}
            //     }
            // }
        }
    }
}