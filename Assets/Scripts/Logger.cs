using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    static class Logger
    {
        public static void Log(string gameObject, params string[] messages)
        {
#if UNITY_EDITOR
            StringBuilder builder = new StringBuilder(gameObject);
            builder.Append(" - ");
            foreach(var s in messages)
            {
                builder.Append(s);
            }

            Debug.Log(builder.ToString());
#endif
        }
        public static void LogError(string gameObject, params string[] messages)
        {
            StringBuilder builder = new StringBuilder(gameObject);
            builder.Append(" - ");
            foreach(var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogError(builder.ToString());
        }
        public static void LogWarning(string gameObject, params string[] messages)
        {
            StringBuilder builder = new StringBuilder(gameObject);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogWarning(builder.ToString());
        }
    }
}
