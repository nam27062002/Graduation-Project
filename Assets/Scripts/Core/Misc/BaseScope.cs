using System;

namespace Alkawa.Core
{
    public abstract class BaseScope : IDisposable
    {
        private bool m_Disposed;

        internal virtual void Dispose(bool disposing)
        {
            if (this.m_Disposed)
                return;
            if (disposing)
                this.CloseScope();
            this.m_Disposed = true;
        }

        ~BaseScope()
        {
            if (!this.m_Disposed)
                AlkawaDebug.Log(this.GetType().Name + " was not disposed! You should use the 'using' keyword or manually call Dispose.");
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected abstract void CloseScope();
    }



}