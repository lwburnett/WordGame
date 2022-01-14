using System;
using System.Collections;
using System.Collections.Generic;

namespace WordGame_Lib
{
    public class OrderedUniqueList<T> : IList<T> where T : IComparable
    {
        public OrderedUniqueList()
        {
            _elements = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_elements.Count == 0)
            {
                _elements.Add(item);
                return;
            }
            
            var lower = 0;
            var upper = _elements.Count - 1;
            do
            {
                var ii = (int)Math.Floor((lower + upper) / 2.0f);
                var thisIterationElement = _elements[ii];
                var compareResult = item.CompareTo(thisIterationElement);
                if (compareResult < 0)
                    upper = ii - 1;
                else if (compareResult > 0)
                    lower = ii + 1;
                else
                    throw new InvalidOperationException($"Duplicate element {item}.");
            } while (upper > lower);

            lower = Math.Min(lower, upper);
            var lastIterationElement = _elements[lower];
            var lastCompareResult = item.CompareTo(lastIterationElement);
            if (lastCompareResult < 0)
                _elements.Insert(lower, item);
            else
            {
                var desiredIndex = lower + 1;
                if (desiredIndex <= _elements.Count - 1)
                    _elements.Insert(lower + 1, item);
                else
                    _elements.Add(item);
            }
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_elements.Count == 0)
            {
                return false;
            }

            var lower = 0;
            var upper = _elements.Count;
            while (lower <= upper)
            {
                var ii = (int)Math.Floor((lower + upper) / 2.0f);
                var thisIterationElement = _elements[ii];
                var compareResult = item.CompareTo(thisIterationElement);
                if (compareResult == 0)
                    return true;
                else if (compareResult > 0)
                    lower = ii + 1;
                else
                    upper = ii - 1;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length <= arrayIndex + _elements.Count)
                throw new IndexOutOfRangeException();

            for (var ii = 0; ii < _elements.Count; ii++)
            {
                var thisIterationArrayIndex = ii + arrayIndex;

                array[thisIterationArrayIndex] = _elements[ii];
            }
        }

        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_elements.Count == 0)
            {
                return false;
            }

            var lower = 0;
            var upper = _elements.Count;
            while (lower <= upper)
            {
                var ii = (int)Math.Floor((lower + upper) / 2.0f);
                var thisIterationElement = _elements[ii];
                var compareResult = item.CompareTo(thisIterationElement);
                if (compareResult == 0)
                {
                    _elements.RemoveAt(ii);
                    return true;
                }
                else if (compareResult > 0)
                    lower = ii + 1;
                else
                    upper = ii - 1;
            }

            return false;
        }

        public int Count => _elements.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            if (_elements.Count == 0)
            {
                return -1;
            }

            var lower = 0;
            var upper = _elements.Count;
            while (lower <= upper)
            {
                var ii = (int)Math.Floor((lower + upper) / 2.0f);
                var thisIterationElement = _elements[ii];
                var compareResult = item.CompareTo(thisIterationElement);
                if (compareResult == 0)
                    return ii;
                else if (compareResult > 0)
                    lower = ii + 1;
                else
                    upper = ii - 1;
            }
            
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("Cannot insert at a specific index.");
        }

        public void RemoveAt(int index)
        {
            _elements.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        private readonly List<T> _elements;
    }
}
