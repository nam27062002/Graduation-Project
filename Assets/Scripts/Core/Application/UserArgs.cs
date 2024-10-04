#if UNITY_EDITOR
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Alkawa.Core
{
    public class UserArgs : ScriptableObject
    {
        [Serializable] public struct ArgKey
        {
            public string value;
            public bool activated;

            public ArgKey(string value = null, bool activated = true)
            {
                this.value = value ?? string.Empty;
                this.activated = activated;
            }

            public static implicit operator ArgKey(string str) => new ArgKey(str);
            public void Deconstruct(out string value, out bool activated)
            {
                value = this.value;
                activated = this.activated;
            }
        }
        
        public SerializableDictionary<ArgKey, string> arguments = new SerializableDictionary<ArgKey, string>();

        public string ToArgsString()
        {
            StringBuilder sb = new StringBuilder(2048);

            foreach (((string key, bool activated), string value) in arguments)
            {
                if (!activated)
                    continue;
                
                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append($"{key} {value}");
            }

            return sb.ToString();
        }

        public static string GetDefaultAssetFolder()
        {
            return @"Assets/Editor/UserData/UserArgs";
        }
    }

    [CustomPropertyDrawer(typeof(UserArgs.ArgKey), true)]
    public class AlkawaErrorEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty activated = property.FindPropertyRelative("activated");
            SerializedProperty value = property.FindPropertyRelative("value");

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Rect togglePos = position; 
                //This could be as little as EditorGUIUtility.singleLineHeight (18.0 instead of 30.0) but the toggle might become non responsive in some nested case (eg. as a key in SerializableDictionary),
                //looking a the unity IMGUI debugger it seems to be due to the first call of this function (to precompute the layout) not taking into account the possible indent
                togglePos.width = 30; 
                position.xMin += togglePos.width;

                using (new EditorGUI.PropertyScope(togglePos, label, activated))
                    activated.boolValue = EditorGUI.Toggle(togglePos, activated.boolValue);
               
                using (new EditorGUI.PropertyScope(position, label, value))
                    value.stringValue = EditorGUI.TextField(position, value.stringValue);
            }
        }
    }
}

#endif