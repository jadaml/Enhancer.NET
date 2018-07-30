/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Collections.
 *
 * Enhancer.Collections is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Collections is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Collections.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Enhancer.Collections
{
    [DebuggerDisplay("Count: {Count}")]
    public class CyclingVector<T> : IList<T>, IList
    {
        /// <summary>The size of the internal array.</summary>
        private int _size;
        /// <summary>The starting index of the first element.</summary>
        private int _first;
        /// <summary>The number of element the vector is currently holding.</summary>
        private int _length;
        /// <summary>The internal array.</summary>
        private T[] _vector;

        private object _syncRoot = new object();

        private List<WeakReference<CyclingVectorEnumerator>> _enumerators = new List<WeakReference<CyclingVectorEnumerator>>();

        private class CyclingVectorEnumerator : IEnumerator<T>
        {
            internal bool _invalid = false;

            private int              _index;
            private CyclingVector<T> _cvector;
            private bool             _disposed = false;

            internal CyclingVectorEnumerator(CyclingVector<T> cvector)
            {
                _cvector = cvector;

                Reset();

                lock (((ICollection)_cvector._enumerators).SyncRoot)
                {
                    _cvector._enumerators.Add(new WeakReference<CyclingVectorEnumerator>(this));
                }
            }

            public T Current
            {
                get
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException(nameof(CyclingVectorEnumerator));
                    }

                    return _cvector[_index];
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    lock (((ICollection)_cvector._enumerators).SyncRoot)
                    {
                        _cvector._enumerators.RemoveAll(wr => wr.TryGetTarget(out CyclingVectorEnumerator enumerator) && enumerator == this);
                    }
                }

                _disposed = true;
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(CyclingVectorEnumerator));
                }

                if (_invalid)
                {
                    throw new InvalidOperationException("The collection is changed.");
                }

                if (_index >= _cvector._length)
                {
                    return false;
                }

                return ++_index < _cvector._length;
            }

            public void Reset()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(CyclingVectorEnumerator));
                }

                _index = -1;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
                }

                lock (_syncRoot)
                {
                    return _vector[(index + _first) % _size];
                }
            }
            set
            {
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
                }

                lock (_syncRoot)
                {
                    _vector[(index + _first) % _size] = value;
                }
            }
        }

        object IList.this[int index]
        {
            get => this[index];
            set
            {
                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException icex)
                {
                    throw InvalidValue(value, nameof(value), icex);
                }
            }
        }

        public int Count => _length;

        /// <summary>
        /// Gets a value indicating if the vector can be modified.
        /// </summary>
        public bool IsReadOnly => false;

        object ICollection.SyncRoot => _syncRoot;

        public bool IsSynchronized => true;

        public bool IsFixedSize => false;

        public int Capacity => _size;

        public CyclingVector() : this(128) { }

        /// <summary>
        /// Creates a new <see cref="CyclingVector{T}"/> instance.
        /// </summary>
        /// <param name="initialSize">The initial size of the vector.</param>
        public CyclingVector(int initialSize)
        {
            if (initialSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialSize), "Parameter must be a positive number.");
            }

            _size   = initialSize;
            _first  = 0;
            _length = 0;
            _vector = new T[initialSize];
        }

        public IEnumerator<T> GetEnumerator() => new CyclingVectorEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void IncreaseStore()
        {
            T[] newVector;
            int newSize;

            lock (_syncRoot)
            {
                newSize = _size * _size;
                newVector = new T[newSize];
                CopyTo(newVector, 0);
                _vector = newVector;
                _size = newSize;
                _first = 0;
            }
        }

        /// <summary>
        /// Previews the first element without removing it.
        /// </summary>
        /// <returns>The first element of the vector, or the default value
        /// of T if there is no element in the vector.</returns>
        public T Peek()
        {
            lock (_syncRoot)
            {
                return _vector[_first];
            }
        }

        /// <summary>
        /// Removes the first element of the vector.
        /// </summary>
        /// <returns>The removed first element, or the default value if no item could be removed.</returns>
        public T Pop()
        {
            if (_length == 0) return default(T);

            T output;

            lock (_syncRoot)
            {
                output = _vector[_first];
                // To allow GC to collect unused objects.
                _vector[_first] = default(T);
                _first = ++_first % _size;

                --_length;

                InvalidateEnumerators();
            }

            return output;
        }

        /// <summary>
        /// Push a new element in the vector, if that is possible.
        /// </summary>
        /// <param name="value">The value to add to the Vector.</param>
        /// <returns><c>true</c> if the push was successful, <c>false</c> otherwise.</returns>
        public void Push(T value)
        {
            lock (_syncRoot)
            {
                if (_length == _size)
                {
                    IncreaseStore();
                }

                _vector[(_first + _length++) % _size] = value;

                InvalidateEnumerators();
            }
        }

        public void Add(T item) => Push(item);

        public void Clear()
        {
            lock (_syncRoot)
            {
                Array.Clear(_vector, 0, _size);

                _length = 0;

                InvalidateEnumerators();
            }
        }

        public bool Contains(T item)
        {
            lock (_syncRoot)
            {
                foreach (T element in this)
                {
                    if (item.Equals(element))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < _length)
            {
                throw new ArgumentException("The array is too small", nameof(array));
            }

            lock (_syncRoot)
            {
                for (int i = 0; i < _length; ++i)
                {
                    array[i + arrayIndex] = this[i];
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the GenericCyclingVector&lt;T&gt;.
        /// </summary>
        /// <note>
        /// This operation is not fully supported yet.
        /// </note>
        /// <param name="item">The object to remove from the vector.</param>
        /// <returns><c>true</c> if <c>item</c> is found and successfully removed, <c>false</c> otherwise.</returns>
        public bool Remove(T item)
        {
            int i;

            lock (_syncRoot)
            {
                for (i = 0; i < _length; ++i)
                {
                    if (this[i].Equals(item)) break;
                }

                if (i >= _length) return false;

                for (; i < _length - 1; ++i)
                {
                    this[i] = this[i + 1];
                }

                // To allow GC to collect unused objects.
                this[i] = default(T);
                --_length;

                InvalidateEnumerators();
            }

            return true;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < _length)
            {
                throw new ArgumentException("The array is too small", nameof(array));
            }

            int i;
            IEnumerator enumerator;

            for (i = 0, enumerator = GetEnumerator(); enumerator.MoveNext(); ++i)
            {
                array.SetValue(enumerator.Current, i + index);
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _length; ++i)
            {
                if (this[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (_length == _size)
            {
                IncreaseStore();
            }

            for (int c = _length++; c > index; --c)
            {
                _vector[(_first + c) % _size] = _vector[(_first + c - 1) % _size];
            }

            _vector[(_first + index) % _size] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            for (int c = index; c <= _length; ++c)
            {
                _vector[(_first + c) % _size] = _vector[(_first + c + 1) % _size];
            }

            _vector[(_first + --_length) % _size] = default(T);
        }

        private void InvalidateEnumerators()
        {
            lock (((ICollection)_enumerators).SyncRoot)
            {
                List<WeakReference<CyclingVectorEnumerator>> toRemove = new List<WeakReference<CyclingVectorEnumerator>>();

                foreach (WeakReference<CyclingVectorEnumerator> enumeratorReference in _enumerators)
                {
                    if (enumeratorReference.TryGetTarget(out CyclingVectorEnumerator enumerator))
                    {
                        enumerator._invalid = true;
                    }

                    toRemove.Add(enumeratorReference);
                }

                toRemove.ForEach(wr => _enumerators.Remove(wr));
            }
        }

        private Exception InvalidValue(object value, string argName, Exception innerEx = null)
        {
            return new ArgumentException($"Cannot add the specified value, because it isn't of type {typeof(T)}.", argName, innerEx);
        }

        int IList.Add(object value)
        {
            try
            {
                Add((T)value);
            }
            catch (InvalidCastException icex)
            {
                throw InvalidValue(value, nameof(value), icex);
            }

            return _length - 1;
        }

        bool IList.Contains(object value) => value is T && _vector.Contains((T)value);

        int IList.IndexOf(object value) => value is T ? IndexOf((T)value) : -1;

        void IList.Insert(int index, object value)
        {
            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException icex)
            {
                throw InvalidValue(value, nameof(value), icex);
            }
        }

        void IList.Remove(object value)
        {
            if (!typeof(T).IsAssignableFrom(value.GetType()))
            {
                return;
            }

            Remove((T)value);
        }
    }
}
