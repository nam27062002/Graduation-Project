using System.Collections;
using Alkawa.Core;
using Alkawa.Engine;
using UnityEngine;
using UnityEngine.Video;

namespace Alkawa.GameSystems
{
    public sealed class VolatileBootstrap : MonoBehaviour
    {
        [SerializeField] private VideoPlayer m_videoPlayer;

        [SerializeField] private VideoIntro[] m_introVideos;

        [SerializeField] private CanvasGroup m_loadingLayer;

        public bool PlayVideoIntro { get; set; } = true;

        private bool m_isVideoFinished = false;
        private Coroutine m_fadeCoroutine;

        public IEnumerator PlayVideos()
        {
            if (PlayVideoIntro)
            {
                BackgroundLoadingContextHandler.Apply(BackgroundLoadingContext.VideoPlayback);
                foreach (var clip in m_introVideos)
                {
                    yield return PlayVideo(clip);
                }

                BackgroundLoadingContextHandler.Apply(BackgroundLoadingContext.FastLoad);
            }

            m_videoPlayer.Stop();
            m_isVideoFinished = true;
            yield return CoroutineHelper.WaitForSeconds(2f);

            if (this != null)
                StartCoroutine(FadeInLoading(1f));
        }

        private IEnumerator PlayVideo(VideoIntro _videoIntro)
        {
            var url = _videoIntro.VideoURL;
            if (!url.IsValid())
                yield break;

            var soundEvent = _videoIntro.SoundEvent;
            AlkawaDebug.Log(ELogCategory.ENGINE, $"PlayVideo {url.VideoInStreamingAssets}");
            m_videoPlayer.url = url.GetVideoURL();

            m_videoPlayer.Prepare();
            while (!m_videoPlayer.isPrepared)
                yield return null;

            if (soundEvent.IsValid())
                AkSoundEngine.PostEvent(soundEvent.Id, gameObject);

            m_videoPlayer.Play();

            while (!m_videoPlayer.isPaused)
                yield return null;
        }

        public IEnumerator WaitVideoFinished()
        {
            while (!m_isVideoFinished)
            {
                yield return null;
            }
        }

        public IEnumerator FadeInLoading(float _duration)
        {
            if (m_fadeCoroutine != null)
                StopCoroutine(m_fadeCoroutine);

            m_fadeCoroutine = StartCoroutine(FadeInLoadingInternal(_duration));
            yield return m_fadeCoroutine;
        }

        public IEnumerator FadeOutLoading(float _duration)
        {
            if (m_fadeCoroutine != null)
                StopCoroutine(m_fadeCoroutine);

            m_fadeCoroutine = StartCoroutine(FadeOutLoadingInternal(_duration));
            yield return m_fadeCoroutine;
        }

        private IEnumerator FadeInLoadingInternal(float _duration)
        {
            if (m_loadingLayer.alpha >= 1f)
                yield break;

            float currentProgress = m_loadingLayer.alpha;
            float lerpStart = currentProgress;
            float adjustedDuration = 1f - (_duration * lerpStart);
            float startTime = Time.time;

            m_loadingLayer.alpha = Mathf.Lerp(lerpStart, 1f, currentProgress);
            while (currentProgress < 1f)
            {
                m_loadingLayer.alpha = Mathf.Lerp(lerpStart, 1f, currentProgress);
                yield return null;
                currentProgress = ((Time.time - startTime) / adjustedDuration);
            }

            m_loadingLayer.alpha = 1f;
            m_fadeCoroutine = null;
        }

        private IEnumerator FadeOutLoadingInternal(float _duration)
        {
            if (m_loadingLayer.alpha == 0f)
                yield break;

            float currentProgress = m_loadingLayer.alpha;
            float lerpStart = currentProgress;
            float adjustedDuration = _duration * lerpStart;
            float startTime = Time.time;

            m_loadingLayer.alpha = Mathf.Lerp(0f, lerpStart, currentProgress);
            while (currentProgress > 0f)
            {
                m_loadingLayer.alpha = Mathf.Lerp(0f, lerpStart, currentProgress);
                yield return null;
                currentProgress = ((Time.time - startTime) / adjustedDuration);
                currentProgress = 1f - currentProgress;
            }

            m_loadingLayer.alpha = 0f;

            m_fadeCoroutine = null;
        }
    }

    [System.Serializable]
    internal struct VideoIntro
    {
        [SerializeField] private PlatformVideoURL m_videoURL;
        public PlatformVideoURL VideoURL => m_videoURL;

        [SerializeField] private AK.Wwise.Event m_soundEvent;
        public AK.Wwise.Event SoundEvent => m_soundEvent;
    }
}