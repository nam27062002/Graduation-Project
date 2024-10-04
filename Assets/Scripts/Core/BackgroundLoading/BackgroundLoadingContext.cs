using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Alkawa.Core
{
    //some references
    //https://blog.unity.com/technology/optimizing-loading-performance-understanding-the-async-upload-pipeline
    public enum BackgroundLoadingContext
    {
        Default = 0,//original behavior
        Playing,
        Fading,
        Idle,
        InGameLoading,
        FastLoad,
        VideoPlayback,
        FastTravelLoading,
    }
    public struct DefaultSettings
    {
        public int AsyncUploadTimeSlice;
        public int AsyncUploadBufferSize;

    }

    public struct BackgroundLoadingSettings
    {
        public ThreadPriority backgroundLoadingPriority;
        public int asyncUploadTimeSlice;
        public int asyncUploadBufferSize;
        public bool cpuBoostFastLoad;

    }

    public class BackgroundLoadingStateEntry
    {
        public BackgroundLoadingContext context;
        public float startTimeSinceStartup = 0.0f;
        public float endTimeSinceStartup = 0.0f;
    }
    

    public class BackgroundLoadingContextScope : BaseScope
    {
        public BackgroundLoadingContextScope(BackgroundLoadingContext context)
        {
            BackgroundLoadingContextHandler.Apply(context);
        }

        protected override void CloseScope()
        {
            BackgroundLoadingContextHandler.Restore();
        }

    }
}