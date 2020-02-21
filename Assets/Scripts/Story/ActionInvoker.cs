using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign.Story
{
    class ActionInvoker : MonoBehaviour
    {
        public string action = "";

        public void Start()
        {
            if (string.IsNullOrEmpty(action))
            {
                Logger.LogWarning(this.name, "action is empty");
            }
        }

        public void Invoke()
        {
            Debug.Log("Invoke");
            ActionManager.instance.Invoke(action);
        }
    }
}
