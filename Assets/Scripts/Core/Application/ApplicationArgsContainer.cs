using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Alkawa.Core
{
#if UNITY_EDITOR
    public class UserArgsKeys
    {
        public static readonly string ArgumentsAssetKey = "POP_UserArgsWindow_arguments";
        public static readonly string UseArgumentsKey = "POP_UserArgsWindow_useArguments";
        public static readonly string transientUseArgumentsKey = "POP_UserArgsWindow_transientArguments";
    }
 #endif
    public class ApplicationArgsContainer
    {
        public ApplicationArgsContainer()
        {
#if UNITY_EDITOR
            string transientArguments = EditorPrefs.GetString(UserArgsKeys.transientUseArgumentsKey, string.Empty);

            string assetPath = string.Empty;
            
            if (!transientArguments.IsNullOrEmpty())
            {
                assetPath = transientArguments;
            }

            if (EditorPrefs.GetBool(UserArgsKeys.UseArgumentsKey, false))
            {
                assetPath = EditorPrefs.GetString(UserArgsKeys.ArgumentsAssetKey, string.Empty);
            }

            if (!assetPath.IsNullOrEmpty())
            {
                var asset = AssetDatabase.LoadAssetAtPath<UserArgs>(assetPath);
                if (asset != null)
                {
                    foreach (((string key, bool activated), string value) in asset.arguments)
                        if (activated)
                            PushKeyValue(key, value);
                }
            }
#endif
        }
        private bool IsAlwakaKeyArg(string arg)
        {
            return arg.StartsWith("-");
        }

        string ConvertKey(string key)
        {
            //remove the first character as '-' if first index is set;
            return (key[0]) == '-' ? key.Substring(1, key.Length - 1) : key;
        }

        public void PushKeyValue(string key, string value)
        {
            if (key.Length <= 1) //ignore this key
                return;

            m_dicKeyValue[ConvertKey(key)] = value;
        }

        void PushKeyAsTrue(string key)
        {
            if (key.Length <= 1) //ignore this key
                return;

            m_dicKeyValue[ConvertKey(key)] = true;
        }

        public bool HasKey(string key)
        {
            return m_dicKeyValue.ContainsKey(key);
        }
      
        public T GetValue<T>(string key, T _default)
        {
            if (m_dicKeyValue.TryGetValue(key,out object value))
            {
                T result;
                if (ObjectTypeUtils.ConvertToType<T>(value, out result))
                {
                    return result;
                }

            }

            return _default;
        }

        public void Gather(string[] args)
        {
            if (args.Length == 0)
                return;

            string currentArgKey = string.Empty;

            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                var currentArg = args[argIndex];
                bool isAlwakaKeyArg = IsAlwakaKeyArg(currentArg);//Alkawa key arg required always a value to define the behavior
                if (isAlwakaKeyArg) //it's an argument ? 
                {
                    if (!string.IsNullOrEmpty(currentArgKey))
                    {
                        PushKeyAsTrue(currentArgKey);
                    }

                    currentArgKey = currentArg;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentArgKey))
                    {
                        PushKeyValue(currentArgKey, currentArg);
                    }
                    else if (isAlwakaKeyArg)
                    {
                        throw new ArgumentException($"Argument key required {currentArg}");
                    }

                    currentArgKey = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(currentArgKey))
                PushKeyAsTrue(currentArgKey);//may missing a value associated

        }

        private Dictionary<string, object> m_dicKeyValue = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void Dump()
        {
            foreach (var keyValue in m_dicKeyValue)
            {
                UnityEngine.Debug.Log($"Key:{keyValue.Key}-->{keyValue.Value.ToString()}");
            }
        }

        public string[] GetAsArray()
        {
            string[] args = new string[m_dicKeyValue.Count];
            int index = 0;
            foreach (var (key,value) in m_dicKeyValue)
            {
                args[index] = $"{key}={value}";
                index++;
            }
                
            return args;
        }
    }


}
