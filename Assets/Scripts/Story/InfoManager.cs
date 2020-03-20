﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            public List<Location> locations = new List<Location>();
            public Dictionary<string, InfoEntity> entities = new Dictionary<string, InfoEntity>();

            public string categoryResource = @"Info/Categories/";
            public string corpsResource = @"Info/Corps/";
            public string charactersResource = @"Info/Characters/";
            public string enemiesResource = @"Info/Enemies/";
            public string locationsResource = @"Info/Locations/";

            [Header("State")]
            public Dictionary<string, InfoEntity> knownEntities = new Dictionary<string, InfoEntity>();
            public List<Character> knownCharacters = new List<Character>();
            public List<Corp> knownCorps = new List<Corp>();
            public List<Enemy> knownEnemies = new List<Enemy>();
            public List<Enemy> uniqueEnemies = new List<Enemy>();
            public List<Location> knownLocations = new List<Location>();


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
                LoadLocations();

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
                    entities.Add(corp.id, corp);
                }
            }

            public void LoadCharacters()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(charactersResource);

                foreach (var file in jsonTextFiles)
                {
                    var character = JsonUtility.FromJson<Character>(file.text);
                    characters.Add(character);
                    entities.Add(character.id, character);
                    Debug.Log(this.name + " character added: " + character.id);
                }
            }

            public void LoadEnemies()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(enemiesResource);

                foreach (var file in jsonTextFiles)
                {
                    var character = JsonUtility.FromJson<Enemy>(file.text);
                    enemies.Add(character);
                    entities.Add(character.id, character);
                    Logger.Log(this.name, "enemy added - ", character.id);
                }
            }

            public void LoadLocations()
            {
                var jsonTextFiles = Resources.LoadAll<TextAsset>(locationsResource);

                foreach (var file in jsonTextFiles)
                {
                    var location = JsonUtility.FromJson<Location>(file.text);
                    locations.Add(location);
                    entities.Add(location.id, location);
                    Logger.Log(this.name, "location added - ", location.id);
                }

            }

            public List<InfoCategory> GetCategories()
            {
                return categories;
            }

            public InfoEntity GetEntity(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Character;
                }

                return null;

            }

            public Character GetCharacter(string id)
            {
                if(entities.ContainsKey(id))
                {
                    return entities[id] as Character;
                }

                return null;
            }

            public Enemy GetEnemy(string id)
            {
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Enemy;
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
                if (entities.ContainsKey(id))
                {
                    return entities[id] as Corp;
                }

                return null;
            }

            public List<InfoEntity> GetKnownEntities(string category)
            {
                var infocategory = categories.FirstOrDefault(e => e.id == category);

                if(infocategory == null)
                {
                    return null;
                }

                return knownEntities.Where(kv => kv.Value.category == category).Select(kv => kv.Value).ToList();
            }

            public Enemy GenerateRandomEnemy(string id)
            {
                Debug.Log(this.name + ": generating unique enemy " + id);
                int uniqueId = uniqueEnemies.Count;
                var enemy = GetEnemy(id);

                if (enemy == null)
                {
                    Debug.LogError(this.name + ": enemy not found: " + id);
                }

                var newEnemy = new Enemy
                {
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

            public bool AddToKnownEntities(string id)
            {
                if (!knownEntities.ContainsKey(id))
                {
                    if(entities.ContainsKey(id))
                    {
                        var entity = entities[id];
                        Logger.Log(name, "adding entity ", entity.id, " to known entities");
                        knownEntities.Add(id, entity);
                        ActionManager.instance.AddActionMessage("'" + entity.id + "' added to " + entity.category);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                        return false;
                    }
                }

                return false;
            }

            /*
            public void AddToKnownEnemies(string id)
            {
                if(!knownEntities.ContainsKey(id))
                {
                    var character = GetEnemy(id);
                    if(character != null)
                    {
                        Logger.Log(name, "adding entity ", character.id, " to known entities");
                        knownEntities.Add(id, character);
                        ActionManager.instance.AddActionMessage("'" + character.id + "' added to " + character.category);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                    }
                }
            }



            public void AddToKnownCharacters(string id)
            {
                if (!knownEntities.ContainsKey(id))
                {
                    var character = GetCharacter(id);
                    if (character != null)
                    {
                        Debug.Log(this.name + ": adding entity " + character.id + " to known entities");
                        character.SetRandomAttributes(); // Unlikely to do anything for NPCs
                        knownCharacters.Add(character);
                        ActionManager.instance.AddActionMessage("'" + character.id + "' added to " + character.category);
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
                        ActionManager.instance.AddActionMessage("'" + corp.id + "' added to " + corp.category);
                    }
                    else
                    {
                        Debug.LogError(this.name + ":  entity not found: " + id);
                    }
                }
            }
            */
        }
    }
}