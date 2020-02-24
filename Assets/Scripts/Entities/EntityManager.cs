using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    namespace Entities
    {
        public class EntityManager : MonoBehaviour
        {
            public static EntityManager instance;

            [Header("Prefabs")]
            public List<GameObject> enemies = null;
            public List<GameObject> traps = null;
            [SerializeField]
            private List<GameObject> npcPrefabList = null;

            private List<Entities.BaseEntity> npcPool = new List<Entities.BaseEntity>();
            public GameObject npcParent;


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
                    Entities.BaseEntity entity = go.GetComponent<Entities.BaseEntity>();

                    npcPool.Add(entity);
                    go.SetActive(false);
                }
            }

            public BaseEntity GetPooledNPC(string name)
            {
                //return null;
                for(int i=0;i<npcParent.transform.childCount;i++)
                {
                    if(npcParent.transform.GetChild(i).name == name)
                    {
                        return npcParent.transform.GetChild(i).gameObject.GetComponent<BaseEntity>();
                    }
                }
                return null;
                //return npcPool.FirstOrDefault(g => g.gameObject.name == name);

            }

        }
    }
}