using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Alkawa.Core
{
    public class BackgroundLoadingContextHandler
    {
        static BackgroundLoadingContext _context;

        static BackgroundLoadingSettings[] _settings;

        public static bool KeepHistoric = false;

        private static List<BackgroundLoadingStateEntry> _backgroundLoadingStateEntries;


        public static BackgroundLoadingSettings[] BackgroundLoadingSettings => _settings;

        public static List<BackgroundLoadingStateEntry> BackgroundLoadingStateEntries => _backgroundLoadingStateEntries;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            //special case,the global application args is init later
            ApplicationArgsContainer args = new ApplicationArgsContainer();
            args.Gather(System.Environment.GetCommandLineArgs());
            KeepHistoric = args.GetValue(ApplicationArguments.BACKGROUND_LOADING_CONTEXT_HISTORIC, KeepHistoric);
#endif

            _context = BackgroundLoadingContext.Default;

            _backgroundLoadingStateEntries = new List<BackgroundLoadingStateEntry>();

            int poolSize = Enum.GetNames(typeof(BackgroundLoadingContext)).Length;

            int defaultAsyncUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
            int defaultAsyncUploadBufferSize = QualitySettings.asyncUploadBufferSize;

            ThreadPriority defaultBackgroundLoadingPriority = Application.backgroundLoadingPriority;

            BackgroundLoadingSettings defaultSettings = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority = defaultBackgroundLoadingPriority,
                asyncUploadTimeSlice = defaultAsyncUploadTimeSlice,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad = false
            };
            
            BackgroundLoadingSettings lowestSetting = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority =  ThreadPriority.Low,
                asyncUploadTimeSlice = defaultAsyncUploadTimeSlice,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad = false
            };

            BackgroundLoadingSettings lowestSettingWithCPUBoost = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority = ThreadPriority.Low,
                asyncUploadTimeSlice = defaultAsyncUploadTimeSlice,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad =
                    true //we boost the CPU while we are fading  (efficient for garbage collection,awake/enable processing)
            };

            BackgroundLoadingSettings highestAllowing60fpsSetting = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority = ThreadPriority.Normal,
                asyncUploadTimeSlice = 10,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad = true
            };
            
            var backgroundLoadingFastLoadSettings = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority = ThreadPriority.High,
                asyncUploadTimeSlice = 33,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad = true
            };
            
            var backgroundLoadingWithoutCPUBoost = new BackgroundLoadingSettings()
            {
                backgroundLoadingPriority = ThreadPriority.High,
                asyncUploadTimeSlice = 33,
                asyncUploadBufferSize = defaultAsyncUploadBufferSize,
                cpuBoostFastLoad = false
            };
            
            _settings = new BackgroundLoadingSettings[poolSize];
            
            SetSettings(BackgroundLoadingContext.Default, defaultSettings);
            SetSettings(BackgroundLoadingContext.Playing, lowestSetting);
            SetSettings(BackgroundLoadingContext.Idle, lowestSetting);
            SetSettings(BackgroundLoadingContext.FastTravelLoading, backgroundLoadingWithoutCPUBoost);
            
            SetSettings(BackgroundLoadingContext.Fading, lowestSettingWithCPUBoost); // unused.
            SetSettings(BackgroundLoadingContext.VideoPlayback, highestAllowing60fpsSetting);
            
            SetSettings(BackgroundLoadingContext.FastLoad, backgroundLoadingFastLoadSettings);
            SetSettings(BackgroundLoadingContext.InGameLoading, backgroundLoadingFastLoadSettings);

        }

        static void SetSettings(BackgroundLoadingContext context, BackgroundLoadingSettings settings)
        {
            _settings[(int)context] = settings;
        }

        public static BackgroundLoadingContext Context => _context;

        public static void Apply(BackgroundLoadingContext context)
        {
            _context = context;

            if (KeepHistoric)
            {
                float realTimeSinceStartup = Time.realtimeSinceStartup;
                if (_backgroundLoadingStateEntries.Count > 0)
                    _backgroundLoadingStateEntries[_backgroundLoadingStateEntries.Count - 1].endTimeSinceStartup = realTimeSinceStartup;

                _backgroundLoadingStateEntries.Add(new BackgroundLoadingStateEntry()
                {
                    context = context,
                    startTimeSinceStartup = realTimeSinceStartup
                });
            }

            BackgroundLoadingSettingsApplyer.Apply(_context, _settings[(int)_context]);

            Log($"{nameof(BackgroundLoadingContextHandler)}-Apply:'{_context}");
        }

        public static void Restore()
        {
            Log($"{nameof(BackgroundLoadingContextHandler)}-Restore:'RESTORE'");

            Apply(BackgroundLoadingContext.Default);
        }


        [Conditional("USE_ALKAWADEBUG")]
        private static void Log(string msg)
        {
            AlkawaDebug.Log(ELogCategory.ENGINE, msg);
        }
    }
}