using System;
using UnityEngine;

namespace Alkawa.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ScriptableSingletonParamsAttribute : Attribute
    {
        public string defaultPath { get; }
        public HideFlags hideFlags { get; } = HideFlags.None;
        public bool preloadAtLaunch { get; } = false;
        public int priority { get; } = 0;// negative order
        public string dlcDescriptorGuid { get; } = "";

        /// <summary>
        /// Set default path where to load singleton asset file
        /// </summary>
        /// <param name="_defaultPath">Default path need to be in Resources folder</param>
        public ScriptableSingletonParamsAttribute(string _defaultPath)
        {
            defaultPath = _defaultPath;
        }

        /// <summary>
        /// Set default path where to load singleton asset file
        /// </summary>
        /// <param name="_defaultPath">Default path need to be in Resources folder</param>
        /// <param name="_hideFlags">Hide flags for scriptable singleton</param>
        public ScriptableSingletonParamsAttribute(string _defaultPath, HideFlags _hideFlags) : this(_defaultPath)
        {
            hideFlags = _hideFlags;
        }


        /// <summary>
        /// Set default path where to load singleton asset file
        /// </summary>
        /// <param name="_defaultPath">Default path need to be in Resources folder</param>
        /// <param name="_preloadAtLaunch">Is ScriptableSingleton should be preloaded at game start</param>
        /// <param name="_priority">Loading priority negative order first</param>
        public ScriptableSingletonParamsAttribute(string _defaultPath, bool _preloadAtLaunch, int _priority = 0, string _dlcDescriptorGuid = "") : this(_defaultPath)
        {
            preloadAtLaunch = _preloadAtLaunch;
            priority = _priority;
            dlcDescriptorGuid = _dlcDescriptorGuid;
        }
        
        /// <summary>
        /// Set default path where to load singleton asset file
        /// </summary>
        /// <param name="_defaultPath">Default path need to be in Resources folder</param>
        /// <param name="_hideFlags">Hide flags for scriptable singleton</param>
        /// <param name="_preloadAtLaunch">Is ScriptableSingleton should be preloaded at game start</param>
        public ScriptableSingletonParamsAttribute(string _defaultPath, HideFlags _hideFlags, bool _preloadAtLaunch, string _dlcDescriptorGuid = "") : this(_defaultPath, _hideFlags)
        {
            preloadAtLaunch = _preloadAtLaunch;
            dlcDescriptorGuid = _dlcDescriptorGuid;
        }

    }
}