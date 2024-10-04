#if UNITY_EDITOR
#define USE_ALKAWADEBUG
#endif

using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Alkawa.Core
{
    public enum ELogCategory
    {
        NONE = 0,
        STREAMING,
        AUDIO,
        AI,
        QUEST,
        GAMEPLAY,
        UI,
        EDITOR,
        ANIMATION,
        FX,
        LD,
        ART,
        LOADSAVE,
        ENGINE,
        CINEMATIC,
        CINERECORDER,
        RUNTIME_INSTANTIATE
    }

    [Flags]
    public enum ELogFlags
    {
        NONE = 0,
        DATAERROR = 1 << 0
    }

    public static class AlkawaDebug
    {
        public enum ELogSeverity
        {
            INFO,
            WARNING,
            ERROR
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void Log(EUserRole roles, ELogCategory category, string message, Object context = null)
        {
            InternalLog(roles, category, ELogSeverity.INFO, message, context, ELogFlags.NONE);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void Log(EUserRole roles, string message, Object context = null)
        {
            Log(roles, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void Log(ELogCategory category, string message, Object context = null)
        {
            Log(EUserRole.All, category, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void Log(string message, Object context = null)
        {
            Log(EUserRole.All, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogWarning(EUserRole roles, ELogCategory category, string message, Object context = null)
        {
            InternalLog(roles, category, ELogSeverity.WARNING, message, context, ELogFlags.NONE);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogWarning(EUserRole roles, string message, Object context = null)
        {
            LogWarning(roles, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogWarning(ELogCategory category, string message, Object context = null)
        {
            LogWarning(EUserRole.All, category, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogWarning(string message, Object context = null)
        {
            LogWarning(EUserRole.All, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogError(EUserRole roles, ELogCategory category, string message, Object context = null)
        {
            InternalLog(roles, category, ELogSeverity.ERROR, message, context, ELogFlags.NONE);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogError(EUserRole roles, string message, Object context = null)
        {
            LogError(roles, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogError(ELogCategory category, string message, Object context = null)
        {
            LogError(EUserRole.All, category, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogError(string message, Object context = null)
        {
            LogError(EUserRole.All, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogDataError(EUserRole roles, ELogCategory category, string message, Object context = null)
        {
            InternalLog(roles, category, ELogSeverity.ERROR, message, context, ELogFlags.DATAERROR);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogDataWarning(EUserRole roles, ELogCategory category, string message, Object context = null)
        {
            InternalLog(roles, category, ELogSeverity.WARNING, message, context, ELogFlags.DATAERROR);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogDataError(EUserRole roles, string message, Object context = null)
        {
            LogDataError(roles, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogDataError(ELogCategory category, string message, Object context = null)
        {
            LogDataError(EUserRole.All, category, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        public static void LogDataError(string message, Object context = null)
        {
            LogDataError(EUserRole.All, ELogCategory.NONE, message, context);
        }

        [Conditional("USE_ALKAWADEBUG")]
        [Conditional("USE_ALKAWALOGGER")]
        public static void LogFinal(string message)
        {
#if USE_ALKAWADEBUG
            Log(message);
#endif
        }

        [Conditional("USE_ALKAWADEBUG")]
        private static void InternalLog(EUserRole roles, ELogCategory category, ELogSeverity severity, string message,
            Object context, ELogFlags flags)
        {
            var msg = $"{(flags != ELogFlags.NONE ? $"[{flags}] " : "")}{(category != ELogCategory.NONE ? $"[{category}] " : "")}{message}";
            switch (severity)
            {
                case ELogSeverity.INFO:
                    Debug.Log(msg, context);
                    break;
                case ELogSeverity.WARNING:
                    Debug.LogWarning(msg, context);
                    break;
                case ELogSeverity.ERROR:
                    Debug.LogError(msg, context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}
