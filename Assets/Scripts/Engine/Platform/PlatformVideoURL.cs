using System.IO;
using Alkawa.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Alkawa.Engine
{
    [System.Serializable]
    public struct PlatformVideoURL
    {
        [SerializeField]
        [HorizontalGroup("Split", 0.95f)]
        [Tooltip("Use this video file located in StreamingAssets/Video directory.")]
        private string m_videoInStreamingAssets;

        public string VideoInStreamingAssets => m_videoInStreamingAssets;

        public void SetVideoURL(string videoStreamingAssets)
        {
            m_videoInStreamingAssets = videoStreamingAssets;
        }

        public bool IsValid()
        {
            return m_videoInStreamingAssets != null && !string.IsNullOrEmpty(m_videoInStreamingAssets);
        }

        private bool TryFindVideoFile(string videoBasename, out string videoFilename)
        {
            if (PlatformVideoURLProvider.IsVideoFileAvailable(videoBasename, out videoFilename))
                return true;
            
            videoFilename = string.Empty;
            return false;
        }

        public string GetVideoURL()
        {
            if (!IsValid())
                return null;


            string videoBasename = Path.ChangeExtension(m_videoInStreamingAssets, GetTargetPreferredVideoExtension());
            TryFindVideoFile(videoBasename, out string videoFilename);

            return videoFilename;
        }

        public static string GetTargetPreferredVideoExtension()
        {
            return ".webm";
        }

#if UNITY_EDITOR_WIN
        private static readonly string EditorVideoStreamingAssetPath =
            Path.Combine(Path.Combine(PlatformVideoURLProvider.VideoFolderRoot, "Editor"));

        [HorizontalGroup("Split", 0.05f)]
        [Button("...")]
        private void BrowseVideo()
        {
            string browsedPath = EditorUtility.OpenFilePanelWithFilters("Select video file",
                EditorVideoStreamingAssetPath,
                new[] { "Video files", "asf,avi,dv,m4v,mov,mp4,mpg,mpeg,ogv,vp8,webm,wmv" });

            if (string.IsNullOrWhiteSpace(browsedPath)) // filter out cancel button
                return;

            // be sure to convert path to be comparable.
            string normalizedBrowsedPath = Path.GetFullPath(browsedPath).ToUpperInvariant();
            string normalizedVideoStreamingAssetPath =
                Path.GetFullPath(EditorVideoStreamingAssetPath).ToUpperInvariant();

            if (normalizedBrowsedPath.StartsWith(normalizedVideoStreamingAssetPath))
            {
                m_videoInStreamingAssets =
                    browsedPath.Substring(EditorVideoStreamingAssetPath.Length + 1); // +1 to also remove '/' at the end
            }
            else
            {
                AlkawaDebug.LogError($"File {browsedPath} is not in the directory {EditorVideoStreamingAssetPath}.");
            }
        }
#endif

        public void InitfromDeprecatedURL(string _deprecatedVideoAssetName)
        {
            m_videoInStreamingAssets = _deprecatedVideoAssetName;
        }
    }

}