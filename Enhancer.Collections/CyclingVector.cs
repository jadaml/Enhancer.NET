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
    /// <summary>
    /// Represents a first-in-last-out collection, who's elements can be
    /// accessed individually.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements.
    /// </typeparam>
    [DebuggerDisplay("Count: {Count}")]
    public class CyclingVector<T> : IList<T>, IList
    {
        /// <summary>The starting index of the first element.</summary>
        private int _first;
        /// <summary>The internal array.</summary>
        private T[] _vector;
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

                if (_index >= _cvector.Count)
                {
                    return false;
                }

                return ++_index < _cvector.Count;
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

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to access.</param>
        /// <returns>
        /// The element at the specified <paramref name="index"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When <paramref name="index"/> is less than zero, or greater than or
        /// equal to <see cref="Count"/>.
        /// </exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
                }

                lock (SyncRoot)
                {
                    return _vector[(index + _first) % Capacity];
                }
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");
                }

                lock (SyncRoot)
                {
                    _vector[(index + _first) % Capacity] = value;
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

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating if the vector can be modified.
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets an object that can be used to synchronize assess to the
        /// <see cref="CyclingVector{T}"/>.
        /// </summary>
        public object SyncRoot { get; } = new object();

        /// <summary>
        /// Gets a value indicating whether access to the
        /// <see cref="CyclingVector{T}"/> is synchronized.
        /// </summary>
        /// <value>
        /// This property returns <see langword="true"/>.
        /// </value>
        public bool IsSynchronized => true;

        /// <summary>
        /// Gets a value indicating if the <see cref="CyclingVector{T}"/> size
        /// is fixed.
        /// </summary>
        /// <value>
        /// This property returns <see langword="false"/>.
        /// </value>
        public bool IsFixedSize => false;

        /// <summary>
        /// Gets the current capacity of this <see cref="CyclingVector{T}"/>.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Creates a new <see cref="CyclingVector{T}"/> instance.
        /// </summary>
        public CyclingVector() : this(128) { }

        /// <summary>
        /// Creates a new <see cref="CyclingVector{T}"/> instance, with a
        /// specified initial capacity.
        /// </summary>
        /// <param name="initialSize">
        /// The initial capacity of the vector.
        /// </param>
        public CyclingVector(int initialSize)
        {
            if (initialSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialSize), "Parameter must be a positive number.");
            }

            Capacity   = initialSize;
            _first  = 0;
            Count = 0;
            _vector = new T[initialSize];
        }

        /// <summary>
        /// Creates an iterator object which can be used to iterate through the
        /// elements of this <see cref="CyclingVector{T}"/> instance.
        /// </summary>
        /// <returns>
        /// The iterator object that can be used to iterate through the
        /// elements.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => new CyclingVectorEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void IncreaseStore()
        {
            T[] newVector;
            int newSize;

            lock (SyncRoot)
            {
                newSize = Capacity * Capacity;
                newVector = new T[newSize];
                CopyTo(newVector, 0);
                _vector = newVector;
                Capacity = newSize;
                _first = 0;
            }
        }

        /// <summary>
        /// Previews the next element without removing it.
        /// </summary>
        /// <returns>
        /// The next element of the vector, or the default value of
        /// <typeparamref name="T"/> if there is no element in the vector.
        /// </returns>
        public T Peek()
        {
            lock (SyncRoot)
            {
                return _vector[_first];
            }
        }

        /// <summary>
        /// Removes the next element of the vector.
        /// </summary>
        /// <returns>
        /// The removed next element, or the default value of
        /// <typeparamref name="T"/> if no item could be removed.
        /// </returns>
        public T Pop()
        {
            if (Count == 0) return default(T);

            T output;

            lock (SyncRoot)
            {
                output = _vector[_first];
                // To allow GC to collect unused objects.
                _vector[_first] = default(T);
                _first = ++_first % Capacity;

                --Count;

                InvalidateEnumerators();
            }

            return output;
        }

        /// <summary>
        /// Push a new element in the vector, if that is possible.
        /// </summary>
        /// <param name="value">The value to add to the Vector.</param>
        /// <returns>
        /// <see langword="true"/> if the push was successful,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public void Push(T value)
        {
            lock (SyncRoot)
            {
                if (Count == Capacity)
                {
                    IncreaseStore();
                }

                _vector[(_first + Count++) % Capacity] = value;

                InvalidateEnumerators();
            }
        }

        /// <summary>
        /// Adds a new element at the end of the <see cref="CyclingVector{T}"/>.
        /// </summary>
        /// <param name="item">The element to add to the collection.</param>
        public void Add(T item) => Push(item);

        /// <summary>
        /// Removes all the elements from this <see cref="CyclingVector{T}"/>.
        /// </summary>
        public void Clear()
        {
            lock (SyncRoot)
            {
                Array.Clear(_vector, 0, Capacity);

                Count = 0;

                InvalidateEnumerators();
            }
        }

        /// <summary>
        /// Checks if this <see cref="CyclingVector{T}"/> contains the specified
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="item"/> could be found
        /// in this collection, otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains(T item)
        {
            lock (SyncRoot)
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

        /// <summary>
        /// Copies the elements of this <see cref="CyclingVector{T}"/> to the
        /// specified <paramref name="array"/> starting at the specified
        /// <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">The array to copy the elements to.</param>
        /// <param name="arrayIndex">
        /// The zero-based index to start copying in to the
        /// <paramref name="array"/>.
        /// </param>
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

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("The array is too small", nameof(array));
            }

            lock (SyncRoot)
            {
                for (int i = 0; i < Count; ++i)
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
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found and
        /// successfully removed, <see langword="false"/> otherwise.
        /// </returns>
        public bool Remove(T item)
        {
            int i;

            lock (SyncRoot)
            {
                for (i = 0; i < Count; ++i)
                {
                    if (this[i].Equals(item)) break;
                }

                if (i >= Count) return false;

                for (; i < Count - 1; ++i)
                {
                    this[i] = this[i + 1];
                }

                // To allow GC to collect unused objects.
                this[i] = default(T);
                --Count;

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

            if (array.Length - index < Count)
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

        /// <summary>
        /// Determines the index of the specified <paramref name="item"/> in
        /// this <see cref="CyclingVector{T}"/>.
        /// </summary>
        /// <param name="item">The element to look for.</param>
        /// <returns>
        /// The index of the specified <paramref name="item"/> in this
        /// <see cref="CyclingVector{T}"/>, or -1 if the element cannot be
        /// found.
        /// </returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (this[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds the specified <paramref name="item"/> at the specified
        /// <paramref name="index"/> shifting all the elements starting from
        /// <paramref name="index"/> to the next index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index to insert the specified <paramref name="item"/>
        /// to.
        /// </param>
        /// <param name="item">
        /// The mew element to add to this <see cref="CyclingVector{T}"/>.
        /// </param>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (Count == Capacity)
            {
                IncreaseStore();
            }

            for (int c = Count++; c > index; --c)
            {
                _vector[(_first + c) % Capacity] = _vector[(_first + c - 1) % Capacity];
            }

            _vector[(_first + index) % Capacity] = item;
        }

        /// <summary>
        /// Removes the element from the specified <paramref name="index"/>
        /// shifting all succeeding element to fill its place.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            for (int c = index; c <= Count; ++c)
            {
                _vector[(_first + c) % Capacity] = _vector[(_first + c + 1) % Capacity];
            }

            _vector[(_first + --Count) % Capacity] = default(T);
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

            return Count - 1;
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
