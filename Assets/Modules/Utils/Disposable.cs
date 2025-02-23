using System;

namespace Modules.Utils
{
    public struct Disposable : IDisposable
    {
        private Action _callback;
        public bool Disposed { get; private set; }

        public Disposable(Action callback)
        {
            _callback = callback;
            Disposed = false;
        }
        
        public void Dispose()
        {
            Disposed = true;
            _callback?.Invoke();
            _callback = null;
        }
    }
}