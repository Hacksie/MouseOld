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
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.Log(builder.ToString());
#endif
        }

        public static void Log(UnityEngine.Object context, params string[] messages)
        {
#if UNITY_EDITOR
            StringBuilder builder = new StringBuilder(context.name);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.Log(builder.ToString(), context);
#endif
        }


        public static void LogError(string gameObject, params string[] messages)
        {
#if UNITY_EDITOR            
            StringBuilder builder = new StringBuilder(gameObject);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogError(builder.ToString());
#endif
        }

        public static void LogError(UnityEngine.Object context, params string[] messages)
        {
#if UNITY_EDITOR            
            StringBuilder builder = new StringBuilder(context.name);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogError(builder.ToString(), context);
#endif
        }

        public static void LogWarning(string gameObject, params string[] messages)
        {
#if UNITY_EDITOR
            StringBuilder builder = new StringBuilder(gameObject);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogWarning(builder.ToString());
#endif
        }

        public static void LogWarning(UnityEngine.Object context, params string[] messages)
        {
#if UNITY_EDITOR
            StringBuilder builder = new StringBuilder(context.name);
            builder.Append(" - ");
            foreach (var s in messages)
            {
                builder.Append(s);
            }

            Debug.LogWarning(builder.ToString(), context);
#endif
        }
    }
}
