using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
#if UNITY_SWITCH
using UnityEngine.Switch;
using Alkawa.Plugins.Switch;
#endif

namespace Alkawa.Core
{
    public static class BackgroundLoadingSettingsApplyer
    {
#if UNITY_EDITOR
        private static bool _allowApply = false;//enable it to true if you want test behaviors
#else
        private static bool _allowApply = true;
#endif

        public static bool AllowApply => _allowApply;

        /// <summary>
        /// On Switch we enable the FastLoad ( 1Ghz -> 1.7 Ghz, less gpu available) 
        /// </summary>
        [Conditional("UNITY_SWITCH")]
        private static void SetCpuBoostFastLoad(bool enable)
        {
#if UNITY_SWITCH
            SwitchPerformanceEx.SetCpuBoostMode(enable ? Performance.CpuBoostMode.FastLoad : Performance.CpuBoostMode.Normal);
#endif
        }

        private static void SetAsyncUploadTimeSlice(int asyncUploadTimeSlice)
        {
            if (QualitySettings.asyncUploadTimeSlice != asyncUploadTimeSlice)
                QualitySettings.asyncUploadTimeSlice = asyncUploadTimeSlice;
        }

        private static void SetAsyncUploadBufferSize(int asyncUploadBufferSize)
        {
            if (QualitySettings.asyncUploadBufferSize != asyncUploadBufferSize)
                QualitySettings.asyncUploadBufferSize = asyncUploadBufferSize;
        }

        public static void Apply(BackgroundLoadingContext context, BackgroundLoadingSettings settings)
        {
            if (!_allowApply)
                return;

            if (Application.backgroundLoadingPriority != settings.backgroundLoadingPriority)
                Application.backgroundLoadingPriority = settings.backgroundLoadingPriority;

            SetAsyncUploadTimeSlice(settings.asyncUploadTimeSlice);
            SetAsyncUploadBufferSize(settings.asyncUploadBufferSize);

            SetCpuBoostFastLoad(settings.cpuBoostFastLoad);
        }
    }



}