using System;
using System.Collections;
using UnityEngine;

namespace Alkawa.Core
{
    public static class CoroutineHelper
    {
        public static IEnumerator WaitForSeconds(float _timeInSec)
        {
            float startTime = Time.unscaledTime;
            while (Time.unscaledTime - startTime <= _timeInSec)
                yield return null;
        }
        
        public static IEnumerator WaitForFramesCount(int _frameCount)
        {
            for (int i=0; i < _frameCount; i++)
                yield return null;
        }
    }
    
    
    public sealed class WaitForDone : CustomYieldInstruction
    {
        private Func<bool> m_Predicate;
        private float m_timeout;
        private bool WaitForDoneProcess()
        {
            m_timeout -= Time.deltaTime;
            return m_timeout <= 0f || m_Predicate();
        }

        public override bool keepWaiting => !WaitForDoneProcess();

        public WaitForDone(float timeout, Func<bool> predicate)
        {
            m_Predicate = predicate;
            m_timeout = timeout;
        }
    }
}