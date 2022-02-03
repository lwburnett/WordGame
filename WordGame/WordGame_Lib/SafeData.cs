using System;

namespace WordGame_Lib
{
    public class SafeData<T>
    {
        public SafeData(Func<T, T> iDeepCopyFunc) :
            this(default, iDeepCopyFunc)
        {
        }

        public SafeData(T iInitialValue, Func<T, T> iDeepCopyFunc)
        {
            _lock = new object();
            _deepCopyFunc = iDeepCopyFunc;
            Value = iInitialValue;
        }

        public T Value
        {
            get
            {
                lock (_lock)
                {
                    return _deepCopyFunc(_value);
                }
            }
            set
            {
                lock (_lock)
                {
                    _value = _deepCopyFunc(value);
                }
            }
        }

        private T _value;
        private readonly Func<T, T> _deepCopyFunc;
        private readonly object _lock;
    }
}
