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

namespace Enhancer.Collections
{
    /// <summary>
    /// This class represents a series of indexes.
    /// </summary>
    public class IndexRange : IList<int>, IList
    {
        private sealed class IndexEnumerator : IEnumerator<int>
        {
            private int _index;
            private IndexRange _owner;

            public IndexEnumerator(IndexRange indexRange)
            {
                _owner = indexRange;
            }

            public int Current => _index;

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                return _index > _owner._end
                    || (_index += _owner._increments) > _owner._end;
            }

            public void Reset()
            {
                _index = _owner._start - _owner._increments;
            }
        }

        private int _start;
        private int _end;
        private int _increments;

        private object _syncRoot = new object();

        /// <summary>
        /// Gets the number of indexes this collection contains.
        /// </summary>
        public int Count => (_end - _start) / _increments + 1;

        bool ICollection<int>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => true;

        int ICollection.Count => Count;

        object ICollection.SyncRoot => _syncRoot;

        bool ICollection.IsSynchronized => true;

        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="index">The index of the value to get.</param>
        /// <returns>The index value at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the index exceeds the boundaries of the collection.
        /// </exception>
        /// <exception cref="NotSupportedException">If a value was assigned.</exception>
        public int this[int index]
        {
            get => index >= 0 && index < Count ? _start + index * _increments : throw new ArgumentOutOfRangeException(nameof(index));
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Creates a new <see cref="IndexRange"/> instance in the start and end indexes, with the specified increments.
        /// </summary>
        /// <param name="start">The first index of the collection.</param>
        /// <param name="end">The highest index that the collection still can contain.</param>
        /// <param name="increments">The increments in which to produce the indexes.</param>
        public IndexRange(int start, int end, int increments)
        {
            if (increments == 0)
            {
                throw new ArgumentException("The increments cannot be zero.", nameof(increments));
            }

            _start      = start;
            _end        = end;
            _increments = increments;
        }

        /// <inheritdoc/>
        public IEnumerator<int> GetEnumerator()
        {
            return new IndexEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(int item)
        {
            if (item > _end || item < _start || (item - _start) % _increments != 0)
            {
                return -1;
            }

            return (item - _start) / _increments;
        }

        /// <inheritdoc/>
        public bool Contains(int item) => item <= _end && item >= _start && (item - _start) % _increments == 0;

        /// <inheritdoc/>
        public void CopyTo(int[] array, int arrayIndex)
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
                throw new ArgumentException();
            }

            for (int i = 0; _start + i * _increments < _end; ++i)
            {
                array[i + arrayIndex] = _start + i * _increments;
            }
        }

        int IList.IndexOf(object value) => value is int ? IndexOf((int)value) : -1;

        bool IList.Contains(object value) => value is int && Contains((int)value);

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
                throw new ArgumentException();
            }

            for (int i = 0; _start + i * _increments < _end; ++i)
            {
                array.SetValue(_start + i * _increments, i + index);
            }
        }

        void IList<int>.Insert(int index, int item) => throw new NotSupportedException();

        void IList<int>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<int>.Add(int item) => throw new NotSupportedException();

        void ICollection<int>.Clear() => throw new NotSupportedException();

        bool ICollection<int>.Remove(int item) => throw new NotSupportedException();

        int IList.Add(object item) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
    }
}
