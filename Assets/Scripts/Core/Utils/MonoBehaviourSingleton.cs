using UnityEngine.Assertions;

namespace Alkawa.Core
{
    using UnityEngine;

    [DefaultExecutionOrder(-10000)]
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T Instance => m_instance;
        public static bool HasInstance => m_instance != null;

        protected virtual void OnSingletonAwake()
        {
        }

        protected virtual void OnSingletonDestroy()
        {
        }

        private void Awake()
        {
            if (!HasInstance)
            {
                AlkawaDebug.Log(ELogCategory.ENGINE, $"MonoBehaviourSingleton::Awake {this.GetType()}");
                m_instance = this as T;

                OnSingletonAwake();
            }
            else
            {
                Assert.IsTrue(false,
                    $"An instance of this Singleton GameObject (Instance of '{typeof(T)}') already exists! It should never happens!");
            }
        }

        private void OnDestroy()
        {
            if (m_instance == this as T)
            {
                OnSingletonDestroy();

                AlkawaDebug.Log(ELogCategory.ENGINE, $"MonoBehaviourSingleton::Destroy {this.GetType()}");
                m_instance = null;
            }
        }


        protected static bool s_applicationIsQuitting = false;

        private void OnApplicationQuit()
        {
            s_applicationIsQuitting = true;
        }

        public static void ResetApplicationIsQuitting()
        {
            s_applicationIsQuitting = false;
        }

        public static bool ApplicationIsQuitting => s_applicationIsQuitting;
    }
}