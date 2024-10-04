using System.Collections;
using Alkawa.Core;
using Alkawa.Engine;
using UnityEngine;

namespace Alkawa.GameSystems
{
    public class Bootstrap : MonoBehaviourSingleton<Bootstrap>
    {
        
        [SerializeField] private VolatileBootstrap m_volatileBootstrap;

        protected override void OnSingletonAwake()
        {
            BackgroundLoadingContextHandler.Apply(BackgroundLoadingContext.FastLoad);
        }

        private void Start()
        {
            StartCoroutine(StartSequence());
        }


        private IEnumerator StartSequence()
        {
            AlkawaDebug.Log(ELogCategory.ENGINE, "Starting bootstrap scene.");

            PlatformVideoURLProvider.CacheVideoMasterFiles();

            yield return WaitUntilWwiseInit();
            
            m_volatileBootstrap.StartCoroutine(m_volatileBootstrap.PlayVideos());
        }
        
        private IEnumerator WaitUntilWwiseInit()
        {
            AlkawaDebug.Log(ELogCategory.ENGINE,"Waiting for AkSoundEngine.IsInitialized() init...");
            while (!AkSoundEngine.IsInitialized())
                yield return null;

            AlkawaDebug.Log(ELogCategory.ENGINE,"AkSoundEngine.IsInitialized().");
        }
    }
}