using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhancer.Containers
{
    public class GenericCyclingVector<T> : ICollection<T>
    {
        ulong _size;
        ulong _first;
        ulong _length;

        T[] _vector;

        public class CyclingVectorEnumerator : IEnumerator<T>
        {
            ulong _index;
            GenericCyclingVector<T> _cvector;

            internal CyclingVectorEnumerator(GenericCyclingVector<T> cvector)
            {
                _cvector = cvector;
                _index = 0;
            }

            public T Current
            {
                get { return _cvector[_index]; }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_index + 1 >= _cvector._length)
                    return false;
                ++_index;
                return true;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public GenericCyclingVector(long maxSize)
        {
            Resize(maxSize);
        }

        public GenericCyclingVector(ulong maxSize)
        {
            Resize(maxSize);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CyclingVectorEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Resize(long maxSize)
        {
            if (maxSize < 0)
                throw new ArgumentException("The argument cannot be negative", "maxSize");
            Resize((ulong)maxSize);
        }

        public void Resize(ulong maxSize)
        {
            _size = maxSize;
            _first = 0;
            _length = 0;
            _vector = new T[_size];
        }

        public T this[ulong i]
        {
            get
            {
                if (i >= _length)
                    throw new ArgumentOutOfRangeException("i", i, "The index is out of range.");
                return _vector[(i + _first) % _size];
            }
            set
            {
                if (i >= _length)
                    throw new ArgumentOutOfRangeException("i", i, "The index is out of range.");
                _vector[(i + _first) % _size] = value;
            }
        }

        public T peek()
        {
            return _vector[_first];
        }

        public T pop()
        {
            T output = _vector[_first];
            // To allow GC to collect unused objects.
            _vector[_first] = default(T);
            _first = ++_first % _size;
            --_length;
            return output;
        }

        public bool push(T value)
        {
            if (_length == _size)
                return false;
            _vector[(_first + _length++) % _size] = value;
            return true;
        }

        public ulong Size { get { return _length; } }
        public ulong MaxSize { get { return _size; } set { Resize(value); } }

        public void Add(T item)
        {
            push(item);
        }

        public void Clear()
        {
            _length = 0;
        }

        public bool Contains(T item)
        {
            foreach (T element in this)
            {
                if (item.Equals(element)) return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if ((ulong)(array.Count() - arrayIndex) < _length)
                throw new ArgumentException("The array is too small", "array");
            for (ulong i = 0; i < _length; ++i)
                array[i + (ulong)arrayIndex] = this[i];
        }

        public int Count
        {
            get { return (int)_length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
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
            ulong i;
            for (i = 0; i < _length; ++i)
                if (this[i].Equals(item)) break;
            if (i >= _length) return false;
            for (; i < _length - 1; ++i)
                this[i] = this[i + 1];
            // To allow GC to collect unused objects.
            this[i] = default(T); --_length;
            return true;
        }

        public override string ToString()
        {
            return String.Format(" {{{1}}} - {{{2}}} : {0} ", _size, _vector[_first], _vector[(_first + _length - 1) % _size]);
        }
    }

    public class CyclingVector : GenericCyclingVector<object>
    {
        public CyclingVector(long maxSize) : base(maxSize) { }
        public CyclingVector(ulong maxSize) : base(maxSize) { }
    }
}
