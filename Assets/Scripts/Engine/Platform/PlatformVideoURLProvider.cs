using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Alkawa.Engine
{
    public static class PlatformVideoURLProvider
    {
        public static readonly string VideoFolderRoot = Path.Combine(Application.streamingAssetsPath, "Video");
        private static readonly Dictionary<string, string> s_dicVideoFileAvailable = new(StringComparer.OrdinalIgnoreCase);

        [RuntimeInitializeOnLoadMethod]
        public static void Reset()
        {
            s_dicVideoFileAvailable.Clear();
        }

        public static bool IsVideoFileAvailable(string videoBasename, out string videoFilename)
        {
            return s_dicVideoFileAvailable.TryGetValue(videoBasename, out videoFilename);
        }

        private static string GetPlatformFolder()
        {
            return GetPlatformNameAtBootstrap();
        }

        private static string GetPlatformNameAtBootstrap()
        {
#if UNITY_EDITOR
            return "EDITOR";
#elif UNITY_STANDALONE_WIN
            return "PC";
#endif
        }

        public static void CacheVideoMasterFiles()
        {
            var masterVideoFolderRoot = Path.Combine(VideoFolderRoot, GetPlatformFolder());
            if (!Directory.Exists(masterVideoFolderRoot)) return;
            var dirInfo = new DirectoryInfo(masterVideoFolderRoot);
            var files = dirInfo.GetFiles($"*{PlatformVideoURL.GetTargetPreferredVideoExtension()}",
                SearchOption.AllDirectories);
            foreach (var file in files)
                s_dicVideoFileAvailable.Add(file.Name, file.FullName);
        }
    }
}