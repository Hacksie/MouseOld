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
            public List<Enemy> enemies = new List<Enemy>();

            public string categoryResource = @"Info/Categories/";
            public string corpsResource = @"Info/Corps/";
            public string charactersResource = @"Info/Characters/";
            public string enemiesResource = @"Info/Enemies/";

            [Header("State")]
            //public List<InfoEntity> knownEntities = new List<InfoEntity>();
            public List<Character> knownCharacters = new List<Character>();
            public List<Corp> knownCorps = new List<Corp>();
            public List<Enemy> knownEnemies = new List<Enemy>();
            public List<Enemy> uniqueEnemies = new List<Enemy>();


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
                LoadEnemies();

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
                    characters.Add(character);
                    Debug.Log(this.name + " character added: " + character.id);
                }
            }

            public void LoadEnemies()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(enemiesResource);

                foreach (var file in jsonTextFiles)
                {
                    var character = JsonUtility.FromJson<Enemy>(file.text);

                    //character.SetRandomAttributes();
                    enemies.Add(character);
                    //entities.Add(character);
                    Debug.Log(this.name + " enemy added: " + character.id);
                }                
            }

            public List<InfoCategory> GetCategories()
            {
                return categories;
            }

            public Character GetCharacter(string id)
            {
                foreach (var v in characters)
                {
                    if (v.id == id)
                        return v;
                }
                return null;
            }

            public Enemy GetEnemy(string id)
            {
                foreach (var v in enemies)
                {
                    if (v.id == id)
                        return v;
                }
                return null;
            }

            public Enemy GetUniqueEnemy(string uniqueId)
            {
                foreach (var v in uniqueEnemies)
                {
                    if (v.uniqueId == uniqueId)
                        return v;
                }
                return null;
            }

            public Corp GetCorp(string id)
            {
                foreach (var v in corps)
                {
                    if (v.id == id)
                        return v;
                }
                return null;
            }            

            public void AddToKnownEnemies(string id)
            {
                if (!knownEnemies.Exists(e => e.id == id))
                {
                    var character = GetEnemy(id);
                    if (character != null)
                    {
                        Debug.Log(this.name + ": adding entity " + character.id + " to known entities");
                        knownEnemies.Add(character);
                        ActionManager.instance.AddActionMessage("'" + character.id + "' added to " + character.parentInfoCategory);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                    }
                }
            }

            public Enemy GenerateRandomEnemy(string id)
            {
                Debug.Log(this.name + ": generating unique enemy " + id);
                int uniqueId = uniqueEnemies.Count;
                var enemy = GetEnemy(id);

                if(enemy == null)
                {
                    Debug.LogError(this.name + ": enemy not found: " + id);
                }

                var newEnemy = new Enemy {
                    id = enemy.id,
                    uniqueId = enemy.id + uniqueId.ToString(),
                    name = enemy.name,
                    read = enemy.read,
                    parentInfoCategory = enemy.parentInfoCategory,
                    description = enemy.description,
                    handle = enemy.handle,
                    corp = enemy.corp,
                    serial = enemy.serial,
                    category = enemy.category,
                    body = enemy.body,
                    skin = enemy.skin,
                    eyes = enemy.eyes,
                    shirt = enemy.shirt,
                    pants = enemy.pants,
                    shoes = enemy.shoes,
                    hair = enemy.hair,
                    shirtcolor = enemy.shirtcolor,
                    pantscolor = enemy.pantscolor,
                    shoescolor = enemy.shoescolor,
                    haircolor = enemy.haircolor,
                    spriteOffset = enemy.spriteOffset
                };

                newEnemy.SetRandomAttributes();

                uniqueEnemies.Add(newEnemy);

                return newEnemy;

            }

            public void AddToKnownCharacters(string id)
            {
                if (!knownCharacters.Exists(e => e.id == id))
                {
                    var character = characters.Find(e => e.id == id);
                    if (character != null)
                    {
                        Debug.Log(this.name + ": adding entity " + character.id + " to known entities");
                        character.SetRandomAttributes(); // Unlikely to do anything for NPCs
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