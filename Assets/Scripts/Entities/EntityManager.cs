using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Entity
    {
        public class EntityManager : MonoBehaviour
        {
            public static EntityManager instance;

            [Header("Prefabs")]
            public List<GameObject> enemies = null;
            public List<GameObject> traps = null;
            [SerializeField]
            private List<GameObject> npcPrefabList = null;

            private List<Entity.BaseEntity> npcPool = new List<Entity.BaseEntity>();


            EntityManager()
            {
                instance = this;
            }



            public void Initialize(GameObject npcParent)
            {
                if (npcParent == null)
                {
                    Debug.LogError(this.name + ": npcParent is null");
                }

                foreach (var npc in npcPrefabList)
                {
                    if (npc == null)
                    {
                        continue;
                    }

                    var go = GameObject.Instantiate(npc, Vector3.zero, Quaternion.identity, npcParent.transform);
                    go.name = npc.name; // We don't want the (cloned) label
                    Debug.Log(this.name + ": instantiating NPC " + npc.name + " " + go.name);
                    Entity.BaseEntity entity = go.GetComponent<Entity.BaseEntity>();

                    npcPool.Add(entity);
                    go.SetActive(false);
                }
            }

            public BaseEntity GetPooledNPC(string name)
            {
                //return null;
                return npcPool.FirstOrDefault(g => g.gameObject.name == name);

            }

        }
    }
}